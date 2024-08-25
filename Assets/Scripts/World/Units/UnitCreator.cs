using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreator : MonoBehaviour
{
    public enum UnitType { swordman, range, healer, builder, tank };

    [Serializable]
    public struct UnitTypes
    {
        public UnitType unitType;
        public GameObject prefab;
    }

    [SerializeField]
    private UnitTypes[] unitTypesSetup;
    
    private Dictionary<UnitType, GameObject> unitsByTypeList = new Dictionary<UnitType, GameObject>();


    private List<UnitType> unitsQueue = new List<UnitType>();
    private float creationTime = 2;
    private float creationTimeElapsed = 0;

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
}
