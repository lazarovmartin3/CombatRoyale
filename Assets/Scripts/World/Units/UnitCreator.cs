using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreator : MonoBehaviour
{
    public enum UnitType { swordman, range, healer, builder, tank , NaN };

    [Serializable]
    public struct UnitTypes
    {
        public UnitType unitType;
        public GameObject prefab;
    }

    [SerializeField]
    private UnitTypes[] unitTypesSetup;

    private Dictionary<UnitType, GameObject> unitsByTypeList = new Dictionary<UnitType, GameObject>();

    private List<UnitType> createdUnits = new List<UnitType>();
    private List<UnitType> unitsQueue = new List<UnitType>();
    private float creationTime = 2;
    private float creationTimeElapsed = 0;

    private UnitType selectedUnitToSpawn;

    private void Start()
    {
        for (int i = 0; i < unitTypesSetup.Length; i++)
        {
            unitsByTypeList.Add(unitTypesSetup[i].unitType, unitTypesSetup[i].prefab);
        }
    }

    public void CreateUnit(UnitType unitType)
    {
        unitsQueue.Add(unitType);
    }

    public void ActivateSpawning(UnitType unitType)
    {
        TileSelector.OnTileSelectionEvent += SpawnUnit;
        selectedUnitToSpawn = unitType;
    }

    private void Update()
    {
        if(unitsQueue.Count > 0)
        {
            creationTimeElapsed += Time.deltaTime;
            if (creationTimeElapsed > creationTime)
            {
                creationTimeElapsed = 0;
                AddUnit();
            }
        }
    }

    private void AddUnit()
    {
        createdUnits.Add(unitsQueue[0]);
        unitsQueue.RemoveAt(0);
    }

    public void SpawnUnit()
    {
        unitsByTypeList.TryGetValue(unitsQueue[0], out GameObject prefab);
        GameObject unit = Instantiate(prefab, transform.position, Quaternion.identity);
        unit.GetComponent<Unit>().SetType(unitsQueue[0]);
        unit.GetComponent<Unit>().SetPosition(GetComponent<Castle>().GetPosition());
        unit.GetComponent<Unit>().SetOwner(GetComponent<Castle>().GetOwner());
        unitsQueue.Remove(unitsQueue[0]);
        GetComponent<Castle>().AssignUnitToPlayer(unit.GetComponent<Unit>());

        Vector2Int targetPos = GetComponent<Castle>().GetPosition();
        targetPos = Map.Instance.GetTileInFront(targetPos.x, targetPos.y);
        unit.GetComponent<Unit>().GoTo(targetPos);
    }

    public void SpawnUnit(GameObject tile)
    {
        UnitType type = UnitType.NaN;
        foreach(UnitType unit in createdUnits)
        {
            if (unit == selectedUnitToSpawn)
            {
                type = unit;
                break;
            }
        }

        if(type != UnitType.NaN)
        {
            unitsByTypeList.TryGetValue(type, out GameObject prefab);
            GameObject unitObject = Instantiate(prefab, transform.position, Quaternion.identity);
            unitObject.GetComponent<Unit>().SetType(type);
            unitObject.GetComponent<Unit>().SetPosition(tile.GetComponent<Tile>().Position);
            unitObject.GetComponent<Unit>().SetOwner(GetComponent<Castle>().GetOwner());
            createdUnits.Remove(type);
            GetComponent<Castle>().AssignUnitToPlayer(unitObject.GetComponent<Unit>());

            //Vector2Int targetPos = GetComponent<Castle>().GetPosition();
            //targetPos = Map.Instance.GetTileInFront(targetPos.x, targetPos.y);
            //unit.GetComponent<Unit>().GoTo(targetPos);
        }
        TileSelector.OnTileSelectionEvent -= SpawnUnit;
    }
}
