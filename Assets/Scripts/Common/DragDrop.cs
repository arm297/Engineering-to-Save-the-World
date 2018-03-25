using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * Basic Drag drop behavior for a UI object. Highlights moused over objects and
 * moves selected objects.
 */
//namespace Drill {

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

        // Highlight color for moused over objects
        [SerializeField]
        protected Color highlightColor;

        // Original color of dragged object
        private Color defaultColor;

        // Material of this object, if any.
        private Material dragMaterial = null;

        // Original material color of the dragged object, if any.
        private Color materialColor;

        // Renderer of drag-dropped object
        protected CanvasRenderer objectRenderer;

        // Rectangle transform for this drag object
        protected RectTransform objectTransform;

        // Original sibling index of this dragged object
        private int defaultSiblingIndex;

        // Highlight moused over objects
        protected void OnMouseEnter() 
        {
            objectRenderer.SetColor(highlightColor);
            if (dragMaterial != null) {
                dragMaterial.color = highlightColor;
            }
        }

        // Reset default color of object.
        protected void OnMouseExit() 
        {
            objectRenderer.SetColor(defaultColor);
            if (dragMaterial != null) {
                dragMaterial.color = materialColor;
            }                                       
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            objectTransform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            mousePos.z = transform.position.z;
            transform.position = mousePos;
        }

        public void OnEndDrag(PointerEventData eventData) 
        {
            objectTransform.SetSiblingIndex(defaultSiblingIndex);
        }

        // Use this for initialization
        protected void Start() 
        {
            objectRenderer = GetComponent<CanvasRenderer>();
            objectTransform = gameObject.transform as RectTransform;
            defaultColor = objectRenderer.GetColor();
            defaultSiblingIndex = objectTransform.GetSiblingIndex();
            if ((dragMaterial = objectRenderer.GetMaterial()) != null) {
            materialColor = dragMaterial.color;
            }
	    }
    }
//}
