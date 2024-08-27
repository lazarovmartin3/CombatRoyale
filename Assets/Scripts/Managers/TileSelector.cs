using System;
using UnityEngine;


public class TileSelector : MonoBehaviour
{
    public static event Action<GameObject> OnTileSelectionEvent;

    void Start()
    {
        
    }

   
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (OnTileSelectionEvent != null)
            {
                OnTileSelectionEvent.Invoke(GetSelectedTile());
            }
        }
    }

    private GameObject GetSelectedTile()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.GetComponent<Tile>() != null && hit.collider.gameObject.GetComponent<Tile>().IsSpawnable)
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }
}
