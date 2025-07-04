using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace DM
{
    public class MazeSystem
    {
        public string mazesPath = "Mazes/";

        public GameObject playerPrefab;
        public GameObject enemyPrefab;
        public GameObject bossPrefab;
        public GameObject doorPrefab;
        public GameObject winPrefab;
        public GameObject goalPrefab;

        public Tilemap groundTilemap;
        public Tilemap wallTilemap;

        public TileBase[] groundTiles;
        public TileBase[] wallTiles;

        private MazeLoader mazeLoader;

        public MazeSystem()
        {
        }

        public void Init()
        {
            // Load prefabs
            playerPrefab = Resources.Load<GameObject>(PrefabInfo.playerPrefab);
            enemyPrefab = Resources.Load<GameObject>(PrefabInfo.enemyPrefab);
            bossPrefab = Resources.Load<GameObject>(PrefabInfo.bossPrefab);
            doorPrefab = Resources.Load<GameObject>(PrefabInfo.doorPrefab);
            winPrefab = Resources.Load<GameObject>(PrefabInfo.winPrefab);
            goalPrefab = Resources.Load<GameObject>(PrefabInfo.goalPrefab);

            // Find tilemaps in the scene
            groundTilemap = GameObject.Find(TilesInfo.tilemapGround).GetComponent<Tilemap>();
            wallTilemap = GameObject.Find(TilesInfo.tilemapWall).GetComponent<Tilemap>();


            // Load tile arrays from Resources
            groundTiles = Resources.LoadAll<TileBase>("Tiles/Ground");
            wallTiles = Resources.LoadAll<TileBase>("Tiles/Wall");
        }

        public void LoadMazeFromFile(string mazeName)
        {
            string[] mazeData = LoadFromFile(mazesPath + mazeName);
            if (mazeData.Length == 0)
            {
                Debug.LogError("Failed to load maze data. Check file path.");
                return;
            }

            mazeLoader = new MazeLoader(playerPrefab, enemyPrefab, bossPrefab, doorPrefab, winPrefab,
                                        goalPrefab, groundTilemap, wallTilemap, groundTiles, wallTiles);
            mazeLoader.GenerateMaze(mazeData);
        }

        private string[] LoadFromFile(string filePath)
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, filePath);

            if (File.Exists(fullPath))
            {
                return File.ReadAllLines(fullPath);
            }
            else
            {
                Debug.LogError($"Maze file not found at: {fullPath}");
                return new string[] { };
            }
        }
    }
}
