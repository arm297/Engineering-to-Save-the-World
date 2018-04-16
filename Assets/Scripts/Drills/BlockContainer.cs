using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Drills {
    /**
     * Behavior for containers to place dragged objects into. This container can
     * only take one dragged object.
     */
    public class BlockContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        // Whether the current block is full. 
        [HideInInspector]
        public bool isFilled { get; private set; }

        // Highlight color when dragged object is over block.
        [SerializeField]
        private Color highlightColor;

        // Original color of this container block.
        private Color defaultColor;

        // Renderer for this container block.
        private CanvasRenderer blockRenderer;

        // Function types for actions performed during drop and pick up.
        public delegate void OnBlockAction();

        // Function called when dragged object is placed in block.
        public OnBlockAction OnBlockPlaced;

        // Function called when dragged object is picked up from block.
        public OnBlockAction OnBlockRemoved;

        // Drag-droppeds object contained by this block, if any.
        private DragDropSnapInto containedObject;

        // Drag-dropped hovering object if any.
        private DragDropSnapInto hoverObject;

        // Use this for initialization
        void Start()
        {
            isFilled = false;
            blockRenderer = GetComponent<CanvasRenderer>();
            defaultColor = blockRenderer.GetColor();
        }

        // When the pointer enters, set Block color and the location of any 
        // dragged object.
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (containedObject == null)
            {
                blockRenderer.SetColor(highlightColor);
                blockRenderer.SetAlpha(1);
            }
            if (eventData.pointerDrag == null)
            {
                return;
            }
            DragDropSnapInto dragdrop = eventData.pointerDrag.GetComponent<DragDropSnapInto>();
            if (!isFilled && dragdrop != null)
            {
                hoverObject = dragdrop;
                dragdrop.returnPosition = transform.position;
                dragdrop.returnParent = transform;
            }
        }


        // When the pointer exits, reset Block color and the location of any 
        // dragged object.
        public void OnPointerExit(PointerEventData eventData)
        {
            if (containedObject == null)
            {
                blockRenderer.SetColor(defaultColor);
            }
            if (eventData != null && eventData.pointerDrag != null)
            {
                hoverObject = eventData.pointerDrag.GetComponent<DragDropSnapInto>();
            }
            if (hoverObject != null)
            {
                hoverObject.returnPosition = hoverObject.originalPos;
                hoverObject.returnParent = hoverObject.originalParent;
                hoverObject = null;
            }
        }

        // Adds a contained object if the container is not filled. Returns whether
        // the object was added.
        public bool AddContainedObject(DragDropSnapInto dragObject)
        {
            if (!isFilled)
            {
                containedObject = dragObject;
                isFilled = true;
                blockRenderer.SetColor(dragObject.GetComponent<CanvasRenderer>().GetMaterial().color);
                if (OnBlockPlaced != null)
                {
                    OnBlockPlaced();
                }
                return true;
            }
            return false;
        }

        // Removes the contained object of this block.
        public void RemoveContainedObject()
        {
            blockRenderer.SetColor(defaultColor);
            isFilled = false;
            containedObject = null;
            if (OnBlockRemoved != null)
            {
                OnBlockRemoved();
            }
        }
    
    }

}