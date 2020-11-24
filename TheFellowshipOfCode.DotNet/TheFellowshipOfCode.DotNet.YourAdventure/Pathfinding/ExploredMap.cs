using System;
using System.Collections.Generic;
using System.Drawing;
using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;
using HTF2020.Contracts.Models.Adventurers;
using HTF2020.Contracts.Models.Enemies;

namespace TheFellowshipOfCode.DotNet.YourAdventure.Pathfinding
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
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                ConvertedMap.Add(new List<Node>());
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    Tile tile = Tiles[j, i];
                    bool walkable = tile.TerrainType == TerrainType.Grass && tile.TileType != TileType.Wall;
                    Node node = new Node(new Point(j, i), walkable);

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
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    
                }
            }
        }
    }

}
