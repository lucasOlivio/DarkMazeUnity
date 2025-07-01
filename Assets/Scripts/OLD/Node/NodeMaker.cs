using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeMaker
{
    public static void GenerateGrid(Tilemap tilemap, out Grid<Node> nodeGrid)
    {
        nodeGrid = new Grid<Node>(tilemap.size.x, tilemap.size.y, tilemap.cellSize.x, 
                                    Vector3.zero, (Grid<Node> g, int x, int y) => new Node(x, y));


        for (int x = 0; x < nodeGrid.GetWidth(); x++) {
            for (int y = 0; y < nodeGrid.GetHeight(); y++) {
                Node node = nodeGrid.GetGridObject(x, y);
                node.isWalkable = tilemap.HasTile(new Vector3Int(x, y, 0));
            }
        }
    }

    public static void ConnectNodes(Grid<Node> nodeGrid)
    {
        for (int x = 0; x < nodeGrid.GetWidth(); x++) {
            for (int y = 0; y < nodeGrid.GetHeight(); y++) {
                Node node = nodeGrid.GetGridObject(x, y);

                if (node != null & node.isWalkable)
                {
                    // Check adjacent nodes (up, down, left, right)
                    TryConnectNode(nodeGrid, node, x + 1, y);
                    TryConnectNode(nodeGrid, node, x - 1, y);
                    TryConnectNode(nodeGrid, node, x, y + 1);
                    TryConnectNode(nodeGrid, node, x, y - 1);
                }
            }
        }
    }

    private static void TryConnectNode(Grid<Node> nodeGrid, Node node, int x, int y)
    {
        Node adjacentNode = nodeGrid.GetGridObject(x, y);
        if (adjacentNode != null && 
            adjacentNode.isWalkable &&
            !node.connections.Contains(adjacentNode))
        {
            node.connections.Add(adjacentNode);
            adjacentNode.connections.Add(node);
        }
    }
}
