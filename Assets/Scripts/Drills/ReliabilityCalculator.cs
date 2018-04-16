using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drills
{

    public class ReliabilityCalculator : MonoBehaviour
    {
        // The reliability drill level.
        [SerializeField]
        private uint levelID;

        // The display to show the calculated reliability results.
        [SerializeField]
        private GameObject reliabilityDisplay;

        // The parent game object of the reliability nodes.
        [SerializeField]
        private GameObject nodesParent;

        // The parent game object for the reliability labels.
        [SerializeField]
        private GameObject labelsParent;

        // Total reliability.
        private double reliability;

        // Target reliability.
        private double targetReliability = 0.97;

        // Target cost.
        private double targetCost = 15;

        // The error allowed before failure.
        private double permissibleError = 0.0001;

        // Whether the target reliability has been reached.
        private bool targetReached = false;

        // The nodes of the reliability diagram.
        private List<GameObject> nodes;

        // The labels for the reliability drill.
        private List<GameObject> labels;

        // The mapping correct blocks for each of the nodes.
        private SortedDictionary<GameObject, GameObject> containingBlocks;

        // Use this for initialization
        void Start() {
            nodes = GetChildrenWithTag(nodesParent, "snap_into");
            labels = GetChildrenWithTag(labelsParent, "block");
            containingBlocks = new SortedDictionary<GameObject, GameObject>();
            foreach (GameObject node in nodes) {
                GameObject label = null;
                BlockContainer container = node.GetComponent<BlockContainer>();
                
            }
        }

        // Update is called once per frame
        void Update() {

        }

        // Get all children of a game object with a specified tag.
        public static List<GameObject> GetChildrenWithTag(GameObject parent,
                                                          string tag) {
            List<GameObject> taggedChildren = new List<GameObject>();
            foreach (Transform childTransform in parent.transform) {
                GameObject child = childTransform.gameObject;
                if (child.tag == tag) {
                    taggedChildren.Add(child);
                }
            }
            return taggedChildren;
        }
    }

}
