using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceLabelScript : MonoBehaviour {

	public Text nameResourceText;
	public InputField weightResouceInputField;
	public Text minValueResourceText;
	public Text maxValueResourceText;

	private AttributeInfo attributeInfo;
	private int weightValue;
	private int minValue;
	private int maxValue;

	void Start () {
		weightValue = 0;
		minValue = 0;
		maxValue = 0;

		weightResouceInputField.onValueChanged.AddListener (delegate {
			weightValue = int.Parse(weightResouceInputField.text);
			GameUtilityScript.InstanceGameUtility.UpdateScore();
		});
	}

	public int GetWeight() {
		return weightValue;
	}

	public int GetMinValue() {
		return minValue;
	}

	public int GetMaxValue() {
		return maxValue;
	}

	public void ChangeLabel(int _weight, int _min, int _max) {
		weightResouceInputField.text = _weight.ToString ();
		minValueResourceText.text = _min.ToString ();
		maxValueResourceText.text = _max.ToString ();
	}

	public void UpdateLabel(AttributeInfo _attributeInfo) {
		attributeInfo = _attributeInfo;

		weightValue = 0;
		minValue = 0;
		maxValue = 0;

		nameResourceText.text = attributeInfo.getName ();
		foreach (NodeInfo nodeInfo in GameControllerScript.InstanceGameCornoller.nodeList) {
			foreach (AttributeElementInfo attributeElementInfo in nodeInfo.attributeElementInfoList) {
				if (attributeInfo == attributeElementInfo.parentAttribute) {
					if (nodeInfo.isLaborSet) {
						if (nodeInfo.isTested) {
							minValue += attributeElementInfo.actualValue;
							maxValue += attributeElementInfo.actualValue;
						} else {
							maxValue += attributeElementInfo.maximumValue;
						}
					}
					break;
				}
			}
		}
		ChangeLabel (weightValue, minValue, maxValue);
	}
}