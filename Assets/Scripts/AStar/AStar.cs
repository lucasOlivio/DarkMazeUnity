using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public static List<Node> GeneratePath(Node start, Node end)
    {
        List<Node> openSet = new List<Node>();

        foreach(Node n in NodeManager.Instance.YieldNodes())
        {
            n.gScore = float.MaxValue;
        }

        start.gScore = 0;
        start.hScore = Vector2.Distance(start.GetPosition(), end.GetPosition());
        openSet.Add(start);

        while(openSet.Count > 0)
        {
            int lowestF = default;

            for(int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FScore() < openSet[lowestF].FScore())
                {
                    lowestF = i;
                }
            }

            Node currentNode = openSet[lowestF];
            openSet.Remove(currentNode);

            if(currentNode == end)
            {
                List<Node> path = new List<Node>();

                path.Insert(0, end);

                while(currentNode != start)
                {
                    currentNode = currentNode.cameFrom;
                    path.Add(currentNode);
                }

                path.Reverse();
                return path;
            }

            foreach(Node connectedNode in currentNode.connections)
            {
                float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.GetPosition(), connectedNode.GetPosition());

                if(heldGScore < connectedNode.gScore)
                {
                    connectedNode.cameFrom = currentNode;
                    connectedNode.gScore = heldGScore;
                    connectedNode.hScore = Vector2.Distance(connectedNode.GetPosition(), end.GetPosition());

                    if (!openSet.Contains(connectedNode))
                    {
                        openSet.Add(connectedNode);
                    }
                }
            }
        }

        return null;
    }

    public static Node FindNearestNode(Vector2 pos)
    {
        Node foundNode = null;
        float minDistance = float.MaxValue;

        foreach(Node node in NodeManager.Instance.YieldNodes())
        {
            float currentDistance = Vector2.Distance(pos, node.GetPosition());

            if(currentDistance < minDistance)
            {
                minDistance = currentDistance;
                foundNode = node;
            }
        }

        return foundNode;
    }

    public static Node FindFurthestNode(Vector2 pos)
    {
        Node foundNode = null;
        float maxDistance = default;

        foreach (Node node in NodeManager.Instance.YieldNodes())
        {
            float currentDistance = Vector2.Distance(pos, node.GetPosition());
            if(currentDistance > maxDistance)
            {
                maxDistance = currentDistance;
                foundNode = node;
            }
        }

        return foundNode;
    }
}
