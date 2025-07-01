using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public class SimulationManager : MonoBehaviour
    {
        public static SimulationManager Instance { get; private set; }

        public static event EventHandler<OnTickArgs> OnTick;
        public MapSystem mapSystem { get; private set; }
        public EntitySystem entitySystem { get; private set; }

        private int tick;
        private float tickTimer;

        void Awake()
        {
            EnforceSingleInstance();

            mapSystem = new MapSystem();
            entitySystem = new EntitySystem();

            tick = 0;
            tickTimer = 0;
        }

        void Start()
        {
            mapSystem.Init();
            mapSystem.GenerateWorldMap("DEMO.txt");

            entitySystem.Init();
            entitySystem.GenerateEntities("DEMO.txt");
        }

        private void EnforceSingleInstance()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        void Update()
        {
            tickTimer += Time.deltaTime;

            if (tickTimer >= SimulationInfo.TICK)
            {
                tick++;
                tickTimer -= SimulationInfo.TICK;

                OnTick?.Invoke(this, new OnTickArgs { tick_ = tick });
            }
        }

        void OnDisable()
        {
            mapSystem.Quit();
            entitySystem.Quit();
        }
    }
}