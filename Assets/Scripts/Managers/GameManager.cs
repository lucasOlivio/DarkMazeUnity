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
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            gameState = new GameState(initialState);
        }

        public void SetMainMenu() => gameState.ChangeState(GameState.State.MainMenu);
        public void SetPlaying() => gameState.ChangeState(GameState.State.Playing);
        public void SetPaused() => gameState.ChangeState(GameState.State.Paused);
        public void SetGameWin() => gameState.ChangeState(GameState.State.GameWin);
        public void SetGameOver() => gameState.ChangeState(GameState.State.GameOver);

        public bool IsPlaying() => gameState.IsPlaying();
        public bool IsMainMenu() => gameState.IsMainMenu();
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

        public void RegisterPlayer(GridBasedMovement player)
        {
            this.player = player;
            foreach (var enemy in enemies)
            {
                enemy.SetTarget(player);
            }
        }
    }
}
