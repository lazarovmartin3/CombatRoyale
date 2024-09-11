using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map Instance;

    public GameObject tilePrefab;
    private int sizeX, sizeY;
    private GameObject[,] field;
    private float tileOffset = 5;

    private Node[,] grid;

    private void Awake()
    {
        Instance = this;
    }

    public void GenerateMap()
    {
        sizeX = 8;
        sizeY = 10;
        field = new GameObject[sizeX, sizeY];
        grid = new Node[sizeX, sizeY];

        for(int x = 0; x < sizeX; x++)
        {
            for(int y = 0; y < sizeY; y++)
            {
                Vector3 position = new Vector3(this.transform.position.x + x * tileOffset, 0, this.transform.position.y + y * tileOffset);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                tile.name = $"{x},{y}";
                tile.transform.SetParent(this.transform);
                tile.GetComponent<Tile>().Position = new Vector2Int(x, y);

                bool walkable = true;
                grid[x, y] = new Node(x, y, walkable, position);
                field[x, y] = tile;
            }
        }
    }

    public int MapSizeX() { return sizeX; }
    public int MapSizeY() {  return sizeY; }

    public Vector3 GetPosition(int x, int y)
    {
        return field[x, y].transform.position;
    }

    public Vector2Int GetXY_fromPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt((position.x - this.transform.position.x) / tileOffset);
        int y = Mathf.RoundToInt((position.z - this.transform.position.y) / tileOffset);
        return new Vector2Int(x, y);
    }

    public void ChangeOwner(int x, int y, Player newOwner)
    {
        field[x,y].GetComponent<Tile>().SetOwner(newOwner);
        grid[x, y].walkable = false;
    }

    public void SetSpawnableTile(int x, int y, bool value, Player owner)
    {
        field[x,y].GetComponent<Tile>().IsSpawnable = value;
        field[x, y].GetComponent<Tile>().SetOwner(owner);
    }

    public Tile GetTileAt(int x, int y)
    {
        return field[x, y].GetComponent<Tile>();
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = NodeFromWorldPoint(startPos);
        Node targetNode = NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null; // No path found
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.x - nodeB.x);
        int distY = Mathf.Abs(nodeA.y - nodeB.y);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

    private Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x - this.transform.position.x) / tileOffset);
        int y = Mathf.RoundToInt((worldPosition.z - this.transform.position.y) / tileOffset);
        return grid[x, y];
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.x + x;
                int checkY = node.y + y;

                if (checkX >= 0 && checkX < sizeX && checkY >= 0 && checkY < sizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public Vector2Int GetTileInFront(int x, int y)
    {
        // Attempt to get the tile in front by moving in the positive y-direction
        int frontX = x;
        int frontY = y + 1;

        // Check if the front position is within the bounds of the map
        if (frontY < sizeY)
        {
            return new Vector2Int(frontX, frontY);
        }
        else
        {
            // If out of bounds, return the tile behind by moving in the negative y-direction
            int behindY = y - 1;

            // Ensure the behind tile is within bounds (y >= 0)
            if (behindY >= 0)
            {
                return new Vector2Int(frontX, behindY);
            }
            else
            {
                Debug.LogError("Both front and behind tiles are out of map bounds.");
                return new Vector2Int(-1, -1); // This should rarely happen if the map is well-formed
            }
        }
    }
}
