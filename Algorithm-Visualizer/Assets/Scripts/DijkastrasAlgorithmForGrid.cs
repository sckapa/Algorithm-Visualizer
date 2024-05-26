using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DijkastrasAlgorithmForGrid : MonoBehaviour
{
    [SerializeField]
    private UserInputToChangeGridTiles grid;
    [SerializeField]
    private TileBase blueTile;
    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]
    private TileBase orangeTile;

    List<Node> openSet = new List<Node>();
    HashSet<Node> closedSet = new HashSet<Node>();

    public class Node
    {
        public int Value { get; set; }
        public Node Next { get; set; }
        public Vector3Int position;
        public int Cost;
        public Node parent;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindPath();
        }
    }

    public void FindPath()
    {
        Node startNode = new Node();
        startNode.position = grid.startPosition;
        startNode.Cost = 0;
        Node endNode = new Node();
        endNode.position = grid.endPosition;

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            int minVal = int.MaxValue;
            Node currentNode = new Node();

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].Cost < minVal)
                {
                    minVal = openSet[i].Cost;
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.position == endNode.position)
            {
                PrintLinkedList(currentNode);
                break;
            }

            for (int i = 0; i < 8; i++)
            {
                Node neighbour = new Node();
                neighbour = GetNeighbours(currentNode.position)[i];

                if (!grid.Walkable(neighbour.position) || CheckIfNodeIsInHashset(neighbour, closedSet))
                {
                    continue;
                }

                tileMap.SetTile(neighbour.position, blueTile);

                int newMovementCostToNeighbour = currentNode.Cost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.Cost || !CheckIfNodeIsInList(neighbour, openSet))
                {
                    neighbour.Cost = newMovementCostToNeighbour;
                    neighbour.parent = currentNode;

                    if (!CheckIfNodeIsInList(neighbour, openSet))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    private bool CheckIfNodeIsInList(Node node, List<Node> list)
    {
        foreach (Node n in list)
        {
            if (n.position == node.position)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckIfNodeIsInHashset(Node node, HashSet<Node> hashSet)
    {
        foreach (Node n in hashSet)
        {
            if (n.position == node.position)
            {
                return true;
            }
        }
        return false;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
        int dstY = Mathf.Abs(nodeA.position.y - nodeB.position.y);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }

    private Node[] GetNeighbours(Vector3Int currentNode)
    {
        Node[] neighbours = new Node[8];

        for (int i = 0; i < 8; i++)
        {
            neighbours[i] = new Node();
            neighbours[i].Cost = int.MaxValue;
        }

        neighbours[0].position = new Vector3Int(currentNode.x + 1, currentNode.y, currentNode.z);
        neighbours[1].position = new Vector3Int(currentNode.x - 1, currentNode.y, currentNode.z);
        neighbours[2].position = new Vector3Int(currentNode.x, currentNode.y + 1, currentNode.z);
        neighbours[3].position = new Vector3Int(currentNode.x, currentNode.y - 1, currentNode.z);
        neighbours[4].position = new Vector3Int(currentNode.x + 1, currentNode.y + 1, currentNode.z);
        neighbours[5].position = new Vector3Int(currentNode.x - 1, currentNode.y - 1, currentNode.z);
        neighbours[6].position = new Vector3Int(currentNode.x + 1, currentNode.y - 1, currentNode.z);
        neighbours[7].position = new Vector3Int(currentNode.x - 1, currentNode.y + 1, currentNode.z);

        return neighbours;
    }

    public void PrintLinkedList(Node node)
    {
        Node currentNode = node;
        while (currentNode != null)
        {
            Debug.Log(currentNode.position);
            tileMap.SetTile(currentNode.position, orangeTile);
            currentNode = currentNode.parent;
        }
    }
}
