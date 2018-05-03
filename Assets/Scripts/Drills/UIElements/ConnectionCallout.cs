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

		// Renderer of this Component.
		private CanvasRenderer calloutRenderer;

		// Whether this component has been selected.
		private bool isSelected = false;

		// The original color of this component.
		[SerializeField]
		private Color defaultColor = Color.grey;

		public void OnPointerEnter(PointerEventData eventData) {
			if (!isSelected) {
				SetColor(highlightColor);
			}
        }

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