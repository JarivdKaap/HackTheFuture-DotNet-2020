using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;
using HTF2020.Contracts.Models.Adventurers;
using HTF2020.Contracts.Models.Enemies;

namespace TheFellowshipOfCode.DotNet.YourAdventure
{
    public class ExploredMap
    {
        public Node FinishNode { get; set; }
        public Tile[,] Tiles { get; set; }
        public List<Node> TreasureNodes { get; set; }
        public List<Node> Enemies { get; set; }
        public List<List<Node>> ConvertedMap { get; set; }

        private static ExploredMap _exploredMap = null;

        private ExploredMap(Tile[,] tiles)
        {
            TreasureNodes = new List<Node>();
            Enemies = new List<Node>();
            this.Tiles = tiles;
            ConvertTilesToNodes();
        }

        public static ExploredMap GetInstance(Tile[,] tiles)
        {
            if (_exploredMap == null)
            {
                _exploredMap = new ExploredMap(tiles);
            }

            return _exploredMap;
        }

        public void ConvertTilesToNodes()
        {
            ConvertedMap = new List<List<Node>>();
            for (int i = 0; i < Tiles.GetLength(1); i++)
            {
                ConvertedMap.Add(new List<Node>());
                for (int j = 0; j < Tiles.GetLength(0); j++)
                {
                    Tile tile = Tiles[j, i];
                    bool walkable = tile.TerrainType == TerrainType.Grass && tile.TileType != TileType.Wall;
                    int weight = 1;
                    if (tile.EnemyGroup != null)
                    {
                        weight = tile.EnemyGroup.Enemies.Sum(e => e.Strength + e.Intelligence + e.Constitution);
                    }
                    Node node = new Node(new Point(j, i), walkable, weight)
                    {
                        Tile = tile
                    };

                    switch (tile.TileType)
                    {
                        case TileType.Enemy: Enemies.Add(node); break;
                        case TileType.Finish: FinishNode = node; break;
                        case TileType.TreasureChest: TreasureNodes.Add(node); break;
                        default: break;
                    }

                    ConvertedMap[i].Add(node);
                }
            }
        }

        public void UpdateEnemyAndLoot(Tile[,] tiles)
        {
            List<Node> newTreasureList = new List<Node>();
            foreach (var treasureNode in TreasureNodes)
            {
                Tile tile = tiles[treasureNode.Position.X, treasureNode.Position.Y];
                if (!tile.TreasureChest.IsEmpty)
                {
                    newTreasureList.Add(treasureNode);
                }
            }

            TreasureNodes = newTreasureList;

            List<Node> newEnemyList = new List<Node>();
            foreach (var enemyNode in Enemies)
            {
                Tile tile = tiles[enemyNode.Position.X, enemyNode.Position.Y];
                if (!tile.EnemyGroup.IsDead)
                {
                    newEnemyList.Add(enemyNode);
                }
            }
            Enemies = newEnemyList;
        }
    }

}
