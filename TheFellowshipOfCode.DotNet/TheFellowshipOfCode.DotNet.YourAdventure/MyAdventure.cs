using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTF2020.Contracts;
using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;
using HTF2020.Contracts.Models.Adventurers;
using HTF2020.Contracts.Requests;
using TheFellowshipOfCode.DotNet.YourAdventure;

namespace TheFellowshipOfCode.DotNet.YourAdventure
{
    public class MyAdventure : IAdventure
    {
        private readonly Random _random = new Random();
        private readonly PathingChoice pathingChoice = new PathingChoice();
        private CharacterManagement characterManagement = CharacterManagement.GetInstance();

        public Task<Party> CreateParty(CreatePartyRequest request)
        {
            var party = new Party
            {
                Name = "My Party",
                Members = new List<PartyMember>()
            };

            for (var i = 0; i < request.MembersCount; i++)
            {
                party.Members.Add(new Fighter()
                {
                    Id = i,
                    Name = $"Member {i + 1}",
                    Constitution = 11,
                    Strength = 12,
                    Intelligence = 11 // 34 points
                });
            }

            return Task.FromResult(party);
        }

        public Task<Turn> PlayTurn(PlayTurnRequest request)
        {
            ExploredMap exploredMap = ExploredMap.GetInstance(request.Map.Tiles);
            exploredMap.UpdateEnemyAndLoot();
            characterManagement.CheckCharacter(request.PartyMember);

            if (request.PossibleActions.Contains(TurnAction.Loot))
            {
                return Task.FromResult(new Turn(TurnAction.Loot));
            }

            if (request.IsCombat && request.PartyMember.CurrentHealthPoints < 50 &&
                request.PossibleActions.Any(pa => pa == TurnAction.DrinkPotion))
            {
                return Task.FromResult(new Turn(TurnAction.DrinkPotion));
            }

            if (request.PossibleActions.Contains(TurnAction.Attack))
            {
                return Task.FromResult(new Turn(TurnAction.Attack));
            }

            // Get all treasures, a* and go to the one with the least steps
            // Repeat till all treasures are found
            if (exploredMap.TreasureNodes.Count > 0)
            {
                TurnAction action = pathingChoice.MoveToClosestTreasure(exploredMap, request.PartyLocation);
                if (action != TurnAction.Pass)
                {
                    return Task.FromResult(new Turn(action));
                }
            }

            if (exploredMap.Enemies.Count > 0 && (characterManagement.CharacterList.Count == 0 || characterManagement.TotalCurrentHealth > 50) )
            {
                TurnAction action = pathingChoice.GoToEnemies(exploredMap, request.PartyLocation);
                if (action != TurnAction.Pass)
                {
                    return Task.FromResult(new Turn(action));
                }
            }

            // Go to the exit
            return Task.FromResult(new Turn(pathingChoice.GoToFinish(exploredMap, request.PartyLocation)));
        }
    }
}