using UnityEngine;

public class BuildingStructureManager : MonoBehaviour
{
    private GameObject selectedBuilding;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectObject();
        }
    }

    private void DetectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;
            if (clickedObject.GetComponent<Building>() != null)
            {
                SelectBuilding(clickedObject);
            }
        }
    }

    private void SelectBuilding(GameObject newBuilding)
    {
        if (selectedBuilding != null)
        {
            selectedBuilding.GetComponent<Building>().Deselect();
            selectedBuilding = newBuilding;
            selectedBuilding.GetComponent<Building>().Select();
        }
        else
        {
            selectedBuilding = newBuilding;
            selectedBuilding.GetComponent<Building>().Select();
        }
    }
}
