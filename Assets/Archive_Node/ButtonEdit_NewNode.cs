using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEdit_NewNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public bool ableToMove = true;
	public string nodeAreaDropName;
	public Transform parentToReturnTo = null;
	public Transform parentToReturnToPre = null;
	public GameObject nodeArea;
	public GameObject nodeConnection;

	private bool isButtonPressing = false;
	private int initialPos;
	private int nodeTrim;
	private Transform currentNode;
	private Transform parentGroup;
	private GameObject nodeAreaGroup;
	private GameObject newNodeArea;
	private NodeInfo currentNodeInfo;
	private Vector3 originalPos;

	void Start () {
		initialPos = GameControllerScript.InstanceGameCornoller.initialPos;
		nodeTrim = GameControllerScript.InstanceGameCornoller.nodeTrim;
		currentNode = transform.parent.parent.parent;
		parentGroup = GameObject.Find("NodeGroup").transform;
		nodeAreaDropName = "";
		currentNodeInfo = null;

	}

	//Create Button
	public void editButtonPressed() {
		string[] possibleNodePos;

		currentNode = transform.parent.parent.parent;
		string currentNodeName = currentNode.name;

		for (int i = 0; i < GameControllerScript.InstanceGameCornoller.nodeList.Count; i++) {
			if (currentNodeName == GameControllerScript.InstanceGameCornoller.nodeList[i].name) {
				currentNodeInfo = GameControllerScript.InstanceGameCornoller.nodeList [i];
				break;
			}
		}
		nodeAreaGroup = new GameObject ("nodeAreaGroup");
		possibleNodePos = validCreatePosition (currentNodeInfo);
		ableToMove = true;

		foreach (string possiblePos in possibleNodePos) {
			int[] pos = GenerateNewNode.InstanceGenerateNewNode.changeAbposName (possiblePos);
			Vector3 vec3Pos = new Vector3 (pos [0] * nodeTrim + initialPos, pos [1] * nodeTrim + initialPos, -50);
			newNodeArea = Instantiate (nodeArea, vec3Pos, Quaternion.identity, nodeAreaGroup.transform) as GameObject;
			newNodeArea.name = possiblePos;
		}
	}

	//Find valid position for a creating new node
	public static string[] validCreatePosition(NodeInfo _node) {
		List<string> possiblePos = new List<string> ();
		List<string> possiblePosF = new List<string> ();

		for (int i = _node.level - 2; i <= _node.level + 2; i++) {
			if (i > 0) {
				for (int j = -i; j <= i; j++) {
					possiblePos.Add (_node.direction + " " + i + " " + j);
				}
			}
		}

		for (int i = 0; i < possiblePos.Count; i++) {
			string[] possiblePosElement = possiblePos [i].Split (' ');
			NodeInfo nodeInfo = new NodeInfo (int.Parse(possiblePosElement[0]), int.Parse(possiblePosElement[1]), int.Parse(possiblePosElement[2]), possiblePos[i], false);
			if (!GenerateNewNode.InstanceGenerateNewNode.isOverwraped (nodeInfo)) {
				possiblePosF.Add (possiblePos [i]);
			}
		}
		return possiblePosF.ToArray();
	}

	//Drag Function
	public void OnBeginDrag(PointerEventData eventData) {
		isButtonPressing = true;
		originalPos = currentNode.position;
		currentNode.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		nodeAreaDropName = "";
		editButtonPressed ();
	}
	public void OnDrag(PointerEventData eventData) {

		if (isButtonPressing) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = -50;
			if (ableToMove) {
				currentNode.position = pos;
			}
			if (parentToReturnTo != parentToReturnToPre) {
				currentNode.SetParent (parentToReturnTo);
				parentToReturnToPre = parentToReturnTo;
			}
		}
	}
	public void OnEndDrag(PointerEventData eventData) {
		isButtonPressing = false;
		currentNode.GetComponent<CanvasGroup> ().blocksRaycasts = true;

		if (nodeAreaDropName != "") {
			currentNode.name = nodeAreaDropName;

			//Edit on nodeList
			string originalCurrentNodeName = currentNodeInfo.name;
			string[] nameArr = nodeAreaDropName.Split(' ');
			List<string> currentNodeChild = currentNodeInfo.childNodeName;
			GameObject nodeGameObject = null;

			foreach (string currentNodeChildName in currentNodeChild) {
				nodeGameObject = GameObject.Find (originalCurrentNodeName + "<>" + currentNodeChildName);
				nodeGameObject.name = nodeAreaDropName + "<>" + currentNodeChildName;
			}

			currentNodeInfo.name = nodeAreaDropName;
			currentNodeInfo.direction = int.Parse (nameArr[0]);
			currentNodeInfo.level = int.Parse (nameArr [1]);
			currentNodeInfo.order = int.Parse (nameArr [2]);

			foreach (NodeInfo eachNode in GameControllerScript.InstanceGameCornoller.nodeList) {
				if(eachNode.childNodeName.Contains(originalCurrentNodeName)) {
					eachNode.changeChildNodeName (originalCurrentNodeName, nodeAreaDropName);
					nodeGameObject = GameObject.Find (eachNode.name + "<>" + originalCurrentNodeName);
					nodeGameObject.name = eachNode.name + "<>" + nodeAreaDropName;
				}
			}

			currentNode.SetParent (parentGroup);
		} else {
			currentNode.position = originalPos;
		}
		Destroy (nodeAreaGroup);
	}
}
