using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DM
{
    public class MapManager : MonoBehaviour
    {
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

            string mazeName = "DEMO.txt";
            mazeSystem.LoadMazeFromFile(mazeName);

            nodeSystem = new NodeSystem();
            nodeSystem.Init();

            nodeSystem.UpdateNodes();
        }

        void Update()
        {
        }
    }
}
