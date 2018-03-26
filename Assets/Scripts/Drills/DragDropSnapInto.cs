using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * Drag-drop behavior which "snaps into" particular bounds
 * or else returns to original location when dropped.
 */
public class DragDropSnapInto : DragDrop, IEndDragHandler, IBeginDragHandler, IDragHandler {

    // Original location of the dragged object
    [HideInInspector]
    public Vector3 originalPos;

    // Position to place the object on drop.
    [HideInInspector]
    public Vector3 returnPosition;

    // Parent to return to.
    [HideInInspector]
    public Transform originalParent;

    // Possible object to snap into
    private List<BlockContainer> snapIntoObjects;

    // Renderers of possible objects to snap into.
    private List<Transform> snapIntoTransforms;

    // Current object snapped into, or null is no containing object.
    [HideInInspector]
    public BlockContainer containingBlock;

    // Checks if dragged object is over possible "snap into" location. If it 
    // is, dragged object is "snapped into" new region, depending on the
    // settings for snapping into. Otherwise, returns object to original 
    // position.
    void OnMouseUpAsButton() {
        //if (returnPosition != originalPos) {
        //    containingBlock.AddContainedObject(GetComponent<DragDropSnapInto>());
        //}             
        //transform.position = returnPosition;
    }

    // Resets the containing block, if there is one.
    void OnMouseDown() {

   }

    public new void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public new void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

    public new void OnEndDrag(PointerEventData eventData) {
        base.OnEndDrag(eventData);
        if (containingBlock != null) {
            containingBlock.GetComponent<BlockContainer>().RemoveContainedObject();
            containingBlock = null;
        }
        transform.position = returnPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

	// Use this for initialization
	public new void Start() {
        base.Start();
        originalPos = transform.position;
        returnPosition = transform.position;

        snapIntoObjects = new List<BlockContainer>(GetComponents<BlockContainer>());
        snapIntoTransforms = new List<Transform>();
        foreach(BlockContainer go in snapIntoObjects) {
            snapIntoTransforms.Add(go.transform);
        }

    }
	
	// Update is called once per frame
	void Update() {

    }

    // Moves the dragged object into the center of the given region.
    private void SnapIntoCenter(GameObject snapToRegion) {
        RectTransform location = snapToRegion.transform as RectTransform;
        Vector2 snapToCenter = location.rect.center;
        transform.position = new Vector3(snapToCenter.x, snapToCenter.y, transform.position.z);
    }


 
}
