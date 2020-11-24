using System;
using System.Collections.Generic;
using System.Linq;
using HTF2020.Contracts.Models;
using HTF2020.Contracts.Models.Adventurers;

namespace TheFellowshipOfCode.DotNet.YourAdventure
{
    public class CharacterManager
    {
        public List<Character> CharacterList{ get; set;}

        private static CharacterManager _characterManager = null;

        public int TotalCurrentHealth => CharacterList.Sum(c => c.CurrentHealthPoints);

        public static CharacterManager GetInstance()
        {
            if(_characterManager == null)
            {
                return new CharacterManager();
            }
            return _characterManager;
        }

        private CharacterManager()
        {
            CharacterList = new List<Character>();
        }

        public void AddCharacter(Character character)
        {
            if (character != null && !CharacterList.Contains(character))
            {
                CharacterList.Add(character);
            }
        }
    }
}