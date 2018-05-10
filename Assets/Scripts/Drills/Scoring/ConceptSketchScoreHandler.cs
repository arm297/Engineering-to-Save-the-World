using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Drills {

/**
 * This class computes the drill score for the concept
 * sketch game.
 */
public class ConceptSketchScoreHandler : ScoreHandler {

	// The containing blocks of the system diagram.
	private List<BlockContainer> blocks = new List<BlockContainer>();

	// The labels for the system diagram.
	private List<DragDrop> labels = new List<DragDrop>();

	// The connections for the system diagram.
	private List<Connection> connections = new List<Connection>();
	
	// Call this for initialization.
	void Start() {
		GameObject[] goBlocks = GameObject.FindGameObjectsWithTag("snap_into");
		GameObject[] goLabels = GameObject.FindGameObjectsWithTag("block");
		GameObject[] goConnections = GameObject.FindGameObjectsWithTag("line");
		foreach (GameObject block in goBlocks) {
			blocks.Add(block.GetComponent<BlockContainer>());
		}
		foreach (GameObject label in goLabels) {
			labels.Add(label.GetComponent<DragDrop>());
		}
		foreach (GameObject connection in goConnections) {
			connections.Add(connection.GetComponent<Connection>());
		}
	}

	// Computes the score for the concept sketch drill. 
	public override float ComputeScore() {
		foreach(BlockContainer container in blocks) {
			if (container.IsFilled()) {
				DragDrop label = container.containedObject;
				if (label.dragID == 1 && container.blockID == 1) {
					score++;
				}
			} 
		}
		return score;
	}

        // Computes the maximum possible score for the concept sketch drill.
        public override float ComputeMaxScore() {
            maxScore = 0;
            foreach (DragDrop label in labels) {
                if (label.dragID == 1) {
                    maxScore++;
                }
            }
            return maxScore;
        }

        // Display to final score for the concept sketch game.
        public override void DisplayScoreInfo() {
    	    statusText.text = "Game Over!";
            scoreText.text = "Final Score: " + score + "\nMax Score: " + maxScore;
        }
    }

}