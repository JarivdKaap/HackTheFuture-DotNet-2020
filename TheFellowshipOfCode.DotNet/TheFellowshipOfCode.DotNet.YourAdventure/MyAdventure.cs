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
using TheFellowshipOfCode.DotNet.YourAdventure.PathChoices;

namespace TheFellowshipOfCode.DotNet.YourAdventure
{
    public class MyAdventure : IAdventure
    {
        private readonly Random _random = new Random();
        private CharacterManager _characterManager = CharacterManager.GetInstance();

        public Task<Party> CreateParty(CreatePartyRequest request)
        {
            var party = new Party
            {
                Name = "My Party",
                Members = new List<PartyMember>()
            };

            for (var i = 0; i < request.MembersCount; i++)
            {
                Fighter fighter = new Fighter()
                {
                    Id = i,
                    Name = $"Member {i + 1}",
                    Constitution = 8,
                    Strength = 8,
                    Intelligence = 18 // 34 points
                };
                _characterManager.AddCharacter(fighter);
                party.Members.Add(fighter);
            }

            return Task.FromResult(party);
        }

        public Task<Turn> PlayTurn(PlayTurnRequest request)
        {
            if (_characterManager.TotalCurrentHealth <= 0)
            {
                return Task.FromResult(new Turn(TurnAction.Pass));
            }

            ExploredMap exploredMap = ExploredMap.GetInstance(request.Map.Tiles);
            exploredMap.UpdateEnemyAndLoot(request.Map.Tiles, _characterManager);
            _characterManager.AddCharacter(request.PartyMember);

            // Always align whenever possible
            if (request.PossibleActions.Contains(TurnAction.Loot))
            {
                return Task.FromResult(new Turn(TurnAction.Loot));
            }

            // Grab the potion if we're in combat and the health is less than half the original points
            if (request.IsCombat && request.PartyMember.CurrentHealthPoints < request.PartyMember.HealthPoints / 2 &&
                request.PossibleActions.Any(pa => pa == TurnAction.DrinkPotion))
            {
                return Task.FromResult(new Turn(TurnAction.DrinkPotion));
            }

            // Always attack when possible, our pathfinding handles whether the enemy is too strong
            if (request.PossibleActions.Contains(TurnAction.Attack))
            {
                return Task.FromResult(new Turn(TurnAction.Attack));
            }

            // Get all treasures, a* and go to the one with the least steps
            // Repeat till all treasures are found
            if (exploredMap.TreasureNodes.Count > 0)
            {
                TurnAction action = new TreasuresPathChoice(exploredMap, request.PartyLocation).FindPath();
                if (action != TurnAction.Pass)
                {
                    return Task.FromResult(new Turn(action));
                }
            }

            // If there are enemies we can handle, go to them
            if (exploredMap.PossibleEnemies.Count > 0)
            {
                TurnAction action = new EnemiesPathChoice(exploredMap, request.PartyLocation).FindPath();
                if (action != TurnAction.Pass)
                {
                    return Task.FromResult(new Turn(action));
                }
            }

            // Go to the finish
            return Task.FromResult(new Turn(new FinishPathChoice(exploredMap, request.PartyLocation).FindPath()));
        }
    }
}