using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBasedMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public Transform movePoint;
    public LayerMask obstacleLayers;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // movePoint.parent = null;
    }

    /// <summary>
    /// Checks if the movement to a specified point is valid based on the current position and walkable nodes.
    /// </summary>
    /// <param name="movement">The movement vector to apply to the current position.</param>
    public bool TestMovePoint(Vector3 movement)
    {
        if (!GameManager.Instance.IsPlaying())
            return false;

        if (Vector3.Distance(transform.position, movePoint.position) > 0.05f)
            return false;

        if (!NodeManager.Instance.IsNodeAccessible(movePoint.position + movement))
            return false;

        if(Physics2D.OverlapCircle(movePoint.position + movement, .2f, obstacleLayers))
            return false;

        return true;
    }

    /// <summary>
    /// Checks if the movement to a specified point is valid based on the current position and obstacle layers.
    /// </summary>
    /// <param name="direction">The direction of the movement to apply to the current position.</param>
    public bool TestMovePoint(Movement.Direction direction)
    {
        Vector3 movement = Movement.GetMovement(direction);

        return TestMovePoint(movement);
    }

    /// <summary>
    /// Updates the move point based on the specified direction, if the movement is valid.
    /// </summary>
    /// <param name="direction">The cardinal direction in which to move (North, East, South, or West).</param>
    public void ChangeMovePoint(Movement.Direction direction)
    {
        Vector3 movement = Movement.GetMovement(direction);

        if (TestMovePoint(movement))
        {
            movePoint.position += movement;
            // Make the object face the movement direction
            if (movement != Vector3.zero)
            {
                transform.right = movement; // Rotates the sprite in the direction of movement
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }
}
