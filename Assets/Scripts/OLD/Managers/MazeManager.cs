using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeManager : MonoBehaviour
{
    public static MazeManager Instance { get; private set; }

    public string mazesPath = "Mazes/";
    public string mazeName = "DEMO.txt";

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public GameObject doorPrefab;
    public GameObject winPrefab;
    public GameObject goalPrefab;

    [Header("Tilemaps")]
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;

    [Header("Tile Variations")]
    public TileBase[] groundTiles;
    public TileBase[] wallTiles;

    private MazeLoader mazeLoader;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        string[] mazeData = LoadMazeFromFile(mazesPath + mazeName);
        if (mazeData.Length == 0)
        {
            Debug.LogError("Failed to load maze data. Check file path.");
            return;
        }

        mazeLoader = new MazeLoader(playerPrefab, enemyPrefab, bossPrefab, doorPrefab, winPrefab,
                                    goalPrefab, groundTilemap, wallTilemap, groundTiles, wallTiles);
        mazeLoader.GenerateMaze(mazeData);
        Debug.Log("Finished generating maze");
    }

    private string[] LoadMazeFromFile(string filePath)
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
