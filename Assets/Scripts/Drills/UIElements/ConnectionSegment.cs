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
	public event MouseAction OnMouseClicked();

	// Called whenever this connection segment is moused over.
	public event MouseAction OnMouseEnter();

	// Called whenever the mouse exits this segment.
	public event MouseAction OnMouseExit();

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
		callout = transform.GetChild(0).gameObject;
	}

	// Toggles to callout state based on the provided argument.
	public SetCalloutActive(bool active) {
		callout.SetActive(active);
	}

	// Set color of the segment to specified color.
	public SetColor(Color c) {
		CanvasRenderer renderer = gameObject.GetComponent<CanvasRenderer>();
		renderer.SetColor(c);
	}
}

}