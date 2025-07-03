using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GridBasedMovement gridBased;
    public string enemyTag = "Enemy";

    void Start()
    {
        //GameManager.Instance.RegisterPlayer(gridBased);
    }

    void Update()
    {
        // if(!GameManager.Instance.IsPlaying()) return;

        // if (Input.GetKey(KeyCode.W)) gridBased.ChangeMovePoint(Movement.Direction.North); // Up Arrow or 'W'
        // if (Input.GetKey(KeyCode.D)) gridBased.ChangeMovePoint(Movement.Direction.East);  // Right Arrow or 'D'
        // if (Input.GetKey(KeyCode.S)) gridBased.ChangeMovePoint(Movement.Direction.South); // Down Arrow or 'S'
        // if (Input.GetKey(KeyCode.A)) gridBased.ChangeMovePoint(Movement.Direction.West);  // Left Arrow or 'A'
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(enemyTag))
        {
            GameManager.Instance.SetGameOver();
        }
    }
}
