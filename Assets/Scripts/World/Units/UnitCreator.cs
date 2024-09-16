using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitCreator : MonoBehaviour
{
    public enum UnitType { swordman, range, healer, builder, tank , NaN };

    [Serializable]
    public struct UnitTypes
    {
        public UnitType unitType;
        public GameObject prefab;
        public int cost;
    }

    [SerializeField]
    private UnitTypes[] unitTypesSetup;

    private Dictionary<UnitType, GameObject> unitsByTypeList = new Dictionary<UnitType, GameObject>();

    private List<UnitType> createdUnits = new List<UnitType>();
    private List<UnitType> unitsQueue = new List<UnitType>();
    private float creationTime = 2;
    private float creationTimeElapsed = 0;

    //Events
    public event Action OnAllUnitsCreatedSpawned;

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
        //Consider to limit the queue up to 5 units
        UpdateOrderedUnitsUI();
    }

    private void Update()
    {
        if(unitsQueue.Count > 0)
        {
            creationTimeElapsed += Time.deltaTime;
            if (GetComponent<Castle>().GetOwner() is HumanPlayer) 
                UnitCreationSystemUI.Instance.UpdateUnitCreationProgress(unitsQueue[0], (creationTimeElapsed / creationTime));
            
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

        UpdateCreatedUnitsUI();
        UpdateOrderedUnitsUI();
    }

    private void UpdateCreatedUnitsUI()
    {
        if (GetComponent<Castle>().GetOwner() is AIPlayer) return;

        Dictionary<UnitType, int> ready_units = new Dictionary<UnitType, int>();

        if (createdUnits.Count > 0)
        {
            for (int i = 0; i < createdUnits.Count; i++)
            {
                UnitType unitType = createdUnits[i];
                if (ready_units.ContainsKey(unitType))
                {
                    ready_units[createdUnits[i]] += 1;
                }
                else
                {
                    ready_units[unitType] = 1;
                }
            }
            foreach (var kvp in ready_units)
            {
                UnitCreationSystemUI.Instance.UpdateCreatedUnitsAmount(kvp.Key, kvp.Value);
            }
        }
        else
        {
            foreach (var unitType in unitTypesSetup)
            {
                UnitCreationSystemUI.Instance.UpdateCreatedUnitsAmount(unitType.unitType, 0);
            }
        }
    }

    private void UpdateOrderedUnitsUI()
    {
        if (GetComponent<Castle>().GetOwner() is AIPlayer) return;

        Dictionary<UnitType, int> queue_units = new Dictionary<UnitType, int>();

        if (unitsQueue.Count > 0)
        {
            for (int i = 0; i < unitsQueue.Count; i++)
            {
                UnitType unitType_ = unitsQueue[i];
                if (queue_units.ContainsKey(unitType_))
                {
                    queue_units[unitsQueue[i]] += 1;
                }
                else
                {
                    queue_units[unitType_] = 1;
                }
            }
            foreach (var kvp in queue_units)
            {
                UnitCreationSystemUI.Instance.UpdateOrderUnitsAmount(kvp.Key, kvp.Value);
            }
        }
        else
        {
            foreach (var unitType in unitTypesSetup)
            {
                UnitCreationSystemUI.Instance.UpdateOrderUnitsAmount(unitType.unitType, 0);
            }
        }
    }


    public void SpawnUnit(GameObject tile)
    {
        UnitType unitType = UnitCreationSystemUI.Instance.GetSelectedUnitType();

        if(createdUnits.Count > 0)
        {
            unitsByTypeList.TryGetValue(unitType, out GameObject prefab);
            if(prefab != null)
            {
                Vector3 position = tile.GetComponent<MeshCollider>().bounds.center;
                position.z = UnityEngine.Random.Range(tile.GetComponent<MeshCollider>().bounds.min.z, tile.GetComponent<MeshCollider>().bounds.max.z);
                GameObject unitObject = Instantiate(prefab, position, Quaternion.identity, transform);
                unitObject.GetComponent<Unit>().SetType(unitType);
                unitObject.GetComponent<Unit>().SetPosition(tile.GetComponent<Tile>().Position);
                unitObject.GetComponent<Unit>().SetOwner(GetComponent<Castle>().GetOwner());
                createdUnits.Remove(unitType);
                GetComponent<Castle>().AssignUnitToPlayer(unitObject.GetComponent<Unit>());

                UpdateCreatedUnitsUI();

                if (createdUnits.Count == 0 && GetComponent<Castle>().GetOwner() is HumanPlayer)
                {
                    OnAllUnitsCreatedSpawned.Invoke();
                }
            }
            else
            {
                if (GetComponent<Castle>().GetOwner() is HumanPlayer)
                    OnAllUnitsCreatedSpawned.Invoke();
            }
        }
        else
        {
            if (GetComponent<Castle>().GetOwner() is HumanPlayer)
                OnAllUnitsCreatedSpawned.Invoke();
        }
    }

    public int GetUnitCost(UnitType unitType)
    {
        for (int i = 0; i < unitTypesSetup.Length; i++)
        {
            if (unitTypesSetup[i].unitType == unitType)
            {
                return unitTypesSetup[i].cost;
            }
        }

        return 0;
    }
}
