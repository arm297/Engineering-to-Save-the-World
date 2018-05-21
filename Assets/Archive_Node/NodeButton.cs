using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NodeButton : MonoBehaviour {

	public Transform button;
	public GameObject node;
	public GameObject nodeConnection;
	public InputField nodeInputField;

	private int initialPos;
	private int nodeTrim;
	private int direction;
	private string currentCondition;
	private string currentNodeName;
	private List<NodeInfo> childNodes;
	private NodeInfo currentNodeInfo;
	private Transform currentNode;
	private Transform nodeParentGroup;
	private GameObject newNode;
	private GameObject newConnection;
	GameObject[] currentConnectionGameObjectList;

	private string MESSAGE_NOTENOUGH = "Not enough labor to set";

	public void pressNodeButton() {

		//Chance for Drill / Event
		GameControllerScript.InstanceGameCornoller.Chance_For_Drill();


		currentNodeName = transform.name;
		foreach (NodeInfo tempNodeInfo in GameControllerScript.InstanceGameCornoller.nodeList) {
			if (currentNodeName == tempNodeInfo.name) {
				currentNodeInfo = tempNodeInfo;
				break;
			}
		}

		currentCondition = button.GetComponentInChildren<Text>().text;

		currentConnectionGameObjectList = GameObject.FindGameObjectsWithTag ("NodeConnection");

		if (currentCondition == "Purchase") {
			if (GameControllerScript.InstanceGameCornoller.playerInfo.fund >= currentNodeInfo.minimumFund) {
				GameControllerScript.InstanceGameCornoller.playerInfo.fund -= currentNodeInfo.minimumFund;
				currentNodeInfo.isPurchased = true;
				button.GetComponentInChildren<Text> ().text = "Labor Set";
				transform.GetComponentInChildren<InputField> ().text = "0";
				transform.GetComponentInChildren<InputField> ().interactable = true;

				foreach (GameObject currentConnectionGameObject in currentConnectionGameObjectList) {
					if (currentConnectionGameObject.name.Contains (currentNodeName)) {
						currentConnectionGameObject.GetComponent<Connection> ().setColor (Color.yellow, Color.yellow);
					}
				}

				GameControllerScript.InstanceGameCornoller.updateFundAndLaborGC ();
				GameUtilityScript.InstanceGameUtility.updateResourceElement ();
				GameUtilityScript.InstanceGameUtility.UpdateScore();
			} else {
				Debug.Log ("Not Enough Fund");
			}
		}

		if (currentCondition == "Labor Set") {
			nodeParentGroup = GameObject.Find ("NodeGroup").transform;
			initialPos = GameControllerScript.InstanceGameCornoller.initialPos;
			nodeTrim = GameControllerScript.InstanceGameCornoller.nodeTrim;
			if (GameControllerScript.InstanceGameCornoller.playerInfo.labor >= int.Parse (nodeInputField.text)) {
				GameControllerScript.InstanceGameCornoller.playerInfo.labor -= int.Parse (nodeInputField.text);
				currentNodeInfo.isLaborSet = true;
				currentNodeInfo.laborSet = int.Parse (nodeInputField.text);

				foreach (GameObject currentConnectionGameObject in currentConnectionGameObjectList) {
					if (currentConnectionGameObject.name.Contains (currentNodeName)) {
						currentConnectionGameObject.GetComponent<Connection> ().setColor (Color.red, Color.red);
					}
				}

				List<string> childNodeNames = currentNodeInfo.childNodeName;
				NodeInfo currentChildNodeInfo = null;
				foreach (string currentChildNodeName in childNodeNames) {
					foreach (NodeInfo tempNode in GameControllerScript.InstanceGameCornoller.GetNodeList()) {
						if (currentChildNodeName == tempNode.name) {
							currentChildNodeInfo = tempNode;
							break;
						}
					}

					if (currentChildNodeInfo.minimumLabor <= int.Parse (nodeInputField.text)) {
						currentChildNodeInfo.isHidden = false;
						if (!GameObject.Find (currentChildNodeName)) {
							int[] pos = GenerateNewNode.InstanceGenerateNewNode.changeAbpos (currentChildNodeInfo);
							Vector3 vec3Pos = new Vector3 (pos [0] * nodeTrim + initialPos, pos [1] * nodeTrim + initialPos, -50);
							newNode = Instantiate (node, vec3Pos, Quaternion.identity, nodeParentGroup) as GameObject;
							newNode.name = currentChildNodeInfo.name;
							newNode.GetComponent<GenerateNewNode> ().updateInfo();
						} else {
							newNode = GameObject.Find (currentChildNodeName);
						}

						newConnection = Instantiate(nodeConnection, new Vector3(0, 0, 0), Quaternion.identity, nodeParentGroup) as GameObject;
						newConnection.name = currentNodeInfo.name + "<>" + newNode.name;

						direction = currentNodeInfo.direction;

						Connection newConnectionComponent = newConnection.GetComponent<Connection> ();
						newConnectionComponent.SetTargets (transform as RectTransform, newNode.transform as RectTransform);
						newConnectionComponent.SetPoints(direction, (direction + 2) % 4);

						if (currentChildNodeInfo.isLaborSet) {
							newConnectionComponent.setColor (Color.red, Color.red);
						} else if (currentChildNodeInfo.isPurchased) {
							newConnectionComponent.setColor (Color.yellow, Color.yellow);
						}
					}
				}

				button.GetComponentInChildren<Text> ().text = "";
				GameControllerScript.InstanceGameCornoller.updateFundAndLaborGC ();
				GameUtilityScript.InstanceGameUtility.updateResourceElement ();
				GameUtilityScript.InstanceGameUtility.UpdateScore();
			} else {
				GameUtilityScript.InstanceGameUtility.MessageBoxUpdate (currentNodeName + " : " + MESSAGE_NOTENOUGH);
			}
		}

		currentCondition = button.GetComponentInChildren<Text> ().text;

		//Button unpressable
		if (currentCondition == "") {
			button.GetComponentInChildren<Text> ().text = "Working";
			button.GetComponent<Button> ().interactable = false;
			transform.GetComponentInChildren<InputField> ().interactable = false;
		}
		GameUtilityScript.InstanceGameUtility.testButtonUpdate ();
	}
}