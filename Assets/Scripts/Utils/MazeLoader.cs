using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DM
{
    /// <summary>
    /// The MazeLoader class is responsible for loading a maze from a text file 
    /// and generating the corresponding grid and tilemap. 
    /// 
    /// It processes the maze layout using specific characters:
    /// - 'X' represents walls.
    /// - 'O' represents normal ground tiles.
    /// - 'B' represents the boss enemy.
    /// - 'E' represents common enemies.
    /// - 'G' represents the goal.
    /// - 'W' represents the maze win/exit.
    /// - 'P' represents the player.
    /// 
    /// Every tile that is not a wall ('X') will have a ground tile placed. 
    /// This class handles the placement of enemies, player, and goal in the maze.
    /// </summary>
    public class MazeLoader
    {
        private string[] mazeData;

        private GameObject playerPrefab;
        private GameObject enemyPrefab;
        private GameObject bossPrefab;
        private GameObject doorPrefab;
        private GameObject winPrefab;
        private GameObject goalPrefab;

        private Tilemap groundTilemap;
        private Tilemap wallTilemap;

        private TileBase[] groundTiles;
        private TileBase[] wallTiles;

        public MazeLoader(GameObject playerPrefab, GameObject enemyPrefab, GameObject bossPrefab,
                        GameObject doorPrefab, GameObject winPrefab, GameObject goalPrefab,
                        Tilemap groundTilemap, Tilemap wallTilemap,
                        TileBase[] groundTiles, TileBase[] wallTiles)
        {
            this.playerPrefab = playerPrefab;
            this.enemyPrefab = enemyPrefab;
            this.bossPrefab = bossPrefab;
            this.doorPrefab = doorPrefab;
            this.winPrefab = winPrefab;
            this.goalPrefab = goalPrefab;
            this.groundTilemap = groundTilemap;
            this.wallTilemap = wallTilemap;
            this.groundTiles = groundTiles;
            this.wallTiles = wallTiles;
        }

        private bool IsOpenTile(int x, int y, string[] mazeData)
        {
            if (y < 0 || y >= mazeData.Length || x < 0 || x >= mazeData[y].Length)
                return false;

            char tile = mazeData[y][x];
            return tile != 'X'; // X is a wall, so anything else is open
        }

        private Quaternion GetFacingRotation(int x, int y, string[] mazeData)
        {
            foreach (var direction in Movement.Directions)
            {
                if (direction == Movement.Direction.Center) continue; // Skip Center

                Vector3 offset = Movement.GetMovement(direction);
                int checkX = x + (int)offset.x;
                int checkY = y - (int)offset.y; // Inverted Y in maze construction

                if (IsOpenTile(checkX, checkY, mazeData))
                {
                    float angle = Movement.DirectionToRotation(direction);
                    return Quaternion.Euler(0, 0, angle);
                }
            }

            return Quaternion.identity; // No open tiles? Keep default rotation.
        }

        public void GenerateMaze(string[] mazeData)
        {
            this.mazeData = mazeData;
            int height = mazeData.Length;
            int width = mazeData[0].Length;

            groundTilemap.ClearAllTiles();
            wallTilemap.ClearAllTiles();

            for (int y = 0; y < height; y++)
            {
                int invertedY = height - 1 - y;
                string line = mazeData[invertedY]; // Invert lines
                for (int x = 0; x < width; x++)
                {
                    char tileChar = line[x];
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);

                    if (tileChar == 'X')
                    {
                        wallTilemap.SetTile(tilePosition, GetRandomTile(wallTiles));
                        continue;
                    }

                    groundTilemap.SetTile(tilePosition, GetRandomTile(groundTiles));

                    Quaternion rotation = GetFacingRotation(x, invertedY, mazeData);
                    switch (tileChar)
                    {
                        case 'P': // Player
                            InstantiatePrefab(playerPrefab, tilePosition, rotation, GameObjectsInfo.playerName);
                            break;
                        case 'E': // Enemy
                            InstantiatePrefab(enemyPrefab, tilePosition, rotation);
                            break;
                        case 'B': // Boss
                            InstantiatePrefab(bossPrefab, tilePosition, rotation);
                            break;
                        case 'G': // Goal
                            GameObject goalObject = InstantiatePrefab(goalPrefab, tilePosition, rotation);

                            // TODO: Find a better place for registering and referencing the objects after loading
                            GameManager.Instance.RegisterGoal(goalObject.GetComponent<TriggerSwitch>());
                            break;
                        case 'W': // Exit with Door + Win
                            GameObject doorObject = InstantiatePrefab(doorPrefab, tilePosition, rotation);
                            GameObject exitObject = InstantiatePrefab(winPrefab, tilePosition, rotation);

                            // TODO: Find a better place for registering and referencing the objects after loading
                            GameManager.Instance.RegisterDoor(doorObject);
                            GameManager.Instance.RegisterExit(exitObject);
                            break;
                    }
                }
            }
        }

        private TileBase GetRandomTile(TileBase[] multiTiles)
        {
            return multiTiles[Random.Range(0, multiTiles.Length)];
        }

        private GameObject InstantiatePrefab(GameObject prefab, Vector3Int position, Quaternion rotation, string name = "")
        {
            if (prefab == null) return null;
            Vector3 worldPos = groundTilemap.GetCellCenterWorld(position);
            GameObject newObject = GameObject.Instantiate(prefab, worldPos, rotation);

            if (name != "")
            {
                newObject.name = name;
            }
            return newObject;
        }
    }
}
