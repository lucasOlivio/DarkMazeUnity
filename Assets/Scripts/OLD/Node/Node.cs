using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int x;
    public int y;

    public Node cameFrom;
    public List<Node> connections;

    public float gScore;
    public float hScore;

    public bool isWalkable;

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
        connections = new List<Node>();
    }

    public float FScore()
    {
        return gScore + hScore;
    }

    public Vector2 GetPosition()
    {
        return new Vector2(x, y);
    }
}
