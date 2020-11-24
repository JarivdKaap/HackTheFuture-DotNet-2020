using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;
using TheFellowshipOfCode.DotNet.YourAdventure.Pathfinding;

namespace TheFellowshipOfCode.DotNet.YourAdventure
{
    class PathingChoice
    {
        private TurnAction FindPath(ExploredMap exploredMap, Node startNode, Node finishMode)
        {
            Astar astar = new Astar(exploredMap.ConvertedMap);

            Stack<Node> nodes = astar.FindPath(startNode, finishMode);

            Node correctNode = nodes.Peek();

            if (correctNode.Position.Y < startNode.Position.Y)
                return TurnAction.WalkNorth;
            if (correctNode.Position.Y > startNode.Position.Y)
                return TurnAction.WalkSouth;
            if (correctNode.Position.X < startNode.Position.X)
                return TurnAction.WalkEast;
            if (correctNode.Position.X > startNode.Position.X)
                return TurnAction.WalkEast;

            return TurnAction.Pass;
        }

        public TurnAction GoToFinish(ExploredMap exploredMap, Location partyLocation, TurnAction[] possibleActions)
        {
            return FindPath(exploredMap, new Node(new Point(partyLocation.X, partyLocation.Y), true),
                exploredMap.FinishNode);
        }
    }

}
