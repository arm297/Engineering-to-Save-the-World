using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drills {

/**
 * This class represents the individual nodes of the Reliability diagram.
 * This class is currently a skeleton and will be added to for any additional
 * required features.
 */
public class ReliabilityNode : BlockContainer {

	// The reliability label contained in this node, if any.
	[HideInInspector]
	public ReliabilityLabel containedLabel;

	// Setup for this node.
	public override void Start() {
		// Currently no additional class.
	}

	// Update is called once per frame.
	void Update() {
		if (IsFilled() && containedLabel == null) {
			containedLabel = (ReliabilityLabel)containedObject;
		}
		else if (IsEmpty() && containedLabel != null) {
			containedLabel = null;
		}
	}
}

}