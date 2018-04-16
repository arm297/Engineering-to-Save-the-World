using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Drills {

/**
 * This class computes the score for any drill where
 * the labels are matched with containing blocks.
 */
public class LabelScoreHandler : ScoreHandler {

	// The labels for this assignment drill.
	private GameObject[] labels;
	
	// The container block for this assignment drill.
	private GameObject[] blocks;

	// The answer blocks for this assignment drill.
	private GameObject[] answers;

	// The incorrect blocks for this assignment drill.
	private List<GameObject> incorrect;

	// Use this for initialization
	void Start () {
		labels = GameObject.FindGameObjectsWithTag("label");
		blocks = GameObject.FindGameObjectsWithTag("block");
		answers = GameObject.FindGameObjectsWithTag("answer");
	}

	// Computes and updates the final score for the drill.
	public override float ComputeScore() {
		foreach(GameObject go in blocks) {
			BlockContainer container = go.GetComponent<BlockContainer>();
			DragDrop contained = container.containedObject;
			if (contained != null) {
				// These are the same elements.
				if (container.blockID == contained.dragID) {
					score++;
				}
				else {
					incorrect.Add(contained.gameObject);
				}
			}
		}
		return score;
	}
}

}