using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drills
{

    public class ReliabilityCalculator : MonoBehaviour
    {
        // Total reliability.
        private double reliability;

        // Target reliability.
        private double targetReliability - 0.97;

        // Target cost.
        private double targetCost = 15;

        // The error allowed before failure.
        private double permissibleError = 0.0001;

        // The reliability drill level.
        [SerializeField]
        private uint levelID;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
