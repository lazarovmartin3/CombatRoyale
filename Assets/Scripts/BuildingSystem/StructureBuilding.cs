using UnityEngine;

public class StructureBuilding : MonoBehaviour
{
    private GameObject targetUpgradeBuilding;
    private bool isUpgrading = false;

    private float max_uprgade_time = 2;
    private float upgrade_time_elapsed = 0;

    public void Upgrade(GameObject upgradeBuilding)
    {
        targetUpgradeBuilding = upgradeBuilding;
    }

    private void Update()
    {
        if (isUpgrading)
        {
            upgrade_time_elapsed += Time.deltaTime;
            if (upgrade_time_elapsed >= max_uprgade_time)
            {
                isUpgrading = false;
                GetComponent<MeshFilter>().mesh = targetUpgradeBuilding.GetComponent<MeshFilter>().mesh;
                upgrade_time_elapsed = 0;
            }
        }
    }
}
