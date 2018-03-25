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

    // Possible parent of objects to snap into.
    [SerializeField]
    private GameObject snapToParent;

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
        if (returnPosition != originalPos) {
            containingBlock.AddContainedObject(GetComponent<DragDropSnapInto>());
        }             
        transform.position = returnPosition;
    }

    // Resets the containing block, if there is one.
    void OnMouseDown() {
        if (containingBlock != null) {
            containingBlock.GetComponent<BlockContainer>().RemoveContainedObject();
            containingBlock = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        //base.OnEndDrag(eventData);
        if (containingBlock != null) {
            containingBlock.GetComponent<BlockContainer>().RemoveContainedObject();
            containingBlock = null;
        }
        transform.position = returnPosition;
    }

	// Use this for initialization
	public void Start() {
        base.Start();
        highlightColor = Color.red;
        originalPos = transform.position;
        returnPosition = transform.position;
        if (snapToParent != null) {
            snapIntoObjects = new List<BlockContainer>(
                snapToParent.GetComponents<BlockContainer>());
        }
        else {
            snapIntoObjects = new List<BlockContainer>(
                GetComponents<BlockContainer>());
        }
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
