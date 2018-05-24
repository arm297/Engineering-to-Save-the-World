using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Drills {

    /**
     * A dummy score handler for the template drill scene. To create a new
     * drill, this script should be replaced.
     */
    public class TemplateDrillScoreHandler : ScoreHandler {
        
        // Calculates dummy 0 score.
        public override float ComputeScore() {
            return 0f;
        }

        // Computes max score of 1 (Prevents possible division by 0 error in
        // dummy scene).
        public override float ComputeMaxScore() {
            return 1f;
        }

        // Displays info about the score. Since this is a placeholder script,
        // there is no real info to display.
        public override void DisplayScoreInfo() {
            statusText.text = "TODO: Replace scoring script with a custom one\n"
                + " and update the score handler field for drill controller";
            scoreText.text = "No real score for this demo script";
        }
    
    }
}
