using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drills {

/**
 * This class computes the reliability score for the reliability drill.
 */
public class ReliabilityScoreHandler : ScoreHandler {

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

    // Total current reliability.
    private double reliability;

    // Total current cost.
    private double cost;

    // Target reliability.
    private double targetReliability = 0.97;

    // Target cost.
    private double targetCost = 15;

    // The error allowed before failure.
    private double permissibleError = 0.0001;

    // Whether the target reliability has been reached.
    private bool targetReached = false;

	// The nodes of the reliability diagram.
    private List<ReliabilityNode> nodes;

    // The labels for the reliability drill.
    private List<ReliabilityLabel> labels;

	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	// Computes the score for the reliability drill.
	public override float ComputeScore() {
		ComputeCost();
		ComputeReliability();
		return 1f;
	}

	// Computes the total cost of the reliability drill.
	private void ComputeCost() {
		cost = 0;
		foreach(ReliabilityNode node in nodes) {
			if (node.IsFilled()) {
				cost += node.containedLabel.cost;
			}
		}
	}

	// Computes the total reliability of the drill.
	private void ComputeReliability() {
		reliability = 0;
		List<double> nodeReliabilities =  new List<double>();
		foreach (ReliabilityNode node in nodes) {
			if (node.IsFilled()) {
				nodeReliabilities.Add(node.containedLabel.reliability);
			}
		}
	}
}

}