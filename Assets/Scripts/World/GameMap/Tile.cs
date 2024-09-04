using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum tileType { grass, dirt, water };
    public tileType currentTileType;

    [Serializable]
    public struct TileData
    {
        public tileType type;
        public Texture texture;
    }

    [SerializeField]
    private TileData[] tileData;
    [SerializeField]
    private GameObject spawnableAreaObject;

    private Player owner = null;
    private bool isSpawnableTile = false;

    private Vector2Int position;

    private void Start()
    {
        ChangeTile(tileType.grass);
    }

    public void ChangeTile(tileType newType)
    {
        for(int i = 0; i < tileData.Length; i++)
        {
            if (tileData[i].type == newType)
            {
                currentTileType = newType;
                GetComponent<MeshRenderer>().material.mainTexture = tileData[i].texture;
            }
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

    public bool IsSpawnable
    {
        get { return isSpawnableTile; }
        set { isSpawnableTile = value; }
    }

    public void ShowSpawnableArea()
    {
        spawnableAreaObject.SetActive(true);
    }

    public void HideSpawnableArea()
    {
        spawnableAreaObject.SetActive(false);
    }

    public Vector2Int Position
    {
        get { return position; }
        set { position = value; }
    }
}
