using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DM
{
    public class WorldRender : MonoBehaviour
    {
        private Grid grid;

        private Tilemap groundTilemap;
        private Tilemap structureTilemap;

        private Dictionary<GroundType, Tile> groundTiles;
        private Dictionary<StructureType, Tile> structureTiles;

        void Awake()
        {
            SetupEvents();

            SetupTilemapResources();
        }

        private void SetupEvents()
        {
            MapSystem.OnUpdateMapRender += UpdateMapRender;
        }

        private void SetupTilemapResources()
        {
            grid = GameObject.Find("Grid").GetComponent<Grid>();

            groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
            structureTilemap = GameObject.Find("Structure").GetComponent<Tilemap>();

            groundTiles = new Dictionary<GroundType, Tile>
            {
                [GroundType.None] = null,
                [GroundType.Earth] = Resources.Load<Tile>("Tiles/GroundEarth1"),
            };

            structureTiles = new Dictionary<StructureType, Tile>
            {
                [StructureType.None] = null,
                [StructureType.Wall] = Resources.Load<Tile>("Tiles/StructureWall1"),
            };
        }

        void Start()
        {

        }

        void Update()
        {

        }

        void OnDisable()
        {
            MapSystem.OnUpdateMapRender -= UpdateMapRender;
        }

        private void UpdateMapRender(object sender, OnMapEventArgs eventArgs)
        {
            foreach (Cell cell in eventArgs.worldMap_.cells)
            {
                Vector3Int tilemapPosition = new Vector3Int(cell.position.x, cell.position.y, 0);

                groundTilemap.SetTile(tilemapPosition, groundTiles[cell.groundType]);
                structureTilemap.SetTile(tilemapPosition, structureTiles[cell.structureType]);
            }
        }
    }
}
