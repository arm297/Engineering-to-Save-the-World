using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaborScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{

	public void OnBeginDrag(PointerEventData eventData) {
		Debug.Log ("Begin");
	}

	public void OnDrag(PointerEventData eventData) {
		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		pos.z = -65;
		transform.position = pos;
	}

	public void OnEndDrag(PointerEventData eventData) {
		Debug.Log ("End Drag");
	}
}
