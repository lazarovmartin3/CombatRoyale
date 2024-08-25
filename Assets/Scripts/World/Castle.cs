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

    public void CreateUnit(UnitCreator.UnitType unitType)
    {
        if (owner.GetGold() >= 100)
        {
            GetComponent<UnitCreator>().CreateUnit(unitType);
            owner.AddRemoveGold(-100);
        }
        else
        {
            print("Not enough gold!");
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
