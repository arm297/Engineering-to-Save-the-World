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

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler,
    IPointerEnterHandler, IPointerExitHandler {

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
        private CanvasRenderer objectRenderer;

        // Image component of the dragged object.
        private Image dragImage;

        // Rectangle transform for this drag object
        protected RectTransform objectTransform;

        // Original sibling index of this dragged object
        private int defaultSiblingIndex;

        // Highlight moused over objects
        public void OnPointerEnter(PointerEventData eventData) 
        {
        if (eventData.pointerDrag == null)
            {
                Debug.Log("Getting Image Color");
                objectRenderer.SetColor(highlightColor);
                objectRenderer.SetAlpha(1);
                if (dragMaterial != null)
                {
                    dragMaterial.color = highlightColor;
                }
            }
        }

        // Reset default color of object.
        public void OnPointerExit(PointerEventData eventData) 
        {
        if (eventData.pointerDrag != gameObject)
        {
            objectRenderer.SetColor(defaultColor);
            if (dragMaterial != null)
            {
                dragMaterial.color = materialColor;
            }
        }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            eventData.pointerDrag = gameObject;
            objectTransform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            mousePos.z = transform.position.z;
            transform.position = mousePos;
        }

        public void OnEndDrag(PointerEventData eventData) {
            eventData.pointerDrag = null;
            objectTransform.SetSiblingIndex(defaultSiblingIndex);
            if (eventData.pointerEnter != gameObject) {
                objectRenderer.SetColor(defaultColor);
                if (dragMaterial != null)
                {
                    dragMaterial.color = materialColor;
                }
            }
        }

        // Use this for initialization
        protected void Start() 
        {
            objectRenderer = GetComponent<CanvasRenderer>();
            dragImage = GetComponent<Image>();
            objectTransform = gameObject.transform as RectTransform;
            defaultColor = objectRenderer.GetColor();
            defaultSiblingIndex = objectTransform.GetSiblingIndex();
            if ((dragMaterial = objectRenderer.GetMaterial()) != null) {
            materialColor = dragMaterial.color;
            }
	    }
    }
//}
