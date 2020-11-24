using System;
using System.Collections.Generic;
using System.Linq;
using HTF2020.Contracts.Models;
using HTF2020.Contracts.Models.Adventurers;

namespace TheFellowshipOfCode.DotNet.YourAdventure
{
    public class CharacterManagement
    {
        public List<Character> CharacterList{ get; set;}

        private static CharacterManagement _characterManagement = null;

        public int TotalCurrentHealth => CharacterList.Sum(c => c.CurrentHealthPoints);

        public static CharacterManagement GetInstance()
        {
            if(_characterManagement == null)
            {
                return new CharacterManagement();
            }
            return _characterManagement;
        }

        private CharacterManagement()
        {
            CharacterList = new List<Character>();
        }

        public void CheckCharacter(Character character)
        {
            if (character != null && !CharacterList.Contains(character))
            {
                CharacterList.Add(character);
            }
        }
    }
}