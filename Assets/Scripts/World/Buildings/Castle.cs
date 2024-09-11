using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    private int currentLevel = 1;

    private Dictionary<int,int> levelsCost = new Dictionary<int,int>();
    private Dictionary<int,int> goldPerLevel = new Dictionary<int,int>();

    private float earningInterval = 2;
    private float earingnIntervalElapsed = 0;

    private Player owner;
    private Vector2Int position;

    private SpawnableAreaCreator spawnableAreaCreator;

    private void Awake()
    {
        spawnableAreaCreator = GetComponent<SpawnableAreaCreator>();
    }

    private void Start()
    {
        levelsCost.Add(2, 100);
        levelsCost.Add(3, 200);
        levelsCost.Add(4, 400);

        goldPerLevel.Add(1, 5);
        goldPerLevel.Add(2, 10);
        goldPerLevel.Add(3, 13);
        goldPerLevel.Add(4, 17);
    }

    public void SetOwner(Player owner)
    {
        this.owner = owner;
    }

    public Player GetOwner()
    {
        return this.owner;
    }

    public void SetGridPosition(int x, int y)
    {
        position = new Vector2Int(x, y);
        Map.Instance.ChangeOwner(x, y, owner);
    }

    public Vector2Int GetPosition()
    {
        return position;
    }

    public void LevelUP()
    {
        if (owner.GetGold() >= levelsCost[currentLevel + 1])
        {
            owner.AddRemoveGold(-goldPerLevel[currentLevel + 1]);
            currentLevel++;
        }
        else
        {
            print("Not enough gold! Need " + (goldPerLevel[currentLevel + 1] - owner.GetGold()));
        }
    }

    public void ShowSpawnAreas()
    {
        List<Tile> areas = new List<Tile>(spawnableAreaCreator.GetSpawnableArea(owner));

        foreach(Tile tile in areas)
        {
            tile.ShowSpawnableArea();
        }
        TileSelector.OnTileSelectionEvent += GetComponent<UnitCreator>().SpawnUnit;
        GetComponent<UnitCreator>().OnAllUnitsCreatedSpawned += HideSpawnAreas;
    }

    public void HideSpawnAreas()
    {
        List<Tile> areas = new List<Tile>(spawnableAreaCreator.GetSpawnableArea(owner));

        foreach (Tile tile in areas)
        {
            tile.HideSpawnableArea();
        }
        TileSelector.OnTileSelectionEvent -= GetComponent<UnitCreator>().SpawnUnit;
    }

    public void CreateSpawnableArea()
    {
        spawnableAreaCreator.CreateSpanwableAreaAround(position.x, position.y, owner);
    }

    public void CreateUnit(UnitCreator.UnitType unitType)
    {
        int cost = GetComponent<UnitCreator>().GetUnitCost(unitType);
        if (owner.GetGold() >= cost)
        {
            GetComponent<UnitCreator>().CreateUnit(unitType);
            owner.AddRemoveGold(-cost);
        }
        else
        {
            print("Not enough gold!");
            //Implement a message logic
        }
    }

    public void AssignUnitToPlayer(Unit unit)
    {
        owner.AddUnit(unit);
    }

    private void Update()
    {
        earingnIntervalElapsed += Time.deltaTime;
        if(earingnIntervalElapsed >= earningInterval)
        {
            earingnIntervalElapsed = 0;
            AddGold();
        }
    }

    private void AddGold()
    {
        owner.AddRemoveGold(goldPerLevel[currentLevel]);
    }
}
