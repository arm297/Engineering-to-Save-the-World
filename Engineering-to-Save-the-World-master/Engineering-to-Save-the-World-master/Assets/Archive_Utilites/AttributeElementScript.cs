using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeElementScript : MonoBehaviour {

	public InputField attributeElementName;
	public InputField attributeElementValue;
	public Toggle attributeElementIsActive;

	private int currentID;
	private Transform currentAttributeElement;

	void Start () {
		currentAttributeElement = transform;
	}

	public int getID() {
		return currentID;
	}

	public void setID(int _ID) {
		currentID = _ID;
	}

	public void setNewID() {
		bool isNew = true;
		int tempID = Random.Range (10000, 99999);
		while (true) {
			foreach (AttributeInfo attributeInfo in GameControllerScript.InstanceGameCornoller.playerInfo.playerAttributes) {
				if (attributeInfo.getID () == tempID) {
					isNew = false;
					break;
				}
			}
			if (!isNew) {
				tempID = Random.Range (10000, 99999);
			} else {
				break;
			}
		}
		currentID = tempID;
	}

	public string getName() {
		return attributeElementName.text;
	}

	public int getValue() {
		return int.Parse (attributeElementValue.text);
	}

	public bool getIsActive() {
		return attributeElementIsActive.isOn;
	}

	public void setInputField(string _name, int _value, bool isActive) {
		attributeElementName.text = _name;
		attributeElementValue.text = _value.ToString ();
		attributeElementIsActive.isOn = isActive;
	}

	public void pressAttributeDeleteButton() {
		currentAttributeElement = transform;

		(GameObject.Find ("GameObject_AttributesSheild").transform as RectTransform).sizeDelta = new Vector2 (200, (GameObject.FindGameObjectsWithTag("AttributeElement").Length - 1) * 25 + 5);

		Destroy (currentAttributeElement.gameObject);
	}
}