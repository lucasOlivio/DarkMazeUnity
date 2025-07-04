using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DM
{
    public class NodeSystem
    {
        public Tilemap tilemap;
        private Grid<Node> nodeGrid;

        public void Init()
        {
            tilemap = GameObject.Find(TilesInfo.tilemapGround).GetComponent<Tilemap>();
        }

        public void UpdateNodes()
        {
            GenerateGrid();
            ConnectNodes();
        }
        
        private void GenerateGrid()
        {
            nodeGrid = new Grid<Node>(tilemap.size.x, tilemap.size.y, tilemap.cellSize.x,
                                        Vector3.zero, (Grid<Node> g, int x, int y) => new Node(x, y));

            for (int x = 0; x < nodeGrid.GetWidth(); x++)
            {
                for (int y = 0; y < nodeGrid.GetHeight(); y++)
                {
                    Node node = nodeGrid.GetGridObject(x, y);
                    node.isWalkable = tilemap.HasTile(new Vector3Int(x, y, 0));
                }
            }
        }

        private void ConnectNodes()
        {
            for (int x = 0; x < nodeGrid.GetWidth(); x++)
            {
                for (int y = 0; y < nodeGrid.GetHeight(); y++)
                {
                    Node node = nodeGrid.GetGridObject(x, y);

                    if (node != null & node.isWalkable)
                    {
                        // Check adjacent nodes (up, down, left, right)
                        TryConnectNode(node, x + 1, y);
                        TryConnectNode(node, x - 1, y);
                        TryConnectNode(node, x, y + 1);
                        TryConnectNode(node, x, y - 1);
                    }
                }
            }
        }

        private void TryConnectNode(Node node, int x, int y)
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

        public Node GetNodeAt(Vector3 position)
        {
            if (nodeGrid == null)
            {
                return null;
            }

            return nodeGrid.GetGridObject(position);
        }

        public IEnumerable<Node> YieldNodes()
        {
            for (int x = 0; x < nodeGrid.GetWidth(); x++)
            {
                for (int y = 0; y < nodeGrid.GetHeight(); y++)
                {
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

        public void DebugNodesConnections()
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
}
