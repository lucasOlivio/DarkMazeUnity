using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace DM
{
    public class EntitySystem : SimulationSystem
    {
        private GameObject playerPrefab;
        private Dictionary<EnemyType, GameObject> enemyPrefabs;
        private List<Enemy> enemyList;

        public override void Init()
        {
            SetupEvents();
            SetupPrefabResources();

            enemyList = new List<Enemy>();
        }

        private void SetupPrefabResources()
        {
            enemyPrefabs = new Dictionary<EnemyType, GameObject>
            {
                [EnemyType.Normal] = Resources.Load<GameObject>("Prefabs/Pref_Mummy"),
                [EnemyType.Boss] = Resources.Load<GameObject>("Prefabs/Pref_Pharao"),
            };

            playerPrefab = Resources.Load<GameObject>("Prefabs/Pref_Player");
        }

        /// <summary>
        /// The GenerateEntities function is responsible for loading the entities from a text file 
        /// and generating the corresponding grid and tilemap. 
        /// 
        /// It processes the map layout using specific characters:
        /// - 'B' represents the boss enemy.
        /// - 'E' represents common enemies.
        /// - 'G' represents the goal.
        /// - 'W' represents the maze win/exit.
        /// - 'P' represents the player.
        /// 
        /// This function handles the placement of enemies, player, and goal in the map.
        /// 
        /// IMPORTANT: THE MAP SHOULD ALWAYS BE SQUARE!
        /// </summary>
        public void GenerateEntities(string mapName)
        {
            string[] mapData = LOIO.LoadStreamingAsset(SimulationInfo.MAZE_FOLDER + mapName);

            for (int y = 0; y < MapInfo.worldMapSize; y++)
            {
                int invertedY = MapInfo.worldMapSize - 1 - y; // Invert lines

                for (int x = 0; x < MapInfo.worldMapSize; x++)
                {
                    Vector3 tilePosition = new Vector3(x, y, 0.0f);
                    char tileChar = mapData[invertedY][x];

                    if (tileChar == 'X' || tileChar == 'O') // Map chars
                    {
                        continue;
                    }

                    Quaternion rotation = LOMovement.GetOpenRotation((int)tilePosition.x, (int)tilePosition.y);
                    switch (tileChar)
                    {
                        case 'P': // Player
                            Object.Instantiate(playerPrefab, tilePosition, rotation);
                            break;
                        case 'E': // Enemy
                            Object.Instantiate(enemyPrefabs[EnemyType.Normal], tilePosition, rotation);
                            break;
                        case 'B': // Boss
                            Object.Instantiate(enemyPrefabs[EnemyType.Boss], tilePosition, rotation);
                            break;
                        // case 'G': // Goal
                        //     GameObject goalObject = InstantiatePrefab(goalPrefab, tilePosition, rotation);

                        //     // TODO: Find a better place for registering and referencing the objects after loading
                        //     GameManager.Instance.RegisterGoal(goalObject.GetComponent<TriggerSwitch>());
                        //     break;
                        // case 'W': // Exit with Door + Win
                        //     GameObject doorObject = InstantiatePrefab(doorPrefab, tilePosition, rotation);
                        //     GameObject exitObject = InstantiatePrefab(winPrefab, tilePosition, rotation);

                        //     // TODO: Find a better place for registering and referencing the objects after loading
                        //     GameManager.Instance.RegisterDoor(doorObject);
                        //     GameManager.Instance.RegisterExit(exitObject);
                        //     break;
                    }
                }
            }
        }

        private void SetupEvents()
        {
            SimulationManager.OnTick += Tick;
        }

        private void SpawnEnemy(EnemyType type, int2 position)
        {
            var newEnemy = new Enemy
            {
                type = type,
                position = position,
                direction = LOMovement.GetDirection(position.x, position.y, MapInfo.center.x, MapInfo.center.y)
            };
            enemyList.Add(newEnemy);
        }

        protected override void Tick(object sender, OnTickArgs eventArgs)
        {
            foreach (Enemy enemy in enemyList)
            {
                enemy.Tick();
            }
        }

        public override void Quit()
        {
            SimulationManager.OnTick -= Tick;
        }
    }
}
