using System.Collections;
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
	public Material RequirementParentChild; // Material of connection between required parent and child
	public Material NonRequirementParentChild; // Material of connection between optional parent and child
	public List<GameObject> Lines = new List<GameObject>(); // List of instantiated lines between nodes
	public float LineWidth = 0.4f; // width of lines
	public Button EndTurn;
	public Sprite PurchasedImage;
	public Sprite SystReqImage;

	// Troubleshooting
	public bool DisplayAllNodes = false;
	public bool RespawnNodes = false;

	// Use this for initialization
	void Start () {
		RespawnNodes = true;
		EndTurn.onClick.AddListener(GameObject.Find("GameControl").GetComponent<GameController>().CommitTurn);
		//EndTurn.onClick.AddListener(Update);
		//GetNodes();
	}

	// Listeners
	public void EndTurnListener(){
		GameObject.Find("GameControl").GetComponent<GameController>().CommitTurn();
		Update();

	}

	// Update is called once per frame
	void Update () {

		//todo: logic to only updateprofile when something interesting happens
		UpdateProfile();
		UpdateSystemScoreDisplay();

		if(RespawnNodes || GameObject.Find("GameControl").GetComponent<GameController>().NodeChange){
			RespawnNodes = false;
			GameObject.Find("GameControl").GetComponent<GameController>().NodeChange = false;
			// Delete Existing
			foreach(GameObject node in Nodes){
				Destroy(node);
			}
			foreach(GameObject line in Lines){
				Destroy(line);
			}
			Nodes = new List<GameObject>();
			NodeIDX = new List<int>();
			Lines = new List<GameObject>();
			GetNodes();
			DrawConnections();
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

		if (node.SystReq){
			NodeGameObject.GetComponent<Image>().sprite = SystReqImage;
		}
		if (node.Purchased){
			NodeGameObject.GetComponent<Image>().sprite = PurchasedImage; 
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
	// Will go through GameObject Nodes and draw appropriate connectiosn
	public void DrawConnections(){
		int i = 0;
		foreach(GameObject node2 in Nodes){
			int idx = NodeIDX[i];
			List<int> requirements = GameObject.Find("GameControl").GetComponent<GameController>().NodeList[idx].RequiredParents;
			List<int> parents = GameObject.Find("GameControl").GetComponent<GameController>().NodeList[idx].Parents;
			// Draw Requirements
			//todo: consider adding additional logic:
			//	if both parents are purchased then draw different lines
			//	else if two few requirements are purchased then draw different line
			//	or draw line to spot where non-visible requirement node resides for child
			foreach(int jdx in requirements){
				int gameObject_idx = NodeIDX.IndexOf(jdx); //find the requirement in list of node gameobjects
				if (gameObject_idx >= 0){
					GameObject line = CreateNodeConnection(Nodes[gameObject_idx], node2, RequirementParentChild);
					Lines.Add(line);
				}
			}
			// Draw Parents
			foreach(int jdx in parents){
				int gameObject_idx = NodeIDX.IndexOf(jdx);
				if (gameObject_idx >= 0){
					GameObject line = CreateNodeConnection(Nodes[gameObject_idx], node2, NonRequirementParentChild);
					Lines.Add(line);
				}
			}

			i += 1;
		}
	}

	// Takes the 2 node GameObjects
	// Draws a line between the nodes
	public GameObject CreateNodeConnection(GameObject Node1, GameObject Node2, Material mat){
		Canvas canvas = gameObject.GetComponent<Canvas>();
		Vector3 startPos = Node1.transform.position;
		Vector3 endPos = Node2.transform.position;
		GameObject lineGameObject =  (GameObject)Instantiate(Node, startPos, Quaternion.identity);
		//lineGameObject.transform.SetParent(Node1.transform);
		LineRenderer lineRenderer = lineGameObject.AddComponent<LineRenderer>();
		lineRenderer.material = mat;
		lineRenderer.widthMultiplier = LineWidth;
		lineRenderer.SetPosition(0,startPos);
		lineRenderer.SetPosition(1,endPos);
		return lineGameObject;

	}

	// Called to update the Render_MainGame.Utility.Profile.(Fund&Labor)
	public void UpdateProfile(){
		GameObject.Find ("ProfileFundText").GetComponent<Text> ().text = ""+GameObject.Find ("GameControl").GetComponent<GameController>().Player.Funds;
		GameObject.Find ("ProfileLaborText").GetComponent<Text> ().text = ""+GameObject.Find ("GameControl").GetComponent<GameController>().Player.Labor;
		GameObject.Find ("ProfileTurnText").GetComponent<Text> ().text = ""+GameObject.Find ("GameControl").GetComponent<GameController>().PastTurns.NumberOfTurns;
		//Debug.Log(GameObject.Find ("GameControl").GetComponent<GameController>().Player.Labor);
	}

	// Called to update display of player performance
	public void UpdateSystemScoreDisplay(){
		List<string> names = GameObject.Find ("GameControl").GetComponent<GameController>().ParameterNames;
		List<float> values = GameObject.Find ("GameControl").GetComponent<GameController>().SystemParameters;

		//Debug.Log(values);
		GameObject.Find ("SystemFeatures").GetComponent<Text> ().text = "System Feautures:";
		for(int i=0; i < names.Count; i++){
			GameObject.Find ("SystemFeatures").GetComponent<Text> ().text += "\n" + names[i] + ":\t" + values[i];
		}
			GameObject.Find ("SystemFeatures").GetComponent<Text> ().text += "\n\n Minimum Requirements:";
			values = GameObject.Find ("GameControl").GetComponent<GameController>().MinRequiredSystemParameters;
			for(int i=0; i < names.Count; i++){
				GameObject.Find ("SystemFeatures").GetComponent<Text> ().text += "\n" + names[i] + ":\t" + values[i];
			}
	}
}
