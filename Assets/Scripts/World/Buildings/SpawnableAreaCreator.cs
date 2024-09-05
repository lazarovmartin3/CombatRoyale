using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnableAreaCreator : MonoBehaviour
{
    private int areaSize = 2;

    private HashSet<Tile> areaTiles = new HashSet<Tile>();

    public void CreateSpanwableAreaAround(int x, int y, Player owner)
    {
        areaTiles.Clear(); // Clear any previous area tiles

        int mapSizeX = Map.Instance.MapSizeX();
        int mapSizeY = Map.Instance.MapSizeY();

        for (int dx = -areaSize; dx <= areaSize; dx++)
        {
            for (int dy = -areaSize; dy <= areaSize; dy++)
            {
                int newX = x + dx;
                int newY = y + dy;

                // Check if within circular area and within map bounds
                if (IsWithinCircle(x, y, newX, newY) && IsWithinMapBounds(newX, newY, mapSizeX, mapSizeY))
                {
                    Map.Instance.SetSpawnableTile(newX, newY, true, owner);
                    areaTiles.Add(Map.Instance.GetTileAt(newX, newY));
                }
            }
        }
    }

    private bool IsWithinCircle(int centerX, int centerY, int x, int y)
    {
        return (x - centerX) * (x - centerX) + (y - centerY) * (y - centerY) <= areaSize * areaSize;
    }

    private bool IsWithinMapBounds(int x, int y, int mapSizeX, int mapSizeY)
    {
        return x >= 0 && x < mapSizeX && y >= 0 && y < mapSizeY;
    }


    public List<Tile> GetSpawnableArea(Player owner)
    {
        return areaTiles.ToList();
    }
}
