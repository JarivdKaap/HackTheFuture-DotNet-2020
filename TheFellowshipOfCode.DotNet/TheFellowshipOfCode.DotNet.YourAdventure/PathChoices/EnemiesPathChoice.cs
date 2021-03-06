﻿using System;
using System.Collections.Generic;
using System.Text;
using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;

namespace TheFellowshipOfCode.DotNet.YourAdventure.PathChoices
{
    class EnemiesPathChoice : PathChoice
    {
        public EnemiesPathChoice(ExploredMap exploredMap, Location location) : base(exploredMap, location)
        {
        }

        public override TurnAction FindPath()
        {
            Node startNode = new Node(location.X, location.Y);
            Stack<Node> path = FindPathToClosestNode(exploredMap, startNode, exploredMap.PossibleEnemies);

            if (path == null)
                return TurnAction.Pass;

            Node pathNode = path.Peek();

            return GetTurnActionForNodes(startNode.Position, pathNode.Position);
        }
    }
}
