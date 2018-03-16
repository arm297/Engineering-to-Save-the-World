using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateNewNode : MonoBehaviour {

	public static GenerateNewNode InstanceGenerateNewNode;
	public int initialPos;
	public InputField nodeInputField;
	public Transform button;
	public GameObject node;
	public GameObject nodeConnection;

	private int nodeTrim;
	private NodeInfo currentNodeInfo;
	private GameObject newNode;
	private	GameObject newConnection;
	private Transform parentGroup;

	void Start() {
		InstanceGenerateNewNode = this;

		nodeTrim = GameControllerScript.InstanceGameCornoller.nodeTrim;
		parentGroup = GameObject.Find("/NodeGroup").transform;
	}

	public void updateInfo() {
		foreach (NodeInfo tempNodeInfo in GameControllerScript.InstanceGameCornoller.GetNodeList()) {
			if (transform.name == tempNodeInfo.name) {
				currentNodeInfo = tempNodeInfo;
				break;
			}
		}

		if (currentNodeInfo.isLaborSet) {
			button.GetComponentInChildren<Text> ().text = "Working";
			button.GetComponent<Button> ().interactable = false;
			nodeInputField.text = currentNodeInfo.laborSet.ToString ();
			nodeInputField.interactable = false;
		} else if (currentNodeInfo.isPurchased) {
			button.GetComponentInChildren<Text> ().text = "Labor Set";
			button.GetComponent<Button> ().interactable = true;
			nodeInputField.text = "0";
			nodeInputField.interactable = true;
		} else {
			button.GetComponentInChildren<Text> ().text = "Purchase";
			button.GetComponent<Button> ().interactable = true;
			nodeInputField.text = currentNodeInfo.minimumFund.ToString();
			nodeInputField.interactable = false;
		}
	}

	public void Generate() {
		List<string> childNames = new List<string>();
		string[] currentNodeName = transform.name.Split(' ');

		//Button unpressable
		if (button.GetComponent<Button> ().IsInteractable () == true) {
			button.GetComponent<Button> ().interactable = false;
			//button.GetComponent<Renderer> ().material.color = Color.black;
		} else {
			button.GetComponent<Button> ().interactable = true;
			//button.GetComponent<Renderer> ().material.color = Color.white;
		}

		//Getting the current node info
		int currentDirection = int.Parse (currentNodeName [0]);
		int currentLevel = int.Parse (currentNodeName [1]);
		int currentOrder = int.Parse (currentNodeName [2]);

		//Making the random number of node
		int maxNode = 2 * (currentLevel + 1) + 1;
		int[] nodeExist = new int[Random.Range(1, maxNode + 1)];
		int element;

		//Checking independence order
		for(int i = 0; i < nodeExist.Length; i++) {
			element = Random.Range (-(currentLevel + 1), currentLevel + 2);
			for (int j = 0; j < i; j++) {
				if (element == nodeExist [j]) {
					i--;
					break;
				}
			}
			nodeExist [i] = element;
		}

		//Checking overwrapping
		for (int i = 0; i < nodeExist.Length; i++) {

			string name = currentDirection + " " + (currentLevel + 1) + " " + nodeExist [i];
			NodeInfo _node = new NodeInfo (currentDirection, currentLevel + 1, nodeExist [i], name, true);
				
			if (!isOverwraped (_node)) {
				Vector3 pos = new Vector3 ();
				switch (currentDirection) {
				case 0:
					pos = new Vector3 (nodeTrim * nodeExist [i] + initialPos, nodeTrim * (currentLevel + 1) + initialPos, -50);
					break;
				case 1:
					pos = new Vector3 (nodeTrim * (currentLevel + 1) + initialPos, nodeTrim * nodeExist [i] + initialPos, -50);
					break;
				case 2:
					pos = new Vector3 (-nodeTrim * nodeExist [i] + initialPos, -nodeTrim * (currentLevel + 1) + initialPos, -50);
					break;
				case 3:
					pos = new Vector3 (-nodeTrim * (currentLevel + 1) + initialPos, -nodeTrim * nodeExist [i] + initialPos, -50);
					break;
				default:
					break;
				}
				newNode = Instantiate (node, pos, Quaternion.identity, parentGroup) as GameObject;
				newNode.name = name;
				childNames.Add (name);
				////////////////////////////////////////////////////////////////////////////////////////////////
				newConnection = Instantiate (nodeConnection, new Vector3(0, 0, 0), Quaternion.identity, parentGroup) as GameObject;

				newConnection.name = name + " Connection";

				Connection nC = newConnection.GetComponent<Connection> ();
				nC.SetTargets (transform as RectTransform, newNode.transform as RectTransform);
				nC.SetPoints(currentDirection, (currentDirection + 2) % 4);
				////////////////////////////////////////////////////////////////////////////////////////////////
				GameControllerScript.InstanceGameCornoller.GetNodeList().Add (_node);
			}
		}

		for(int i = 0; i < GameControllerScript.InstanceGameCornoller.GetNodeList().Count; i++) {
			if (GameControllerScript.InstanceGameCornoller.GetNodeList() [i].name == transform.name) {
				GameControllerScript.InstanceGameCornoller.GetNodeList() [i].setChildNodeName (childNames);
				break;
			}
		}

		foreach (NodeInfo node in GameControllerScript.InstanceGameCornoller.GetNodeList()) {
			Debug.Log (node.ToString ());
		}
	}

	//Checking overrapping method
	public bool isOverwraped(NodeInfo _node) {
		foreach (NodeInfo node in GameControllerScript.InstanceGameCornoller.GetNodeList()) {
			if (isSamePos(_node, node))
				return true;
		}
		return false;
	}

	//Checking same node position method
	public bool isSamePos(NodeInfo node1, NodeInfo node2) {
		int[] node1Ab = changeAbpos (node1);
		int[] node2Ab = changeAbpos (node2);
		return node1Ab[0] == node2Ab[0] && node1Ab[1] == node2Ab[1];
	}

	//Changing to absolute position method
	public int[] changeAbpos(NodeInfo node) {
		switch (node.direction) {
		case 0:
			return new int[] { node.order, node.level };
		case 1:
			return new int[] { node.level, node.order };
		case 2:
			return new int[] { -node.order, -node.level };
		case 3:
			return new int[] { -node.level, -node.order };
		default:
			return new int[] {0, 0};
		}
	}

	//Changing to absolute position from node name method
	public int[] changeAbposName(string name) {
		string[] nameArr = name.Split (' ');

		switch (int.Parse(nameArr[0])) {
		case 0:
			return new int[] { int.Parse(nameArr[2]), int.Parse(nameArr[1]) };
		case 1:
			return new int[] { int.Parse(nameArr[1]), int.Parse(nameArr[2]) };
		case 2:
			return new int[] { -int.Parse(nameArr[2]), -int.Parse(nameArr[1]) };
		case 3:
			return new int[] { -int.Parse(nameArr[1]), -int.Parse(nameArr[2])};
		default:
			return new int[] {0, 0};
		}
	}
}