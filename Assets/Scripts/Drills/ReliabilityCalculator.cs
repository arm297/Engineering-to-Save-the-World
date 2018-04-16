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
        private List<ReliabilityNodes> nodes;

        // The labels for the reliability drill.
        private List<ReliabilityLabels> labels;

        // The mapping correct blocks for each of the nodes.
        private SortedDictionary<BlockContainer, int> correctBlocks;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }

}
