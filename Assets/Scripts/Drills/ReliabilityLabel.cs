using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Drills {

public class ReliabilityLabel : MonoBehaviour {

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
	private static readonly float lowCostThreshold = 1.1;

	// Reliability threshold to make labels high cost.
	private static readonly float midCostThreshold = 2.1;

	// Use this for initialization
	void Start () {
		Text labelText = transform.getChild(0).GetComponent<Text>();
		labelText += " " + reliability;
		CanvasRenderer renderer = GetComponent<CanvasRenderer>();
		if (reliability < lowCostThreshold) {
			CanvasRenderer.SetColor(cheapColor);
		}
		else if (reliability < midCostThreshold) {
			CanvasRenderer.SetColor(midColor);
		}
		else {
			CanvasRenderer.SetColor(highColor);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

}