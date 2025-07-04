using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PharaoAI : EnemyAI
{
    public GridBasedMovement movement;

    private Vector3 currentTargetPosition;
    private List<Node> path;
    private int currentPathIndex;
    
    protected override void Start()
    {
        base.Start();
        ComputePath();
    }

    void Update()
    {
        if (target == null) return;
     
        if(currentTargetPosition != target.movePoint.position)
        {
            currentTargetPosition = target.movePoint.position;
            ComputePath();
        }

        FollowPath();
    }

    private void ComputePath()
    {
        if (target == null) return;
        
        Node startNode = NodeManager.Instance.GetNodeAt(movement.movePoint.position);
        Node endNode = NodeManager.Instance.GetNodeAt(currentTargetPosition);

        if (startNode != null && endNode != null)
        {
            path = AStar.GeneratePath(startNode, endNode);
            currentPathIndex = 0;
        }
    }

    private void FollowPath()
    {
        if (path == null || currentPathIndex >= path.Count) return;
        
        Node nextNode = path[currentPathIndex];
        Vector3 nextPosition = nextNode.GetPosition();

        if (Vector3.Distance(transform.position, nextPosition) < 0.1f)
        {
            currentPathIndex++;
            if (currentPathIndex >= path.Count) return;
            nextNode = path[currentPathIndex];
            nextPosition = nextNode.GetPosition();
        }

        Vector3 direction = (nextPosition - transform.position).normalized;
        movement.ChangeMovePoint(GetDirectionFromVector(direction));
    }

    private Movement.Direction GetDirectionFromVector(Vector3 direction)
    {
        if (direction.x > 0.5f) return Movement.Direction.East;
        if (direction.x < -0.5f) return Movement.Direction.West;
        if (direction.y > 0.5f) return Movement.Direction.North;
        if (direction.y < -0.5f) return Movement.Direction.South;
        return Movement.Direction.Center;
    }

    private void DebugDrawPath()
    {
        if (path == null || path.Count == 0) return;
        
        Color lineColor = Color.green;
        float lineDuration = 100f;
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i].GetPosition(), path[i + 1].GetPosition(), lineColor, lineDuration);
        }
    }
}
