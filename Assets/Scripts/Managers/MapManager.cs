using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DM
{
    public class MapManager : MonoBehaviour
    {
        public string mazeName;

        public static MapManager Instance { get; private set; }

        public MazeSystem mazeSystem;
        public NodeSystem nodeSystem;

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
            mazeSystem = new MazeSystem();
            mazeSystem.Init();

            // If maze name not set then the maze must be created manually on the editor
            if (mazeName != null && mazeName != "")
            {
                mazeSystem.LoadMazeFromFile(mazeName);
            }

            nodeSystem = new NodeSystem();
            nodeSystem.Init();

            nodeSystem.UpdateNodes();
        }

        void Update()
        {
        }
    }
}
