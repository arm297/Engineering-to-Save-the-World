using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * Basic Class for connection creation, highlighting, and deletion.
 */

namespace Drills
{
    class Connection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        // Moused over highlight color for connections.
        [SerializeField]
        private Color mousedOverColor;

        // Selected highlight color for fields.
        [SerializeField]
        private Color selectedColor;

        // First endpoint of the connection.
        [SerializeField]
        private BlockContainer connectionEnd1;

        // Second endpoint of the connection.
        [SerializeField]
        private BlockContainer connectionEnd2;

        // Original color of the connection segments.
        private Color defaultColor;

        // Whether the connection segments are active.
        private bool activeSegments;

        private List<GameObject> segments = new List<GameObject>();
        private List<GameObject> callouts = new List<GameObject>();

        public void OnPointerClick(PointerEventData eventData) {

        }

        public void OnPointerEnter(PointerEventData eventData) {

        }

        public void OnPointerExit(PointerEventData eventData) {

        }

        void Start() {
            activeSegments = false;
            GameObject[] children;
        }


        public void Enable() {
            activeSegments = true;
           foreach(GameObject segment in segments)
            {

            }
        }

        public void Disable() {

        }

    }

}
