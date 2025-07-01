using System;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace DM
{
    public class MapSystem : SimulationSystem
    {
        public static event EventHandler<OnMapEventArgs> OnUpdateMapRender;
        private WorldMap worldMap;

        public override void Init()
        {
            SetupEvents();
        }

        private void SetupEvents()
        {
            SimulationManager.OnTick += Tick;
        }

        /// <summary>
        /// The GenerateWorldMap function is responsible for loading a map from a text file 
        /// and generating the corresponding grid and tilemap. 
        /// 
        /// It processes the map layout using specific characters:
        /// - 'X' represents walls.
        /// - 'O' represents normal ground tiles.
        /// 
        /// Every tile that is not a wall ('X') will have a ground tile placed. 
        /// 
        /// IMPORTANT: THE MAP SHOULD ALWAYS BE SQUARE!
        /// </summary>
        public void GenerateWorldMap(string mapName)
        {
            string[] mapData = LOIO.LoadStreamingAsset(SimulationInfo.MAZE_FOLDER + mapName);
            MapInfo.worldMapSize = mapData.Length;

            worldMap = new WorldMap(MapInfo.worldMapSize);

            for (int y = 0; y < MapInfo.worldMapSize; y++)
            {
                int invertedY = MapInfo.worldMapSize - 1 - y;// Invert lines
                
                for (int x = 0; x < MapInfo.worldMapSize; x++)
                {
                    int2 tilePosition = new int2(x, y);
                    Cell cell = new Cell
                    {
                        id = PositionToId(tilePosition),
                        isWalkable = false,
                        position = tilePosition,
                        groundType = GroundType.None,
                        structureType = StructureType.None,
                    };

                    if (mapData[invertedY][x] == 'X')
                    {
                        cell.structureType = StructureType.Wall;
                    }
                    else
                    {
                        cell.groundType = GroundType.Earth;
                    }

                    worldMap.cells.Add(cell);
                }
            }

            OnUpdateMapRender?.Invoke(this, new OnMapEventArgs { worldMap_ = worldMap });
        }

        protected override void Tick(object sender, OnTickArgs eventArgs)
        {

        }

        public override void Quit()
        {
            SimulationManager.OnTick -= Tick;
        }

        private Cell GetCell(int id)
        {
            if (OnMap(id))
            {
                return worldMap.cells[id];
            }

            return null;
        }

        private Cell GetCell(int2 position)
        {
            return GetCell(position.x, position.y);
        }

        private Cell GetCell(int x, int y)
        {
            int cellId = PositionToId(x, y);

            return GetCell(cellId);
        }

        public bool IsWalkable(int x, int y)
        {
            if (OnMap(x, y))
            {
                Cell cell = GetCell(x, y);

                return cell.isWalkable;
            }
            else
            {
                return false;
            }
        }

        public bool IsWalkable(int2 position)
        {
            return IsWalkable(position.x, position.y);
        }

        private void SetCell(int x, int y, GroundType groundType)
        {
            if (!OnMap(x, y))
            {
                return;
            }

            Cell cell = GetCell(x, y);
            cell.groundType = groundType;
        }

        private void SetCell(int x, int y, StructureType structureType)
        {
            if (!OnMap(x, y))
            {
                return;
            }

            Cell cell = GetCell(x, y);
            cell.isWalkable = false;
            cell.structureType = structureType;
        }

        private bool OnMap(int x, int y)
        {
            bool insideHorizontalBounds = x >= 0 && x < worldMap.size;
            bool insideVerticalBounds = y >= 0 && y < worldMap.size;

            return insideHorizontalBounds && insideVerticalBounds;
        }

        private bool OnMap(int2 position)
        {
            return OnMap(position.x, position.y);
        }

        private bool OnMap(int id)
        {
            return id >= 0 && id < worldMap.area;
        }

        private int2 IdToPosition(int id)
        {
            int x = id % worldMap.width;
            int y = id / worldMap.width;

            return new int2(x, y);
        }

        private int PositionToId(int x, int y)
        {
            return y * worldMap.width + x;
        }

        private int PositionToId(int2 position)
        {
            return PositionToId(position.x, position.y);
        }

        public int2 GetOpenPositionAround(int2 center, int minDistance, int maxDistance)
        {
            return GetOpenPositionAround(center.x, center.y, minDistance, maxDistance);
        }

        public int2 GetOpenPositionAround(int centerX, int centerY, int minDistance, int maxDistance)
        {
            int2 position;

            do
            {
                position = LOMovement.RandomPositionAround(centerX, centerY, minDistance, maxDistance);
            } while (!OnMap(position));

            return position;
        }
    }
}
