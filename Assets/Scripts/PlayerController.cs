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

        void FixedUpdate() {
            if (!GameManager.Instance.IsPlaying()) return;

            if (Input.GetKey(KeyCode.W)) gridBased.ChangeMovePoint(Movement.Direction.North); // Up Arrow or 'W'
            if (Input.GetKey(KeyCode.D)) gridBased.ChangeMovePoint(Movement.Direction.East);  // Right Arrow or 'D'
            if (Input.GetKey(KeyCode.S)) gridBased.ChangeMovePoint(Movement.Direction.South); // Down Arrow or 'S'
            if (Input.GetKey(KeyCode.A)) gridBased.ChangeMovePoint(Movement.Direction.West);  // Left Arrow or 'A'
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
