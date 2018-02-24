using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonCreate_NewNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public bool ableToMove = true;
	public string nodeAreaDropName;
	public Transform parentToReturnTo = null;
	public Transform parentToReturnToPre = null;
	public GameObject nodeArea;
	public GameObject nodeAreaOverwrap;
	public GameObject nodeConnection;

	private bool isButtonPressing = false;
	private int initialPos;
	private int nodeTrim;
	string currentNodeName;
	private List<string> nodeOverwrapList;
	private Transform currentNode;
	private Transform parentGroup;
	private GameObject node;
	private GameObject newNode;
	private GameObject newConnection;
	private GameObject nodeAreaGroup;
	private GameObject newNodeArea;
	private NodeInfo currentNodeInfo;
	private Connection newConnectionComponent;

	void Start () {
		initialPos = GameControllerScript.InstanceGameCornoller.initialPos;
		nodeTrim = GameControllerScript.InstanceGameCornoller.nodeTrim;
		currentNode = transform.parent.parent.parent;
		node = GameControllerScript.InstanceGameCornoller.node;
		parentGroup = GameObject.Find("NodeGroup").transform;
		nodeOverwrapList = new List<string> ();
		nodeAreaDropName = "";
		currentNodeInfo = null;
	}

	//Create Button
	public void createButtonPressed() {
		nodeOverwrapList = new List<string> ();

		string[] possibleNodePos;

		nodeAreaGroup = new GameObject ("nodeAreaGroup");
		possibleNodePos = validCreatePosition (currentNodeInfo);
		ableToMove = true;

		string[] possiblePosArr;
		string possiblePosName = "";
		string possiblePosCondition = "";
		int[] pos;
		Vector3 vec3Pos;
		foreach (string possiblePos in possibleNodePos) {
			possiblePosArr = possiblePos.Split (' ');
			possiblePosName = possiblePosArr [0] + " " + possiblePosArr [1] + " " + possiblePosArr [2];
			possiblePosCondition = possiblePosArr [3];
			pos = GenerateNewNode.InstanceGenerateNewNode.changeAbposName (possiblePosName);
			vec3Pos = new Vector3 (pos [0] * nodeTrim + initialPos, pos [1] * nodeTrim + initialPos, -60);

			if (possiblePosCondition == "new") {
				newNodeArea = Instantiate (nodeArea, vec3Pos, Quaternion.identity, nodeAreaGroup.transform) as GameObject;
				newNodeArea.name = possiblePos;
			} else {
				newNodeArea = Instantiate (nodeAreaOverwrap, vec3Pos, Quaternion.identity, nodeAreaGroup.transform) as GameObject;
				newNodeArea.name = possiblePos;

				nodeOverwrapList.Add (possiblePosName);

				Transform theNode = GameObject.Find (possiblePosName).transform;
				Transform theParent = ((newNodeArea.transform).Find ("Canvas")).Find ("Panel_EmptyArea");
				theNode.SetParent (theParent);
			}
		}
	}

	//Find valid position for a creating new node
	public static string[] validCreatePosition(NodeInfo _node) {
		List<string> possiblePos = new List<string> ();
		List<string> possiblePosF = new List<string> ();

		for (int i = _node.level - 2; i <= _node.level + 2; i++) {
			if (i > 0) {
				for (int j = -i; j <= i; j++) {
					possiblePos.Add (_node .direction + " " + i + " " + j);
				}
			}
		}

		for (int i = 0; i < possiblePos.Count; i++) {
			string[] possiblePosElement = possiblePos [i].Split (' ');
			NodeInfo nodeInfo = new NodeInfo (int.Parse(possiblePosElement[0]), int.Parse(possiblePosElement[1]), int.Parse(possiblePosElement[2]), possiblePos[i], false);
			if (!GenerateNewNode.InstanceGenerateNewNode.isOverwraped (nodeInfo)) {
				possiblePosF.Add (possiblePos [i] + " new");
			} else {
				possiblePosF.Add (possiblePos [i] + " overwrap");
			}
		}

		return possiblePosF.ToArray();
	}

	//Drag Function
	public void OnBeginDrag(PointerEventData eventData) {
		currentNode = transform.parent.parent.parent;

		currentNodeName = currentNode.name;

		for (int i = 0; i < GameControllerScript.InstanceGameCornoller.nodeList.Count; i++) {
			if (currentNodeName == GameControllerScript.InstanceGameCornoller.nodeList[i].name) {
				currentNodeInfo = GameControllerScript.InstanceGameCornoller.nodeList [i];
				break;
			}
		}

		nodeAreaDropName = "";
		isButtonPressing = true;
		newNode = Instantiate (node, currentNode.position, Quaternion.identity, parentToReturnTo) as GameObject;
		newNode.name = "NewNode";
		newNode.GetComponent<CanvasGroup> ().blocksRaycasts = false;

		newConnection = Instantiate(nodeConnection, new Vector3(0, 0, 0), Quaternion.identity, parentGroup) as GameObject;
		newConnection.name = currentNodeInfo.name + "<>" + newNode.name;

		int currentDirection = currentNodeInfo.direction;
		newConnectionComponent = newConnection.GetComponent<Connection> ();
		newConnectionComponent.SetTargets (currentNode.transform as RectTransform, newNode.transform as RectTransform);
		newConnectionComponent.SetPoints(currentDirection, (currentDirection + 2) % 4);

		createButtonPressed ();
	}
	public void OnDrag(PointerEventData eventData) {

		if (isButtonPressing) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = -50;
			if (ableToMove) {
				newNode.transform.position = pos;
			}
			if (parentToReturnTo != parentToReturnToPre) {
				newNode.transform.SetParent (parentToReturnTo);
				parentToReturnToPre = parentToReturnTo;
			}
		}
	}
	public void OnEndDrag(PointerEventData eventData) {
		isButtonPressing = false;
		newNode.GetComponent<CanvasGroup> ().blocksRaycasts = true;

		if (nodeAreaDropName != "") {
			string[] nameArr = nodeAreaDropName.Split(' ');
			string nodeName = nameArr [0] + " " + nameArr [1] + " " + nameArr [2];
			string nodeCondition = nameArr [3];

			newConnection.name = currentNodeInfo.name + "<>" + nodeName;

			if (nodeCondition == "new") {
				NodeInfo newNodeInfo = new NodeInfo (int.Parse (nameArr [0]), int.Parse (nameArr [1]), int.Parse (nameArr [2]), true);
				newNode.name = nodeName;
				GameControllerScript.InstanceGameCornoller.nodeList.Add (newNodeInfo);

				newNode.GetComponentInChildren<Canvas> ().GetComponentInChildren<InputField> ().text = newNodeInfo.minimumFund.ToString ();
				newNode.GetComponent<NewNodeScript> ().UpdateNodeInfo ();

				currentNodeInfo.addChildNodeName (nodeName);
				newNode.transform.SetParent (parentGroup);
			}
			if (nodeCondition == "overwrap") {
				if (!currentNodeInfo.childNodeName.Contains (nodeName)) {
					currentNodeInfo.addChildNodeName (nodeName);

					newConnectionComponent.SetTargets (currentNode.transform as RectTransform, GameObject.Find(nodeName).transform as RectTransform);
					newConnectionComponent.SetPoints(currentNodeInfo.direction, (currentNodeInfo.direction + 2) % 4);
				} else {
					Destroy (newConnection);
				}

				Destroy (newNode);
			}
		} else {
			Destroy (newConnection);
			Destroy (newNode);
		}

		foreach (string nodeOverwrapName in nodeOverwrapList) {
			
			((GameObject.Find (nodeOverwrapName)).transform).SetParent (parentGroup);
		}

		Destroy (nodeAreaGroup);
	}
}
