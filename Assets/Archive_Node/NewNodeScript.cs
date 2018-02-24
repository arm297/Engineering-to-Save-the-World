using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNodeScript : MonoBehaviour {

	private string name;
	private NodeInfo nodeInfo;

	void Start () {
		name = transform.name;
		foreach (NodeInfo globalNodeInfo in GameControllerScript.InstanceGameCornoller.GetNodeList()) {
			if (name == globalNodeInfo.name) {
				nodeInfo = globalNodeInfo;
				break;
			}
		}
	}

	public void UpdateNodeInfo() {
		name = transform.name;
		foreach (NodeInfo globalNodeInfo in GameControllerScript.InstanceGameCornoller.GetNodeList()) {
			if (name == globalNodeInfo.name) {
				nodeInfo = globalNodeInfo;
				Debug.Log (nodeInfo.name);
				break;
			}
		}
	}

	public NodeInfo getNodeInfo() {
		return nodeInfo;
	}
}