using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor;

public class UnitCreationSystemUI : MonoBehaviour
{
    public static UnitCreationSystemUI Instance;

    [Serializable]
    public struct UnitCreationAndSpawningData
    {
        public Button createUnitBtn;
        public Button spawnUnitBtn;
        public UnitCreator.UnitType unitType;
    }

    [SerializeField] private UnitCreationAndSpawningData[] unitCreationAndSpawningDatas;

    private UnitCreator.UnitType selectedUnitType;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitButtons();
    }

    private void InitButtons()
    {
        for (int i = 0; i < unitCreationAndSpawningDatas.Length; i++)
        {
            // Capture the current value of i in a local variable
            int index = i;
            unitCreationAndSpawningDatas[index].createUnitBtn.onClick.AddListener(() =>
            {
                GameManager.Instance.GetPlayer().GetSelectedCastle().CreateUnit(unitCreationAndSpawningDatas[index].unitType);
            });

            unitCreationAndSpawningDatas[index].spawnUnitBtn.onClick.AddListener(() =>
            {
                selectedUnitType = unitCreationAndSpawningDatas[index].unitType;
                GameManager.Instance.GetPlayer().GetSelectedCastle().ShowSpawnAreas();
            });
        }
    }

    public void UpdateUnitCreationProgress(UnitCreator.UnitType unitType, float progress)
    {
        for (int i = 0; i < unitCreationAndSpawningDatas.Length; i++)
        {
            if (unitCreationAndSpawningDatas[i].unitType == unitType)
            {
                unitCreationAndSpawningDatas[i].spawnUnitBtn.GetComponent<UnitCreationUIButton>().UpdateProgress(progress);
                break;
            }
        }
    }

    public void UpdateCreatedUnitsAmount(UnitCreator.UnitType unitType, int amount)
    {
        for (int i = 0; i < unitCreationAndSpawningDatas.Length; i++)
        {
            if (amount == 0)
            {
                unitCreationAndSpawningDatas[i].spawnUnitBtn.interactable = false;
            }
            else
            {
                if (unitCreationAndSpawningDatas[i].unitType == unitType)
                {
                    unitCreationAndSpawningDatas[i].spawnUnitBtn.interactable = true;
                    unitCreationAndSpawningDatas[i].spawnUnitBtn.GetComponent<UnitCreationUIButton>().UpdateUnitsAmount(amount);
                    break;
                }
            }
        }
    }

    public void UpdateOrderUnitsAmount(UnitCreator.UnitType unitType, int amount)
    {
        for (int i = 0; i < unitCreationAndSpawningDatas.Length; i++)
        {
            if (unitCreationAndSpawningDatas[i].unitType == unitType)
            {
                unitCreationAndSpawningDatas[i].createUnitBtn.GetComponent<UnitCreationUIButton>().UpdateUnitsAmount(amount);
                break;
            }
        }
    }

    public UnitCreator.UnitType GetSelectedUnitType()
    {
        return selectedUnitType;
    }
}
