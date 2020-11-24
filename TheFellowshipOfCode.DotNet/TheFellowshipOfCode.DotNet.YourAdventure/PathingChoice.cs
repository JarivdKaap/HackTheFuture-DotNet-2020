using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;

namespace TheFellowshipOfCode.DotNet.YourAdventure
{
    class PathingChoice
    {
        private TurnAction FindPath(ExploredMap exploredMap, Node startNode, Node finishMode)
        {
            Astar astar = new Astar(exploredMap.ConvertedMap);

            Stack<Node> nodes = astar.FindPath(startNode, finishMode);

            Node firstNode = nodes.Peek();

            if (firstNode.Position.Y < startNode.Position.Y)
                return TurnAction.WalkNorth;
            if (firstNode.Position.Y > startNode.Position.Y)
                return TurnAction.WalkSouth;
            if (firstNode.Position.X > startNode.Position.X)
                return TurnAction.WalkEast;
            if (firstNode.Position.X < startNode.Position.X)
                return TurnAction.WalkWest;

            return TurnAction.Pass;
        }

        public TurnAction MoveToClosestTreasure(ExploredMap exploredMap, Location partyLocation, TurnAction[] possibleActions)
        {
            Stack<Node> closestPath = null;
            Astar astar = new Astar(exploredMap.ConvertedMap);
            Node startNode = new Node(new Point(partyLocation.X, partyLocation.Y), true);
            foreach (var treasureNode in exploredMap.TreasureNodes)
            {
                Stack<Node> nodes = astar.FindPath(startNode, treasureNode);
                if (closestPath == null || closestPath.Count > nodes.Count)
                {
                    closestPath = nodes;
                }
            }

            Node firstNode = closestPath.Peek();

            if (firstNode.Position.Y < startNode.Position.Y)
                return TurnAction.WalkNorth;
            if (firstNode.Position.Y > startNode.Position.Y)
                return TurnAction.WalkSouth;
            if (firstNode.Position.X > startNode.Position.X)
                return TurnAction.WalkEast;
            if (firstNode.Position.X < startNode.Position.X)
                return TurnAction.WalkWest;

            return TurnAction.Pass;
        }

        public TurnAction GoToFinish(ExploredMap exploredMap, Location partyLocation, TurnAction[] possibleActions)
        {
            return FindPath(exploredMap, new Node(new Point(partyLocation.X, partyLocation.Y), true),
                exploredMap.FinishNode);
        }
    }

}
