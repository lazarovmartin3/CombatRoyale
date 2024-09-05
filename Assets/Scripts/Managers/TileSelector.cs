using System;
using UnityEngine;


public class TileSelector : MonoBehaviour
{
    public static event Action<GameObject> OnTileSelectionEvent;

    private float holdingFingerDown_timeElapsed = 0;
    private float maxHoldingFingerDownTime = 0.5f;
    private bool isFingerDown;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnTileSelectionEvent?.Invoke(GetSelectedTile());
            isFingerDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isFingerDown = false;
        }

        if (isFingerDown)
        {
            holdingFingerDown_timeElapsed += Time.deltaTime;
            if (holdingFingerDown_timeElapsed >= maxHoldingFingerDownTime)
            {
                OnTileSelectionEvent?.Invoke(GetSelectedTile());
                holdingFingerDown_timeElapsed = 0;
            }
        }
    }

    private GameObject GetSelectedTile()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GameObject tile = null;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.gameObject.GetComponentInParent<Tile>() != null && hit.collider.gameObject.GetComponentInParent<Tile>().IsSpawnable)
            {
                tile = hit.collider.gameObject.transform.parent.gameObject;
            }
        }

        return tile;
    }
}
