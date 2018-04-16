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

        // Individual segmemts of the drill.
        private List<GameObject> segments = new List<GameObject>();

        // Individual callouts of the drill.
        private List<GameObject> callouts = new List<GameObject>();


        public void OnPointerClick(PointerEventData eventData) {

        }

        public void OnPointerEnter(PointerEventData eventData) {

        }

        public void OnPointerExit(PointerEventData eventData) {

        }

        // Use this for initialization.
        void Start() {
            activeSegments = false;

            // Set the different elements of the connection.
            foreach (GameObject segment in transform) {
                segments.Add(segment);
                callouts.Add(segment.transform.GetChild(0).gameObject);
            }

            // Show connections when block is placed in endpoint.
            connectionEnd1.OnBlockPlaced += Enable;
            connectionEnd1.OnBlockRemoved += Disable;

            // Hide connection when no blocks at either endpoint.
            connectionEnd2.OnBlockPlaced += Enable;
            connectionEnd2.OnBlockRemoved += Disable;
            Disable();
        }

        // Deallocated resouces before destroying connection.
        private void OnDestroy() {
            connectionEnd1.OnBlockPlaced -= Enable;
            connectionEnd1.OnBlockRemoved -= Disable;
            connectionEnd2.OnBlockPlaced -= Enable;
            connectionEnd2.OnBlockRemoved -= Disable;
        }


        // Set the connections to active if it is currently inactive.
        public void Enable() {
            if (!activeSegments) {
                activeSegments = true;
                foreach(GameObject segment in segments) {
                    segment.SetActive(true);
                    callouts.SetActive(false);
                }
            }
        }

        // Disable to game object if both endpoints are not filled.
        public void Disable() {
            if (activeSegments && !connectionEnd1.isFilled 
                && !connectionEnd2.isFilled) {
                activeSegments = false;
                foreach(GameObject segment in segments)
                {
                    segment.SetActive(false);
                }
            }
        }
    }
}
