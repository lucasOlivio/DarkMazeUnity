using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeManager : MonoBehaviour
{
    public Node nodePrefab;
    private Grid<Node> nodeGrid;
    public Tilemap tilemap;

    public static NodeManager Instance { get; private set; }

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
        NodeMaker.GenerateGrid(tilemap, out nodeGrid);
        NodeMaker.ConnectNodes(nodeGrid);
    }

    void Update()
    {
        DebugNodesConnections();
    }

    public Node GetNodeAt(Vector3 position)
    {
        if(nodeGrid == null)
        {
            return null;
        }

        return nodeGrid.GetGridObject(position);
    }

    public IEnumerable<Node> YieldNodes()
    {
        for (int x = 0; x < nodeGrid.GetWidth(); x++) {
            for (int y = 0; y < nodeGrid.GetHeight(); y++) {
                yield return nodeGrid.GetGridObject(x, y);
            }
        }
    }

    /// <summary>
    /// Check if node exists for this world position and if is walkable
    /// </summary>
    /// <param name="movement">The movement vector to apply to the current position.</param>
    public bool IsNodeAccessible(Vector3 position)
    {
        Node node = nodeGrid.GetGridObject(position);

        return node != null && node.isWalkable;
    }

    private void DebugNodesConnections()
    {
        Color lineColor = Color.blue;
        Color nodeColor = Color.green;
        float lineDuration = 100f;

        for (int x = 0; x < nodeGrid.GetWidth(); x++)
        {
            for (int y = 0; y < nodeGrid.GetHeight(); y++)
            {
                Node node = nodeGrid.GetGridObject(x, y);
                
                if (node.connections == null || node.connections.Count == 0)
                {
                    continue;
                }

                for (int i = 0; i < node.connections.Count; i++)
                {
                    Node connection = node.connections[i];
                    Debug.DrawLine(nodeGrid.GetWorldPosition(node.x, node.y), nodeGrid.GetWorldPosition(connection.x, connection.y), lineColor, lineDuration);
                    Debug.DrawRay(nodeGrid.GetWorldPosition(node.x, node.y), new Vector3(0f, 0.3f), nodeColor, lineDuration, false);
                }
            }
        }
    }
}
