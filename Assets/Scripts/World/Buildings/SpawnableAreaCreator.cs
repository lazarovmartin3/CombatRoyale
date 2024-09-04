using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnableAreaCreator : MonoBehaviour
{
    private int areaSize = 3;

    private HashSet<Tile> areaTiles = new HashSet<Tile>();

    public void CreateSpanwableAreaAround(int x, int y, Player owner)
    {
        for (int i = x; i < x + areaSize; i++)
        {
            if (i < Map.Instance.MapSizeX())
            {
                Map.Instance.SetSpawnableTile(i, y, true, owner);
                areaTiles.Add(Map.Instance.GetTileAt(i, y));
            }
        }

        for (int i = x; i > x - areaSize; i--)
        {
            if (i > 0)
            {
                Map.Instance.SetSpawnableTile(i, y, true, owner);
                areaTiles.Add(Map.Instance.GetTileAt(i, y));
            }
        }

        for (int i = y; i < y + areaSize; i++)
        {
            if (i < Map.Instance.MapSizeY())
            {
                Map.Instance.SetSpawnableTile(x, i, true, owner);
                areaTiles.Add(Map.Instance.GetTileAt(x, i));
            }
        }

        for (int i = y; i > y - areaSize; i--)
        {
            if (i > 0)
            {
                Map.Instance.SetSpawnableTile(x, i, true, owner);
                areaTiles.Add(Map.Instance.GetTileAt(x, i));
            }
        }
    }

    public List<Tile> GetSpawnableArea(Player owner)
    {
        //List<Tile> list = new List<Tile>();

        //foreach(Tile tile in areaTiles)
        //{
        //    if (tile.GetOwner() == owner)
        //    {
        //        list.Add(tile);
        //    }
        //}
        //return list;
        return areaTiles.ToList();
    }
}
