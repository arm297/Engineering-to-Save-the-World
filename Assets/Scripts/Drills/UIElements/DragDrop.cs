using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Drills {
    /**
     * Basic Drag drop behavior for a UI object. Highlights moused over objects and
     * moves selected objects.
     */
    public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler,
    IPointerEnterHandler, IPointerExitHandler {

        // ID for matching dragged object with blocks. 
        public int dragID;

        // Highlight color for moused over objects
        [SerializeField]
        private Color highlightColor;

        // Original location of the dragged object
        [HideInInspector]
        public Vector3 originalPosition;

        // Parent to return to.
        [HideInInspector]
        public Transform originalParent;

        // Position to place the object on drop.
        [HideInInspector]
        public Vector3 returnPosition;

        // Parent of the object on drop.
        [HideInInspector]
        public Transform returnParent;

        // Original color of dragged object
        protected Color defaultColor;

        // Material of this object, if any.
        protected Material dragMaterial = null;

        // Original material color of the dragged object, if any.
        protected Color materialColor;

        // Renderer of drag-dropped object
        protected CanvasRenderer objectRenderer;

        // Original sibling index of this dragged object
        private int defaultSiblingIndex;

        // Current object snapped into, or null is no containing object.
        [HideInInspector]
        public BlockContainer containingBlock;

        // Highlight moused over objects if not already dragging an object.
        public void OnPointerEnter(PointerEventData eventData) {
            if (eventData.pointerDrag == null) {
                objectRenderer.SetColor(highlightColor);
                objectRenderer.SetAlpha(1);
                if (dragMaterial != null) {
                    dragMaterial.color = highlightColor;
                }
            }
        }

        // Reset default color of object.
        public void OnPointerExit(PointerEventData eventData) {
            if (eventData.pointerDrag != gameObject) {
                objectRenderer.SetColor(defaultColor);
                objectRenderer.SetAlpha(1);
                if (dragMaterial != null) {
                    dragMaterial.color = materialColor;
                }
            }
        }

        // Begin dragging the object, and set sibling index.
        public void OnBeginDrag(PointerEventData eventData) {
            eventData.pointerDrag = gameObject;
            returnParent = originalParent;
            transform.SetAsLastSibling();
            if (containingBlock != null) {
                containingBlock.RemoveContainedObject();
                containingBlock = null;
            }
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData) {
            //Vector3 mousePos = eventData.position;
            //Vector3 newPos = Camera.main.ScreenToWorldPoint(mousePos);
            //newPos.z = transform.position.z;
            float oldZ = transform.position.z;
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = Camera.main.nearClipPlane;
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        }

        // Resets the color and sibling index of the dragged object, and sets
        // the new position and parent.
        public void OnEndDrag(PointerEventData eventData) {
            eventData.pointerDrag = null;
            if (eventData.pointerEnter != gameObject) {
                objectRenderer.SetColor(defaultColor);
                if (dragMaterial != null) {
                    dragMaterial.color = materialColor;
                }
            }
            transform.SetSiblingIndex(defaultSiblingIndex);

            // Set new location of dragged object.
            if (returnParent != originalParent) {
                containingBlock = returnParent.GetComponent<BlockContainer>();
                containingBlock.AddContainedObject(this);
            }
            transform.position = returnPosition;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        // Use this for initialization. This method should be overridden in any
        // subclass.
        public virtual void Start() {
            originalPosition = transform.position;
            returnPosition = transform.position;
            originalParent = transform.parent;
            objectRenderer = GetComponent<CanvasRenderer>();
            defaultColor = objectRenderer.GetColor();
            defaultSiblingIndex = transform.GetSiblingIndex();
            if ((dragMaterial = objectRenderer.GetMaterial()) != null) {
                materialColor = dragMaterial.color;
            }
        }
    }

}