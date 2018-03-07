using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Drag-drop behavior which "snaps into" particular bounds
 * or else returns to original location when dropped.
 */
public class DragDropSnapInto : DragDrop {

    // Whether the object takes up the entire body of which it snaps into.
    public bool snapEntirely;

    // Original location of the dragged object
    private Vector3 originalPos;

    // Possible object to snap into
    private List<GameObject> snapIntoObjects;

    private List<Renderer> snapIntoRenderers;

    // Save original position of clicked mouse.
    void OnMouseDown()
    {
        originalPos = transform.position;
    }

    void OnMouseUpAsButton()
    {
        foreach(GameObject go in snapIntoObjects)
        {

        }
    }

	// Use this for initialization
	void Start ()
    {
        base.Start();
        snapIntoObjects = new List<GameObject>(
            GameObject.FindGameObjectsWithTag("snap_into"));
        snapIntoRenderers = new List<Renderer>();
        foreach(GameObject go in snapIntoObjects)
        {
            snapIntoRenderers.Add(go.GetComponent<Renderer>());
        }

    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    // Snaps the dragged object into the center of the given region.
    private void SnapIntoCenter(GameObject snapToRegion)
    {
        Vector3 snapToPos = snapToRegion.transform.position;
        Vector3 dragObjectPos = transform.position;
   
        // Get the screen coordinates of drag object and "snap into" region.
        Vector3 screenSnapToPos = Camera.main.WorldToScreenPoint(snapToPos);
        Vector3 screenDragObjectPos = Camera.main.WorldToScreenPoint(dragObjectPos);
 
        // Find new position of the game object.
        float dx = (screenSnapToPos.x - screenDragObjectPos.x) / (snapToPos.x - dragObjectPos.x);
        float dz = (screenSnapToPos.z - screenDragObjectPos.z) / (snapToPos.z - dragObjectPos.z);
        float xIntercept = screenDragObjectPos.x - dx * snapToPos.x;
        float zIntercept = screenDragObjectPos.z - dz * snapToPos.z;
        float xCoordNew = (screenDragObjectPos.x - xIntercept)/dx;
        float zCoordNew = (screenDragObjectPos.z - zIntercept)/dz;
   
        transform.position = new Vector3(xCoordNew, dragObjectPos.y, zCoordNew);
    }
 

    // Get component which object can snap into, or null if no such component
    // exists.
    private GameObject IntersectsObjectBounds()
    {
        foreach(Renderer objRenderer in snapIntoRenderers)
        {

        }
        return null;
    }

}
