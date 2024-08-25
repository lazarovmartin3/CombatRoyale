using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int gold;
    private Castle mainCastle;
    private List<Tower> towers;
    private List<Unit> units;
    private GameManager.PlayerInitData playerInitData;

    public void InitPlayer(int startGold, Castle castle, GameManager.PlayerInitData initData)
    {
        gold = startGold;
        mainCastle = castle;
        towers = new List<Tower>();
        units = new List<Unit>();
        playerInitData = initData;
        mainCastle.GetComponent<Renderer>().material = initData.buildingMaterial;
    }

    public void AddUnit(Unit unit)
    {
        unit.ChangeMaterial(playerInitData.unitMaterial);
        units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public List<Unit> GetUnits()
    {
        return units;
    }

    public void AddTower(Tower tower)
    {
        towers.Add(tower);
    }

    public void RemoveTower(Tower tower)
    {
        towers.Remove(tower);
    }

    public void AddRemoveGold(int amount)
    {
        gold += amount;
    }

    public int GetGold() { return gold; }

    public Castle GetSelectedCastle()
    {
        return mainCastle;
    }
}
