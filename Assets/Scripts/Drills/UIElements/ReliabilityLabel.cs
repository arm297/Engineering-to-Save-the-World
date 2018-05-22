using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Drills {

    /**
     * Class for the individual labels of the reliability nodes.
     * 
     * Handles the reliability and the cost of each individual label.
     */
    public class ReliabilityLabel : DragDrop {

        // Cost of dragging the label into the system.
        public double cost = 1.0;

        // Reliability of this label.
        public double reliability = 0.01;

        // Color of the cheap labels.
        private static readonly Color cheapColor = new Color(0, 1, 0, 1);

        // Color of the medium labels.
        private static readonly Color midColor = new Color(1, 0.92f, 0.016f, 1);

        // Color of the high cost labels.
        private static readonly Color highColor = new Color(1, 0, 0, 1);

        // Reliability threshold to make labels medium cost.
        private static readonly double lowCostThreshold = 1.1;

        // Reliability threshold to make labels high cost.
        private static readonly double midCostThreshold = 2.1;

        // Initializes the reliability labels during the start.
        public override void Start() {
            base.Start();
            Text labelText = transform.GetChild(0).GetComponent<Text>();
            labelText.text += "\n" + reliability;

            if (cost < lowCostThreshold) {
                materialColor = cheapColor;
            } else if (cost < midCostThreshold) {
                materialColor = midColor;
            } else {
                materialColor = highColor;
            }
            if (dragMaterial != null) {
                dragMaterial.color = materialColor;
            }
        }

    }

}