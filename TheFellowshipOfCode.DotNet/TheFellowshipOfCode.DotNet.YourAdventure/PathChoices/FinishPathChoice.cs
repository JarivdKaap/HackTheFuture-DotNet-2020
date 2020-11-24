using System;
using System.Collections.Generic;
using System.Text;
using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;

namespace TheFellowshipOfCode.DotNet.YourAdventure.PathChoices
{
    class FinishPathChoice : PathChoice
    {
        public FinishPathChoice(ExploredMap exploredMap, Location location) : base(exploredMap, location)
        {
        }

        public override TurnAction FindPath()
        {
            return this.FindPath(exploredMap, new Node(location.X, location.Y),
                exploredMap.FinishNode);
        }
    }
}
