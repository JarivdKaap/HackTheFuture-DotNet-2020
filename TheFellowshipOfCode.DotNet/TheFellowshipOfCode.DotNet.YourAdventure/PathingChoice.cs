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
        private TurnAction GetTurnActionForNodes(Node startNode, Node endNode)
        {
            if (endNode.Position.Y < startNode.Position.Y)
                return TurnAction.WalkNorth;
            if (endNode.Position.Y > startNode.Position.Y)
                return TurnAction.WalkSouth;
            if (endNode.Position.X > startNode.Position.X)
                return TurnAction.WalkEast;
            if (endNode.Position.X < startNode.Position.X)
                return TurnAction.WalkWest;

            return TurnAction.Pass;
        }

        private TurnAction FindPath(ExploredMap exploredMap, Node startNode, Node finishMode)
        {
            Astar aStar = new Astar(exploredMap.ConvertedMap);

            Stack<Node> nodes = aStar.FindPath(startNode, finishMode);

            Node pathNode = nodes.Peek();

            return GetTurnActionForNodes(startNode, pathNode);
        }

        private Stack<Node> FindPathToClosestNode(ExploredMap exploredMap, Node startNode, List<Node> endNodes)
        {
            Stack<Node> closestPath = null;
            Astar aStar = new Astar(exploredMap.ConvertedMap);
            foreach (var node in endNodes)
            {
                Stack<Node> nodes = aStar.FindPath(startNode, node);
                if (closestPath == null || closestPath.Count > nodes.Count)
                {
                    closestPath = nodes;
                }
            }

            return closestPath;
        }

        public TurnAction MoveToClosestTreasure(ExploredMap exploredMap, Location partyLocation)
        {
            Node startNode = new Node(new Point(partyLocation.X, partyLocation.Y), true);
            Stack<Node> path = FindPathToClosestNode(exploredMap, startNode, exploredMap.TreasureNodes);

            Node pathNode = path.Peek();

            return GetTurnActionForNodes(startNode, pathNode);
        }

        public TurnAction GoToEnemies(ExploredMap exploredMap, Location partyLocation)
        {
            Node startNode = new Node(new Point(partyLocation.X, partyLocation.Y), true);
            Stack<Node> path = FindPathToClosestNode(exploredMap, startNode, exploredMap.Enemies);

            Node pathNode = path.Peek();

            return GetTurnActionForNodes(startNode, pathNode);
        }

        public TurnAction GoToFinish(ExploredMap exploredMap, Location partyLocation)
        {
            return FindPath(exploredMap, new Node(new Point(partyLocation.X, partyLocation.Y), true),
                exploredMap.FinishNode);
        }
    }
}