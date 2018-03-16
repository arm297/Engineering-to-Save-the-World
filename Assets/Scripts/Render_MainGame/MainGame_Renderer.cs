﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame_Renderer : MonoBehaviour {

	// Define Parameters
	public int CanvasHeight;
	public int CanvasWidth;
	public List<GameObject> Nodes = new List<GameObject>();  // List of instantiated Nodes
	public List<int> NodeIDX = new List<int>(); // index of Node, matching Nodes in length, idx for node
	public GameObject Node;  // Must drag and drop Node prefab onto Node in Unity
	public float X_Space;  // Spacing between nodes (horizontal)
	public float Y_Space;  // Spacing between nodes (vertical)
	public GameObject ProfileSettingPrefab; // Profile setting prefab

	// Troubleshooting
	public bool DisplayAllNodes = false;
	public bool RespawnNodes = false;

	// Use this for initialization
	void Start () {
		RespawnNodes = true;
		//GetNodes();
	}
	
	// Update is called once per frame
	void Update () {
		if(RespawnNodes || GameObject.Find("GameControl").GetComponent<GameController>().NodeChange){
			RespawnNodes = false;
			GameObject.Find("GameControl").GetComponent<GameController>().NodeChange = false;
			// Delete Existing
			foreach(GameObject node in Nodes){
				Destroy(node);
			}
			Nodes = new List<GameObject>();
			NodeIDX = new List<int>();
			GetNodes();
		}
		// Check for newly purchased nodes
		//GetNodes();
	}

	// Instantiate Nodes if visible
	void GetNodes(){
		// Get a temporary copy of the NodeList to read from
		GameController gc;
		GameObject temp_GC = GameObject.Find("GameControl");
		gc = temp_GC.GetComponent<GameController>();
		var temp = gc.NodeList;
		
		foreach (var node in temp){
			int idx = node.IDX;
			if(DisplayAllNodes){
				Nodes.Add(CreateNodeGameObject(node,idx));
				NodeIDX.Add(idx);
			}else{
				// Go through logic to determine if a node should be added to list
				// Only consider Nodes not already appended to list
				if(NodeIDX.IndexOf(idx) == -1){
					if(node.Visible){
						Nodes.Add(CreateNodeGameObject(node,idx));
						NodeIDX.Add(idx);
					}
				}
			}
		}
	}

	// Creates a node gameobject based on input parameters
	GameObject CreateNodeGameObject(GameController.NodeData node, int idx){
		Canvas canvas = gameObject.GetComponent<Canvas>();
		
		int x = node.X;
		int y = node.Y;
		//todo: get other parameters from node & tie to node's visual/hover-over
		GameObject NodeGameObject =  (GameObject)Instantiate(Node, new Vector3(x*X_Space, y*Y_Space, 0), Quaternion.identity);
		NodeGameObject.transform.SetParent(canvas.transform);
		NodeGameObject.GetComponent<NodeListener>().idx = idx;
		NodeGameObject.GetComponent<NodeListener>().InitializeNode();
		// If node is purchaseable, then set Purchase Button to active
		if (node.Purchaseable){
			NodeGameObject.GetComponent<NodeListener>().purchase.interactable = true;
		}else{
			NodeGameObject.GetComponent<NodeListener>().purchase.interactable = false;
			NodeGameObject.GetComponent<NodeListener>().HidePurchaseButton();
		}
		//todo implement testing
		//TESTING IS CURRENTLY DISABLED
		bool test_disabled = true;
		// If node is purchased and not tested, then set Test Button to active
		if (!node.Tested && node.Purchased && !test_disabled){
			NodeGameObject.GetComponent<NodeListener>().test.interactable = true;
		}else{
			NodeGameObject.GetComponent<NodeListener>().test.interactable = false;
			NodeGameObject.GetComponent<NodeListener>().HideTestButton();
		}
		return NodeGameObject;
	}

	// Changes the profile setting, includes name, fund, and labor
	public void ProfileSettingChange() {
		GameObject ProfileBox = GameObject.Find ("Profile");
		GameObject SettingButtonGO = GameObject.Find ("SettingButton");

		if (SettingButtonGO.GetComponentInChildren<Text> ().text == "Setting") { // Start profile setting
			GameObject ProfileSettingBox = (GameObject)Instantiate(ProfileSettingPrefab, ProfileBox.transform.position, Quaternion.identity, ProfileBox.transform);
			ProfileSettingBox.name = "ProfileSettingBox";
			SettingButtonGO.GetComponentInChildren<Text> ().text = "Apply";
		} else if (SettingButtonGO.GetComponentInChildren<Text> ().text == "Apply") { // Apply the profile setting
			GameObject ProfileSettingBox = GameObject.Find("ProfileSettingBox");
			GameObject.Find ("ProfileNameText").GetComponent<Text> ().text = GameObject.Find ("ProfileNameInputField").GetComponent<InputField> ().text;
			GameObject.Find ("ProfileFundText").GetComponent<Text> ().text = GameObject.Find ("ProfileFundInputField").GetComponent<InputField> ().text;
			GameObject.Find ("ProfileLaborText").GetComponent<Text> ().text = GameObject.Find ("ProfileLaborInputField").GetComponent<InputField> ().text;
			/* Changes directly on Player profile
			 * GameObject GameControlGO = GameObject.Find ("GameControl");
			 * GameControlGO.GetComponent<GameController> ().Player.Name = GameObject.Find ("ProfileNameInputField").GetComponent<InputField> ().text;
			 * GameControlGO.GetComponent<GameController> ().Player.Funds = float.Parse(GameObject.Find ("ProfileFundInputField").GetComponent<InputField> ().text);
			 * GameControlGO.GetComponent<GameController> ().Player.Labor = float.Parse(GameObject.Find ("ProfileLaborInputField").GetComponent<InputField> ().text);
			*/
			SettingButtonGO.GetComponentInChildren<Text> ().text = "Setting";
			Destroy (ProfileSettingBox);
		} else { // Error Fix
			SettingButtonGO.GetComponentInChildren<Text> ().text = "Setting";
		}
	}
}
