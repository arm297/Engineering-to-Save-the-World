using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeElementForNodeScript : MonoBehaviour {

	public Text attributeElementForNodeName;
	public Text attributeElementForNodeMin;
	public Text attributeElementForNodeMax;
	public InputField attributeElementForNodeActual;
	public Toggle attributeElementForNodeIsActive;

	public void Start() {
		attributeElementForNodeActual.onValueChanged.AddListener (delegate {
			SetMinMaxValue ();
			GameUtilityScript.InstanceGameUtility.UpdateScore();
		});
	}

	public void setData(string name, int min, int actual, int max, bool isActive) {
		attributeElementForNodeName.text = name;
		attributeElementForNodeMin.text = min.ToString();
		attributeElementForNodeMax.text = max.ToString();
		attributeElementForNodeActual.text = actual.ToString();
		attributeElementForNodeIsActive.isOn = isActive;
	}

	public string getName() {
		return attributeElementForNodeName.text;
	}

	public int getActual() {
		return int.Parse (attributeElementForNodeActual.text);
	}

	public int getMin() {
		return int.Parse (attributeElementForNodeMin.text);
	}

	public int getMax() {
		return int.Parse (attributeElementForNodeMax.text);
	}

	public void SetMinMaxValue() {
		int actualValue = int.Parse (attributeElementForNodeActual.text);
		int minValue = 0, maxValue = 0;

		if (actualValue < 10) {
			minValue = Random.Range (0, actualValue);
		} else {
			minValue = Random.Range (actualValue - 10, actualValue);
		}

		maxValue = actualValue;

		attributeElementForNodeMin.text = minValue.ToString ();
		attributeElementForNodeMax.text = maxValue.ToString ();
	}
}
