using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Texture[] textures;
    public enum tileType { grass, dirt, water };
    public tileType currentTileType;

    public Player owner = null;

    private void Start()
    {
        currentTileType = tileType.grass;
    }

    public void ChangeTile(tileType newType)
    {
        currentTileType = newType;
        GetComponent<MeshRenderer>().material.mainTexture = textures[(int)currentTileType];
        
        switch (newType) 
        {
            case tileType.grass:
               
                break;
            case tileType.dirt:
                break;
            case tileType.water:
                break;
            default:
                break;
        }
    }

    public Player GetOwner()
    {
        return owner;
    }

    public void SetOwner(Player newOwner)
    {
        owner = newOwner;
    }

}
