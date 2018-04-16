using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drills {

/**
 * The base class for computing the drill's score.
 *
 * Individual drills should override this class to compute the score at the end
 * of the drill.
 */
public abstract class ScoreHandler : MonoBehaviour {

		// The score of the drill.
		public float score { get; private set; }

		// Computes and sets the score at the end of the drill.
		public abstract float ComputeScore();

		// Resets the score.
		public void ResetScore() {
			score = 0;
		}
	}
}