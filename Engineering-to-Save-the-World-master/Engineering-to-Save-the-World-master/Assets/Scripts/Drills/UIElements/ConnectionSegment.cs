using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Drills {

public class ConnectionSegment : MonoBehaviour, IPointerEnterHandler, 
		IPointerExitHandler, IPointerClickHandler {

	// Reference to callouts for this connection segment.
	[HideInInspector]
	public GameObject callout;
	
	// Function type for mouse events.
	public delegate void MouseAction();

	// Called whenever the mouse clicks on this segment.
	public MouseAction OnMouseClicked;

	// Called whenever this connection segment is moused over.
	public MouseAction OnMouseEnter;

	// Called whenever the mouse exits this segment.
	public MouseAction OnMouseExit;

	// The default color of the segment. 
	private Color defaultColor;

    // Private flag to determine whether the the mouse entered the callout.
    [HideInInspector]
    public bool pointerEnteredCallout;

	// Activates the event for mouse clicks.
	public void OnPointerClick(PointerEventData eventData) {
		if (OnMouseClicked != null) {
			OnMouseClicked();
		}
    }

	// Activates the events for when the mouse enters the segment.
    public void OnPointerEnter(PointerEventData eventData) {
		if (OnMouseEnter != null) {
			OnMouseEnter();
		}
    }

	// Activates the event for whenever the mouse exits the segment.
    public void OnPointerExit(PointerEventData eventData) {
		if (OnMouseExit != null) {
			OnMouseExit();
		}
    }

	// Use this for initialization
	void Start () {
		defaultColor = GetComponent<CanvasRenderer>().GetColor();
		callout = transform.GetChild(0).gameObject;
        ConnectionCallout c = callout.GetComponent<ConnectionCallout>();
        c.OnMouseEnter += ToggleCalloutFlag;
	}
    
    // Deallocates resources before destroying segment.
    private void OnDestroy() {
        callout.GetComponent<ConnectionCallout>().OnMouseEnter -= ToggleCalloutFlag;
    }

    //Sets the color of the individual segment.
    public void SetColor(Color c) {
		CanvasRenderer renderer = GetComponent<CanvasRenderer>();
		renderer.SetColor(c);
		renderer.SetAlpha(1);
	}

	// Resets the color of the individual segment.
	public void ResetColor() {
		CanvasRenderer renderer = GetComponent<CanvasRenderer>();
		renderer.SetColor(defaultColor);
		renderer.SetAlpha(1);
	}

	// Toggles to callout state based on the provided argument.
	public void SetCalloutActive(bool active) {
		callout.SetActive(active);
	}
    
    private void ToggleCalloutFlag() {
       pointerEnteredCallout = !pointerEnteredCallout;
    }
}

}