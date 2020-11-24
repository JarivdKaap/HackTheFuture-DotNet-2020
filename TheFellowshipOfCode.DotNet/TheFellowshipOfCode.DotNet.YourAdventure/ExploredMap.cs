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
        public List<Node> PossibleEnemies { get; set; }
        public List<List<Node>> ConvertedMap { get; set; }

        private static ExploredMap _exploredMap = null;

        private ExploredMap(Tile[,] tiles)
        {
            TreasureNodes = new List<Node>();
            PossibleEnemies = new List<Node>();
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
                        case TileType.Enemy: PossibleEnemies.Add(node); break;
                        case TileType.Finish: FinishNode = node; break;
                        case TileType.TreasureChest: TreasureNodes.Add(node); break;
                        default: break;
                    }

                    ConvertedMap[i].Add(node);
                }
            }
        }

        public void UpdateEnemyAndLoot(Tile[,] tiles, CharacterManagement characterManagement)
        {
            List<Node> newTreasureList = new List<Node>();
            foreach (var treasureNode in TreasureNodes)
            {
                if (!treasureNode.Tile.TreasureChest.IsEmpty)
                {
                    newTreasureList.Add(treasureNode);
                }
            }

            TreasureNodes = newTreasureList;

            List<Node> newEnemyList = new List<Node>();
            for (int i = 0; i < tiles.GetLength(1); i++)
            {
                for (int j = 0; j < tiles.GetLength(0); j++)
                {
                    Tile tile = Tiles[j, i];
                    if (tile.TileType == TileType.Enemy)
                    {
                        if (!tile.EnemyGroup.IsDead || tile.EnemyGroup.Loot > 0)
                        {
                            Node enemyNode = ConvertedMap[i][j];
                            enemyNode.Walkable = (characterManagement.CharacterList.Count == 0 || characterManagement.TotalCurrentHealth >
                                enemyNode.Tile.EnemyGroup.Enemies.Sum(e => e.CurrentHealthPoints) / 2);

                            if(enemyNode.Walkable)
                                newEnemyList.Add(enemyNode);
                        }
                    }
                }
            }
            PossibleEnemies = newEnemyList;
        }
    }

}
