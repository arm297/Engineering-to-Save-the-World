using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Basic Drag drop behavior. Highlights moused over objects and moves selected
 * objects.
 */
public class DragDrop : MonoBehaviour {

    // Material of drag-dropped object
    protected Renderer objectRenderer;

    // Original settings of dragged object
    private Color defaultColor;

    // Highlight color for moused over objects
    public Color highlightColor;

    // Whether the current object is being dragged
    protected bool selected;

    // Highlight moused over objects
    protected void OnMouseEnter()
    {
        objectRenderer.material.color = highlightColor;
    }

    // Reset default color of object.
    protected void OnMouseExit()
    {
        if (!selected)
        {
            objectRenderer.material.color = defaultColor;
        }
    }


    protected void OnMouseDown()
    {
        selected = true;
    }

    protected void OnMouseDrag()
    {
        float screenDistance = Camera.main.WorldToScreenPoint(
                gameObject.transform.position).z;
   
        // Move to new point bounded by screen width and length.
        Vector3 positionMove = Camera.main.ScreenToWorldPoint(new Vector3(
                Mathf.Clamp(Input.mousePosition.x, 0, Screen.width),
                Mathf.Clamp(Input.mousePosition.y, 0, Screen.height),
                screenDistance));
        transform.position = new Vector3(positionMove.x, transform.position.y,
                positionMove.z);
    }

    protected void OnMouseUpAsButton()
    {
        selected = false;   
    }

    // Use this for initialization
    void Start ()
    {
        objectRenderer = GetComponent<Renderer>();
        defaultColor = objectRenderer.material.color;
	}
}
