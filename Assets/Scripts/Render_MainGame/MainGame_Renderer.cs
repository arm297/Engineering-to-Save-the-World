using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame_Renderer : MonoBehaviour {

	// Define Parameters
	public int CanvasHeight;
	public int CanvasWidth;
	public List<GameObject> Nodes = new List<GameObject>();  // List of instantiated Nodes
	public List<int> NodeIDX = new List<int>(); // index of Node, matching Nodes in length, idx for node
	public GameObject Node;  // Must drag and drop Node prefab onto Node in Unity
	public float X_Space;  // Spacing between nodes (horizontal)
	public float Y_Space;  // Spacing between nodes (vertical)

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
			print(RespawnNodes);
			RespawnNodes = false;
			GameObject.Find("GameControl").GetComponent<GameController>().NodeChange = false;
			// Delete Existing
			foreach(GameObject node in Nodes){
				Destroy(node);
			}
			Nodes = new List<GameObject>();
			NodeIDX = new List<int>();
			print("GetNodes");
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
}
