using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Drills {
    /**
     * Behavior for containers to place dragged objects into. This container can
     * only take one dragged object.
     */
    public class BlockContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        // ID for matching block with dragged components.
        public int blockID;

        // Drag-droppeds object contained by this block, if any.
        public DragDrop containedObject { get; set; }

        // Highlight color when dragged object is over block.
        [SerializeField]
        private Color highlightColor;

        // Original color of this container block.
        private Color defaultColor;

        // Renderer for this container block.
        private CanvasRenderer blockRenderer;

        // Drag-dropped hovering object if any.
        private DragDrop hoverObject;

        // Event for adding listener when the block is placed.
        public UnityEvent blockPlacedEvent;

        // Event for added listener when the block is removed.
        public UnityEvent blockRemovedEvent;

        // Function types for actions performed during drop and pick up.
        public delegate void OnBlockAction();

        // Function called when dragged object is placed in block.
        public OnBlockAction OnBlockPlaced;

        // Function called when dragged object is picked up from block.
        public OnBlockAction OnBlockRemoved;

        // When the pointer enters, set Block color and the location of any 
        // dragged object.
        public void OnPointerEnter(PointerEventData eventData) {
            if (IsEmpty()) {
                blockRenderer.SetColor(highlightColor);
                blockRenderer.SetAlpha(1);

                if (eventData.pointerDrag == null) {
                    return;
                }
                DragDrop dragdrop = eventData.pointerDrag.GetComponent<DragDrop>();
                if (dragdrop != null) {
                    hoverObject = dragdrop;
                    dragdrop.returnPosition = transform.position;
                    dragdrop.returnParent = transform;
                }
            }
        }


        // When the pointer exits, reset Block color and the location of any 
        // dragged object.
        public void OnPointerExit(PointerEventData eventData) {
            if (IsEmpty()) {
                blockRenderer.SetColor(defaultColor);
            }
            if (eventData != null && eventData.pointerDrag != null) {
                hoverObject = eventData.pointerDrag.GetComponent<DragDrop>();
            }
            if (hoverObject != null) {
                hoverObject.returnPosition = hoverObject.originalPosition;
                hoverObject.returnParent = hoverObject.originalParent;
                hoverObject = null;
            }
        }

        // Use this for initialization.
        public virtual void Start() {
            blockRenderer = GetComponent<CanvasRenderer>();
            defaultColor = blockRenderer.GetColor();
            if (blockPlacedEvent == null) {
                blockPlacedEvent = new UnityEvent();
            }
            if(blockRemovedEvent == null) {
                blockRemovedEvent = new UnityEvent();
            }  
        }

        // Returns whether this block contained a drag-dropped object.
        public bool IsFilled() {
            return containedObject != null;
        }

        // Returns whether this container is empty.
        public bool IsEmpty() {
            return containedObject == null;
        }

        // Adds a contained object if the container is not filled. Returns whether
        // the object was added.
        public bool AddContainedObject(DragDrop dragObject) {
            if (IsEmpty()) {
                containedObject = dragObject;

                // Sets the color to the contained object's color.
                blockRenderer.SetColor(dragObject.GetComponent<CanvasRenderer>().GetMaterial().color);

                // Call delegate if provided.
                if (OnBlockPlaced != null) {
                    OnBlockPlaced();
                }

                // Invoke event if provided.
                if (blockPlacedEvent != null) {
                    blockPlacedEvent.Invoke();
                }
                return true;
            }
            return false;
        }

        // Removes the contained object of this block.
        public void RemoveContainedObject() {
            blockRenderer.SetColor(defaultColor);
            containedObject = null;

            // Call delegate if provided.
            if (OnBlockRemoved != null) {
                OnBlockRemoved();
            }

            // Invoke event if provided
            if (blockRemovedEvent != null) {
                blockRemovedEvent.Invoke();
            }
        }

    }

}