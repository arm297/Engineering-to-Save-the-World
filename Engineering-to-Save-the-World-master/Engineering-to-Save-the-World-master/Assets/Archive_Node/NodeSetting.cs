using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeSetting : MonoBehaviour {

	public InputField minimumFundInputField;
	public InputField minimumLaborInputField;
	public Toggle purchasedToggle;
	public Toggle finalNodeToggle;
	public Transform attributesGroup;
	public GameObject attributeElementForNode;

	private string currentNodeName;
	List<string> currentNodeChild;
	private NodeInfo currentNodeInfo;
	private Transform currentSetting;
	private Transform currentNode;
	private Transform currentNodeConnection;
	private Transform newAttributeElementForNode;

	private GameControllerScript gcs;

	void Start () {
		gcs = GameObject.Find("GameController").GetComponent<GameControllerScript>();
		currentSetting = transform;

		string[] nameArr = transform.name.Split (' ');
		currentNodeName = nameArr [0] + " " + nameArr [1] + " " + nameArr [2];

		currentNode = (GameObject.Find (currentNodeName)).transform;

		foreach (NodeInfo node in gcs.nodeList) {
			if (currentNodeName == node.name) {
				currentNodeInfo = node;
			}
		}

		//Presetting
		minimumFundInputField.text = (currentNodeInfo.minimumFund).ToString ();
		minimumLaborInputField.text = (currentNodeInfo.minimumLabor).ToString ();
		purchasedToggle.isOn = currentNodeInfo.isPurchased;
		finalNodeToggle.isOn = currentNodeInfo.isFinalNode;

		List<AttributeElementInfo> attributeElmentInfoList = currentNodeInfo.attributeElementInfoList;

		foreach (AttributeElementInfo attributeElementInfo in attributeElmentInfoList) {
			newAttributeElementForNode = Instantiate (attributeElementForNode.transform, new Vector3(0, 0, 0), Quaternion.identity, attributesGroup);
			newAttributeElementForNode.name = attributeElmentInfoList.IndexOf(attributeElementInfo) + " Attribute";
			newAttributeElementForNode.GetComponent<AttributeElementForNodeScript> ().setData (attributeElementInfo.parentAttribute.name, attributeElementInfo.minimumValue, attributeElementInfo.actualValue, attributeElementInfo.maximumValue, attributeElementInfo.parentAttribute.isActive);
		}
	}

	public void pressCloseButton() {
		Destroy (currentSetting.gameObject);
	}

	public void pressDeleteNodeButton() {
		currentNode = GameObject.Find (currentNodeName).transform;
		currentNodeChild = currentNodeInfo.childNodeName;

		if (currentNodeChild.Count != 0) {
			foreach (string currentNodeChildName in currentNodeChild) {
				Destroy(GameObject.Find (currentNodeName + "<>" + currentNodeChildName));
			}
		}

		foreach (NodeInfo node in gcs.nodeList) {
			if (node.childNodeName.Contains(currentNodeName)) {
				node.removeChildNodeName(currentNodeName);
				Destroy (GameObject.Find (node.name + "<>" + currentNodeName));
			}
		}

		gcs.nodeList.Remove (currentNodeInfo);

		Destroy (currentNode.gameObject);
		Destroy (currentSetting.gameObject);
	}

	public void pressApplyButton() {

		List<GameObject> attributeElementGameObjectList = new List<GameObject> (GameObject.FindGameObjectsWithTag("AttributeElementForNode"));

		currentNodeInfo.minimumFund = int.Parse (minimumFundInputField.text);
		currentNodeInfo.minimumLabor = int.Parse(minimumLaborInputField.text);
		currentNodeInfo.isPurchased = purchasedToggle.isOn;
		currentNodeInfo.isFinalNode = finalNodeToggle.isOn;

		currentNode.GetComponentInChildren<Canvas> ().GetComponentInChildren<InputField> ().text = currentNodeInfo.minimumFund.ToString ();

		foreach (AttributeElementInfo attributeElementInfo in currentNodeInfo.attributeElementInfoList) {
			foreach (GameObject attributeElementGameObject in attributeElementGameObjectList) {
				if (attributeElementInfo.parentAttribute.getName () == attributeElementGameObject.GetComponent<AttributeElementForNodeScript> ().getName()) {
					attributeElementInfo.actualValue = attributeElementGameObject.GetComponent<AttributeElementForNodeScript> ().getActual ();
					attributeElementInfo.minimumValue = attributeElementGameObject.GetComponent<AttributeElementForNodeScript> ().getMin ();
					attributeElementInfo.maximumValue = attributeElementGameObject.GetComponent<AttributeElementForNodeScript> ().getMax ();
					break;
				}
			}
		}

		GameUtilityScript.InstanceGameUtility.updateResourceElement ();
		GameUtilityScript.InstanceGameUtility.UpdateScore();

		Destroy (currentSetting.gameObject);
	}
}