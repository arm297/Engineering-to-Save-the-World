using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Drills {

	/**
	 * This class handles the individual callouts of a connection.
	 */
    public class ConnectionCallout : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerClickHandler {

		// Highlight color for mouse over.
		[SerializeField]
		private Color highlightColor =  Color.red;

		// Highlight color for selection.
		[SerializeField]
		private Color selectedColor = Color.green;

		// Highlight color for possible callout.
		[SerializeField]
		private Color possibleColor = Color.cyan;

        // Handles mouse events for the callout.
        public delegate void MouseAction();

        // Method to call on mouse entry.
        public MouseAction OnMouseEnter;

		// Renderer of this Component.
		private CanvasRenderer calloutRenderer;

        // Whether this component has been selected.
        [HideInInspector]
        public bool isSelected { get; private set; }

		// The original color of this component.
		[SerializeField]
		private Color defaultColor = Color.grey;

        // Sets the color of the callout on pointer entry.
		public void OnPointerEnter(PointerEventData eventData) {
			if (!isSelected) {
				SetColor(highlightColor);
			}
            if (OnMouseEnter != null) {
                OnMouseEnter();
            }
        }

        // Resets the color of the callout on pointer exit.
		public void OnPointerExit(PointerEventData eventData) {
			if (!isSelected) {
				SetColor(defaultColor);
			}
		}

		public void OnPointerClick(PointerEventData eventData) {
			SetColor(isSelected ? defaultColor : selectedColor);
			isSelected = !isSelected;
        }

        // Use this for initialization
        void Start() {
            isSelected = false;
			SetColor(defaultColor);
        }

		void OnEnable() {
			SetColor(defaultColor);
		}

        private void SetColor(Color c) {
			calloutRenderer = GetComponent<CanvasRenderer>();
			calloutRenderer.SetColor(c);
			calloutRenderer.SetAlpha(1);
		}
        
    }

}