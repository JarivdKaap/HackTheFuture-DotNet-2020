using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;

namespace TheFellowshipOfCode.DotNet.YourAdventure.PathChoices
{
    abstract class PathChoice
    {
        protected ExploredMap exploredMap;
        protected Location location;

        protected PathChoice(ExploredMap exploredMap, Location location)
        {
            this.exploredMap = exploredMap;
            this.location = location;
        }

        protected TurnAction GetTurnActionForNodes(Point startPoint, Point endPoint)
        {
            if (endPoint.Y < startPoint.Y)
                return TurnAction.WalkNorth;
            if (endPoint.Y > startPoint.Y)
                return TurnAction.WalkSouth;
            if (endPoint.X > startPoint.X)
                return TurnAction.WalkEast;
            if (endPoint.X < startPoint.X)
                return TurnAction.WalkWest;

            return TurnAction.Pass;
        }

        protected TurnAction FindPath(ExploredMap exploredMap, Node startNode, Node finishMode)
        {
            Astar aStar = new Astar(exploredMap.ConvertedMap);

            Stack<Node> nodes = aStar.FindPath(startNode, finishMode);

            if (nodes == null)
                return TurnAction.Pass;

            Node pathNode = nodes.Peek();

            return GetTurnActionForNodes(startNode.Position, pathNode.Position);
        }

        protected Stack<Node> FindPathToClosestNode(ExploredMap exploredMap, Node startNode, List<Node> endNodes)
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

        public abstract TurnAction FindPath();
    }
}
