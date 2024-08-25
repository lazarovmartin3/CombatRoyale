using UnityEngine;

public class Node
{
    public int x, y;
    public bool walkable;
    public Vector3 worldPosition;

    public int gCost; // Distance from the start node
    public int hCost; // Distance from the target node
    public int fCost => gCost + hCost; // gCost + hCost
    public Node parent;

    public Node(int x, int y, bool walkable, Vector3 worldPosition)
    {
        this.x = x;
        this.y = y;
        this.walkable = walkable;
        this.worldPosition = worldPosition;
    }
}
