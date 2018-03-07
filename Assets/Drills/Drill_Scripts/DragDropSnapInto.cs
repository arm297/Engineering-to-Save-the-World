using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Drag-drop behavior which "snaps into" particular bounds
 * or else returns to original location when dropped.
 */
public class DragDropSnapInto : DragDrop {

    // Whether the object should snap into center.
    public bool snapCenter;

    // Original location of the dragged object
    private Vector3 originalPos;

    // Possible object to snap into
    private List<GameObject> snapIntoObjects;

    // Renderers of possible objects to snap into.
    private List<Renderer> snapIntoRenderers;

    // Checks if dragged object is over possible "snap into" location. If it 
    // is, dragged object is "snapped into" new region, depending on the
    // settings for snapping into. Otherwise, returns object to original 
    // position.
    void OnMouseUpAsButton() {
        bool overSnapRegion = false;
        foreach(Renderer r in snapIntoRenderers) {
            BlockContainer rBlock = r.GetComponent<BlockContainer>();
            if (rBlock.Intersects2D(GetComponent<DragDrop>())) {
                overSnapRegion = true;
 
                // Snaps object into region
                if (snapCenter) {
                    SnapIntoCenter(r.gameObject);
                } else {
                
                }
                break;
            }
        }

        // Resets dragged object if not over "snap into" region.
        if (!overSnapRegion) {
            transform.position = originalPos;
        }
    }

	// Use this for initialization
	void Start() {
        base.Start();
        originalPos = transform.position;
        snapIntoObjects = new List<GameObject>(
            GameObject.FindGameObjectsWithTag("snap_into"));
        snapIntoRenderers = new List<Renderer>();

        // If object expands into region, store minimum size.
        if (snapEntire) {
            originalSize = objectRenderer.bounds.size();
        }

        foreach(GameObject go in snapIntoObjects) {
            snapIntoRenderers.Add(go.GetComponent<Renderer>());
        }

    }
	
	// Update is called once per frame
	void Update() {

    }

    // Moves the dragged object into the region, but not into the center.    
    private void SnapIntoRegion(GameObject snapToRegion) {
        Vector3 snapToPos = SnapIntoRegion.transform.position;
        Vector3 dragObjectPos = transform.position;

        // Get the screen coordinates of drag object and "snap into" region.
        Vector3 screenSnapToPos = Camera.main.WorldToScreenPoint(snapToPos);
        Vector3 screenDragObjectPos = Camera.main.WorldToScreenPoint(dragObjectPos);

        // Find new position for the dragged object.
        float dx = 0.5;
        float dz = 0.5;
        transform.position = new Vector3(
            dragObjectPos.x + dx, 
            dragObjectPos.y, 
            dragObjectPos.z + dz);
        Vector3 screenMovedPos = Camera.main.WorldToScreenPoint(transform.temp);
        float xCoeff = dx / (screenMovedPos.x - screenDragObjectPos.x);
        float zCoeff = dz / (screenMovedPos.z - screenDragObjectPos.z);
        float xMove = xCoeff * (screenSnapToPos.x - screenDragObjectPos.x);
        float zMove = zCoeff * (screenSnapToPos.y - screenDragObjectPos.y);
        transform.position = new Vector3(
            dragObjectPos.x + xMove, 
            dragObjectPos.y, 
            dragObjectPos.z + zMove);
    }

    // Moves the dragged object into the center of the given region.
    private void SnapIntoCenter(GameObject snapToRegion) {
        Vector3 snapToPos = snapToRegion.transform.position;
        Vector3 dragObjectPos = transform.position;
   
        // Get the screen coordinates of drag object and "snap into" region.
        Vector3 screenSnapToPos = Camera.main.WorldToScreenPoint(snapToPos);
        Vector3 screenDragObjectPos = Camera.main.WorldToScreenPoint(dragObjectPos);
 
        // Find new position of the game object.
        float dx = (screenSnapToPos.x - screenDragObjectPos.x) 
            / (snapToPos.x - dragObjectPos.x);
        float dz = (screenSnapToPos.z - screenDragObjectPos.z) 
            / (snapToPos.z - dragObjectPos.z);
        float xIntercept = screenDragObjectPos.x - dx * snapToPos.x;
        float zIntercept = screenDragObjectPos.z - dz * snapToPos.z;
        float xCoordNew = (screenDragObjectPos.x - xIntercept) / dx;
        float zCoordNew = (screenDragObjectPos.z - zIntercept) / dz;
   
        transform.position = new Vector3(xCoordNew, dragObjectPos.y, zCoordNew);
    }
 
}
