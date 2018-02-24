using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ManualMode : MonoBehaviour {

	public GameObject nodeSetting;

	private Transform currentNode;
	private GameObject newNodeSetting;

	public void setButtonPress() {
		currentNode = transform;

		newNodeSetting = Instantiate (nodeSetting, new Vector3 (0, 0, -100), Quaternion.identity, null) as GameObject;
		newNodeSetting.name = currentNode.name;
	}
}