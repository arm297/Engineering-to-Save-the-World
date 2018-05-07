using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Drills {

/**
 * The base class for computing and displaying the drill score.
 *
 * Individual drills should override this class to compute the score at the end
 * of the drill.
 */
public abstract class ScoreHandler : MonoBehaviour {

		// The score of the drill.
		public float score { get; protected set; }
		
		// The text to display the final game status.
		public Text statusText;

		// The text to display score info. 
		public Text scoreText;

		// Computes and sets the score at the end of the drill.
		public abstract float ComputeScore();

		// Displays to info for the score.
		public abstract void DisplayScoreInfo();

		// Resets the score.
		public void ResetScore() {
			score = 0;
		}
	}
}