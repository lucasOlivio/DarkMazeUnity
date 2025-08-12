using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DM
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GameState gameState;
        public GameState.State initialState;
        public GameObject pauseMenu;

        private GridBasedMovement player;
        private List<EnemyAI> enemies = new List<EnemyAI>();
        private GameObject door;
        private GameObject exit;
        private TriggerSwitch goal;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            gameState = new GameState(initialState);
        }

        public void SetPlaying()
        {
            gameState.ChangeState(GameState.State.Playing);
            pauseMenu?.SetActive(false);
        }

        public void SetPaused()
        {
            gameState.ChangeState(GameState.State.Paused);
            pauseMenu?.SetActive(true);
        }

        public void SetGameWin() => gameState.ChangeState(GameState.State.GameWin);
        public void SetGameOver() => gameState.ChangeState(GameState.State.GameOver);

        public bool IsPlaying() => gameState.IsPlaying();
        public bool IsPaused() => gameState.IsPaused();
        public bool IsGameWin() => gameState.IsGameWin();
        public bool IsGameOver() => gameState.IsGameOver();

        public void RegisterEnemy(EnemyAI enemy)
        {
            enemies.Add(enemy);
            if (player != null)
            {
                enemy.SetTarget(player);
            }
        }

        public void RegisterPlayer(GridBasedMovement player)
        {
            this.player = player;
            foreach (var enemy in enemies)
            {
                enemy.SetTarget(player);
            }
        }

        public void RegisterExit(GameObject exitObject)
        {
            exit = exitObject;
            exit.SetActive(false);

            if (goal != null)
            {
                goal.objectsToActivate.Add(exit);
            }
        }

        public void RegisterDoor(GameObject doorObject)
        {
            door = doorObject;

            if (goal != null)
            {
                goal.objectsToDeactivate.Add(door);
            }
        }

        public void RegisterGoal(TriggerSwitch goalTrigger)
        {
            goal = goalTrigger;

            if (exit != null)
            {
                goal.objectsToActivate.Add(exit);
            }

            if (door != null)
            {
                goal.objectsToDeactivate.Add(door);
            }
        }
    }
}
