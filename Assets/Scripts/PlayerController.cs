using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DM
{
    public class PlayerController : MonoBehaviour
    {
        public GridBasedMovement gridBased;

        void Start()
        {
            GameManager.Instance.RegisterPlayer(gridBased);
        }

        void Pause()
        {
            if (GameManager.Instance.IsPaused())
            {
                GameManager.Instance.SetPlaying();
                return;
            }

            GameManager.Instance.SetPaused();
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.P)) Pause();
        }

        void FixedUpdate()
        {
            if (!GameManager.Instance.IsPlaying()) return;

            if (Input.GetKey(KeyCode.W)) gridBased.ChangeMovePoint(Movement.Direction.North); // Up
            if (Input.GetKey(KeyCode.D)) gridBased.ChangeMovePoint(Movement.Direction.East);  // Right
            if (Input.GetKey(KeyCode.S)) gridBased.ChangeMovePoint(Movement.Direction.South); // Down
            if (Input.GetKey(KeyCode.A)) gridBased.ChangeMovePoint(Movement.Direction.West);  // Left
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GameObjectsInfo.enemyTag))
            {
                GameManager.Instance.SetGameOver();
            }
        }
    }
}
