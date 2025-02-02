﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * Basic Class for connection creation, highlighting, and deletion.
 */

namespace Drills {
    class Connection : MonoBehaviour {
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

        // Whether the connection segments are active.
        private bool activeSegments;

        // Whether the connection callouts are active.
        private bool activeCallouts;

        // Whether this connection has been selected.
        [HideInInspector]
        public bool isSelected { get; protected set; }

        // Individual segmemts of the drill.
        private List<ConnectionSegment> segments =
                new List<ConnectionSegment>();

        // Individual callouts of the drill.
        private List<GameObject> callouts = new List<GameObject>();

        // Use this for initialization.
        void Start() {
            activeSegments = true;

            // Set the different elements of the connection.
            foreach (Transform child in transform) {
                ConnectionSegment segment = child.GetComponent<ConnectionSegment>();
                segments.Add(segment);

                segment.OnMouseClicked += ColorSelectedSegments;
                segment.OnMouseEnter += HiglightMousedSegments;
                segment.OnMouseExit += ResetSegmentColors;
                callouts.Add(child.GetChild(0).gameObject);
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

            foreach (ConnectionSegment segment in segments) {
                segment.OnMouseClicked -= ColorSelectedSegments;
                segment.OnMouseEnter -= HiglightMousedSegments;
                segment.OnMouseExit -= ResetSegmentColors;
            }
        }


        // Set the connections to active if it is currently inactive.
        public void Enable() {
            if (!activeSegments) {
                activeSegments = true;
                foreach (ConnectionSegment segment in segments) {
                    segment.gameObject.SetActive(true);
                }
                foreach (GameObject callout in callouts) {
                    callout.SetActive(false);
                }
            }
        }

        // Disable to game object if both endpoints are not filled.
        public void Disable() {
            if (activeSegments && connectionEnd1.IsEmpty()
                && connectionEnd2.IsEmpty()) {
                activeSegments = false;
                foreach (ConnectionSegment segment in segments) {
                    segment.gameObject.SetActive(false);
                }
            }
        }

        // Highlight the segments of the connection.
        public void HiglightMousedSegments() {
            foreach (ConnectionSegment segment in segments) {
                segment.SetColor(mousedOverColor);
            }
        }

        // Reset the color of the segments of the connection.
        public void ResetSegmentColors() {
            foreach (ConnectionSegment segment in segments) {
                segment.ResetColor();
            }
        }

        // Toggles the selected color of this connection and the callouts'
        // states.
        public void ColorSelectedSegments() {
            if (isSelected) {
                foreach (ConnectionSegment segment in segments) {
                    segment.ResetColor();
                    segment.SetCalloutActive(false);
                }
            } else {
                foreach (ConnectionSegment segment in segments) {
                    segment.SetColor(selectedColor);
                    segment.SetCalloutActive(true);
                }
            }
            isSelected = !isSelected;
        }
    }
}
