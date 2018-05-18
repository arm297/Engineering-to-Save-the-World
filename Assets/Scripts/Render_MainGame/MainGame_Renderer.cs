using UnityEditor;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainGame_Renderer : MonoBehaviour {

    // Define Parameters
    public int CanvasHeight;
    public int CanvasWidth;
    public List<GameObject> Nodes = new List<GameObject>();  // List of instantiated Nodes
    public List<int> NodeIDX = new List<int>(); // index of Node, matching Nodes in length, idx for node
    public GameObject Node;  // Must drag and drop Node prefab onto Node in Unity
    public GameObject conn; // Connection GameObject prefab
    public float X_Space;  // Spacing between nodes (horizontal)
    public float Y_Space;  // Spacing between nodes (vertical)
    public GameObject ProfileSettingPrefab; // Profile setting prefab
    public GameObject NodeBucket; // Canvas with Node Layer for Nodes to be generated into
    public GameObject NodeConnectionBucket; // Canvas with Node Layer for NodeConnections to be generated into
    public Material RequirementParentChild; // Material of connection between required parent and child
    public Material NonRequirementParentChild; // Material of connection between optional parent and child
    public List<GameObject> Lines = new List<GameObject>(); // List of instantiated lines between nodes
    public float LineWidth = 0.4f; // width of lines
    public Button EndTurn;
    public Button LoadMinigame;
    public Sprite PurchasedImage;
    public Sprite SystReqImage;
    public Sprite ObscuredImage;

    // Troubleshooting
    public bool DisplayAllNodes = false;
    public bool RespawnNodes = false;

    // List of potential node images
    // The size of NodeImages needs to match or exceed number of Feautures possible in node
    public Sprite[] NodeImages = new Sprite[5];

    private bool stat_purchase_added = false;
    // Use this for initialization
    private Vector3 pay_attention_location; // location of pay attention graphic

    void Start() {
        pay_attention_location = GameObject.Find("[Make to pay attention]").transform.localScale;
        HideUnHideNextTurnPayAttention(false);
        RespawnNodes = true;
        EndTurn.onClick.AddListener(EndTurnListener);
        LoadMinigame.onClick.AddListener(GameObject.Find("GameControl").GetComponent<GameController>().LoadMinigame);

        //EndTurn.onClick.AddListener(Update);
        //GetNodes();

        // Add Listeners for Purchase Stat buttons
        Dictionary<string, int> playerStats = GameController.Player.Stats;
        //for (int i = 0; i < playerStats.Count; i++)
        int i = 0;
        foreach (KeyValuePair<string, int> item in playerStats) {
            //todo: check if object is active or not instead of try/except
            try {
                string statName = item.Key;//playerStats.Keys.ElementAt(i);
                GameObject.Find("Purchase_Stat_" + (1 + i)).GetComponent<Button>().onClick.AddListener(
                delegate { GameObject.Find("GameControl").GetComponent<GameController>().PurchasePlayerStat(statName); }
                );
                stat_purchase_added = true;
            } catch (NullReferenceException e) {
                // Do nothing
            }
            i += 1;
        }
    }

    // Listeners
    public void EndTurnListener(){
    			HideUnHideNextTurnPayAttention(false);
    	GameObject.Find("GameControl").GetComponent<GameController>().CommitTurn();
    	Update();

    }

    // Update is called once per frame
    void Update() {

        //if listeners weren't added, add them now:
        if (!stat_purchase_added) {
            // Add Listeners for Purchase Stat buttons
            Dictionary<string, int> playerStats = GameController.Player.Stats;

            try {
                int i = 0;
                foreach (KeyValuePair<string, int> item in playerStats) {
                    string statName = item.Key;//playerStats.Keys.ElementAt(i);
                    GameObject.Find("Purchase_Stat_" + (1 + i)).GetComponent<Button>().onClick.AddListener(
                    delegate { GameObject.Find("GameControl").GetComponent<GameController>().PurchasePlayerStat(statName); }
                    );
                    i += 1;
                }
                stat_purchase_added = true;
            } catch (NullReferenceException e) {
                // Do nothing
            }
        }

        //todo: logic to only updateprofile when something interesting happens
        UpdateProfile();
		//try { UpdateSystemScoreDisplay(); } catch (NullReferenceException e) { }
		UpdateSystemScoreDisplay();

        try { UpdatePlayerStatDisplay(); } catch (NullReferenceException e) { }

        if (RespawnNodes || GameObject.Find("GameControl").GetComponent<GameController>().NodeChange) {
            RespawnNodes = false;
            GameObject.Find("GameControl").GetComponent<GameController>().NodeChange = false;
            // Delete Existing
            foreach (GameObject node in Nodes) {
                Destroy(node);
            }
            foreach (GameObject line in Lines) {
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
    void GetNodes() {
        // Get a temporary copy of the NodeList to read from
        GameController gc;
        GameObject temp_GC = GameObject.Find("GameControl");
        gc = temp_GC.GetComponent<GameController>();
        var temp = gc.NodeList;

        foreach (var node in temp) {
            int idx = node.IDX;
            if (DisplayAllNodes) {
                Nodes.Add(CreateNodeGameObject(node, idx));
                NodeIDX.Add(idx);
            } else {
                // Go through logic to determine if a node should be added to list
                // Only consider Nodes not already appended to list
                if (NodeIDX.IndexOf(idx) == -1 && node.Visible) {
                    Nodes.Add(CreateNodeGameObject(node, idx));
                    NodeIDX.Add(idx);
                    // Also add parents & children to Node List
                    foreach (var node2 in temp) {
                        if (node2.Children.IndexOf(idx) != -1) {
                            // Child
                            Nodes.Add(CreateNodeGameObject(node2, node2.IDX));
                            NodeIDX.Add(node2.IDX);
                        }
                        if (node2.RequiredParents.IndexOf(idx) != -1 || node2.Parents.IndexOf(idx) != -1) {
                            // Parent
                            Nodes.Add(CreateNodeGameObject(node2, node2.IDX));
                            NodeIDX.Add(node2.IDX);
                        }
                    }

                }
            }
        }
    }

    // Creates a node gameobject based on input parameters
    GameObject CreateNodeGameObject(NodeData node, int idx) {
        Canvas canvas = NodeBucket.GetComponent<Canvas>();

        int x = node.X;
        int y = node.Y;
        //todo: get other parameters from node & tie to node's visual/hover-over
        GameObject NodeGameObject = (GameObject)Instantiate(Node, new Vector3(x * X_Space, y * Y_Space, 0), Quaternion.identity);
        NodeGameObject.transform.SetParent(canvas.transform);
        NodeGameObject.GetComponent<NodeListener>().idx = idx;
        NodeGameObject.GetComponent<NodeListener>().InitializeNode();

        // If node is purchaseable, then set Purchase Button to active
        if (node.Purchaseable) {
            NodeGameObject.GetComponent<NodeListener>().purchase.interactable = true;
        } else {
            NodeGameObject.GetComponent<NodeListener>().purchase.interactable = false;
            NodeGameObject.GetComponent<NodeListener>().HidePurchaseButton();
        }
        //todo implement testing
        //TESTING IS CURRENTLY DISABLED
        bool test_disabled = true;
        // If node is purchased and not tested, then set Test Button to active
        if (!node.Tested && node.Purchased && !test_disabled) {
            NodeGameObject.GetComponent<NodeListener>().test.interactable = true;
        } else {
            NodeGameObject.GetComponent<NodeListener>().test.interactable = false;
            NodeGameObject.GetComponent<NodeListener>().HideTestButton();
        }

        if (node.SystReq) {
            NodeGameObject.GetComponent<Image>().sprite = SystReqImage;
        }
        if (node.Purchased) {
            NodeGameObject.GetComponent<Image>().sprite = PurchasedImage;
        }
        if (node.Obscured && !node.SystReq) {
            NodeGameObject.GetComponent<Image>().sprite = ObscuredImage;
        }

        //set node image (depends on first max feauture in node data)
        int maxFeautureIDX = 0;
        float maxFeauture = 0;
        int ticker = 0;
        foreach (float val in node.ParameterEstimated) {
            if (val > maxFeauture) {
                maxFeauture = val;
                maxFeautureIDX = ticker;
            }
            ticker += 1;
        }
        NodeGameObject.transform.Find("NodeTypeImage").gameObject.GetComponent<Image>().sprite = NodeImages[maxFeautureIDX];

        return NodeGameObject;
    }

    // Changes the profile setting, includes name, fund, and labor
    public void ProfileSettingChange() {
        GameObject ProfileBox = GameObject.Find("Profile");
        GameObject SettingButtonGO = GameObject.Find("SettingButton");

        if (SettingButtonGO.GetComponentInChildren<Text>().text == "Setting") { // Start profile setting
            GameObject ProfileSettingBox = (GameObject)Instantiate(ProfileSettingPrefab, ProfileBox.transform.position, Quaternion.identity, ProfileBox.transform);
            ProfileSettingBox.name = "ProfileSettingBox";
            SettingButtonGO.GetComponentInChildren<Text>().text = "Apply";
        } else if (SettingButtonGO.GetComponentInChildren<Text>().text == "Apply") { // Apply the profile setting
            GameObject ProfileSettingBox = GameObject.Find("ProfileSettingBox");
            GameObject.Find("ProfileNameText").GetComponent<Text>().text = GameObject.Find("ProfileNameInputField").GetComponent<InputField>().text;
            GameObject.Find("ProfileFundText").GetComponent<Text>().text = GameObject.Find("ProfileFundInputField").GetComponent<InputField>().text;
            GameObject.Find("ProfileLaborText").GetComponent<Text>().text = GameObject.Find("ProfileLaborInputField").GetComponent<InputField>().text;
            /* Changes directly on Player profile
			 * GameObject GameControlGO = GameObject.Find ("GameControl");
			 * GameControlGO.GetComponent<GameController> ().Player.Name = GameObject.Find ("ProfileNameInputField").GetComponent<InputField> ().text;
			 * GameControlGO.GetComponent<GameController> ().Player.Funds = float.Parse(GameObject.Find ("ProfileFundInputField").GetComponent<InputField> ().text);
			 * GameControlGO.GetComponent<GameController> ().Player.Labor = float.Parse(GameObject.Find ("ProfileLaborInputField").GetComponent<InputField> ().text);
			*/
            SettingButtonGO.GetComponentInChildren<Text>().text = "Setting";
            Destroy(ProfileSettingBox);
        } else { // Error Fix
            SettingButtonGO.GetComponentInChildren<Text>().text = "Setting";
        }
    }

    // Will go through GameObject Nodes and draw appropriate connectiosn
    // todo: Ensure that connections aren't written over eachother if more than one connection type exists between two nodes
    public void DrawConnections() {
        int i = 0;
        foreach (GameObject node2 in Nodes) {
            int idx = NodeIDX[i];
            List<int> requirements = GameObject.Find("GameControl").GetComponent<GameController>().NodeList[idx].RequiredParents;
            List<int> parents = GameObject.Find("GameControl").GetComponent<GameController>().NodeList[idx].Parents;
            List<int> children = GameObject.Find("GameControl").GetComponent<GameController>().NodeList[idx].Children;
            // Draw Requirements
            //todo: consider adding additional logic:
            //	if both parents are purchased then draw different lines
            //	else if two few requirements are purchased then draw different line
            //	or draw line to spot where non-visible requirement node resides for child


            Color chil = Color.white;
            foreach (int jdx in children) {
                int gameObject_idx = NodeIDX.IndexOf(jdx); //find the requirement in list of node gameobjects
                if (gameObject_idx >= 0) {
                    GameObject line = CreateNodeConnection(Nodes[gameObject_idx], node2, chil, 1);
                    Lines.Add(line);
                }
            }

            // Draw Parents
            Color nonReq = Color.blue;
            foreach (int jdx in parents) {
                int gameObject_idx = NodeIDX.IndexOf(jdx);
                if (gameObject_idx >= 0) {
                    GameObject line = CreateNodeConnection(Nodes[gameObject_idx], node2, nonReq, 5);
                    Lines.Add(line);
                }
            }

            Color req = Color.red;
            foreach (int jdx in requirements) {
                int gameObject_idx = NodeIDX.IndexOf(jdx); //find the requirement in list of node gameobjects
                if (gameObject_idx >= 0) {
                    GameObject line = CreateNodeConnection(Nodes[gameObject_idx], node2, req, 10);
                    Lines.Add(line);
                }
            }


            i += 1;
        }
    }

    // Takes the 2 node GameObjects
    // Draws a line between the nodes
    public GameObject CreateNodeConnection(GameObject Node1, GameObject Node2, Color col, int thickness) {
        Canvas canvas = NodeConnectionBucket.GetComponent<Canvas>();
        Vector3 startPos = Node1.transform.position;
        Vector3 endPos = Node2.transform.position;


        GameObject lineGameObject = (GameObject)Instantiate(conn, startPos, Quaternion.identity);
        lineGameObject.transform.SetParent(canvas.transform);
        LineRenderer lineRenderer = lineGameObject.GetComponent<LineRenderer>();//lineGameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(col, col);
        lineRenderer.widthMultiplier = LineWidth;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        lineRenderer.SetWidth(thickness, thickness);
        return lineGameObject;

    }

    // Called to update the Render_MainGame.Utility.Profile.(Fund&Labor)
    public void UpdateProfile() {
        GameObject.Find("ProfileFundText").GetComponent<Text>().text = "" + Mathf.Round(GameController.Player.Funds);
        GameObject.Find("ProfileLaborText").GetComponent<Text>().text = "" + Mathf.Round(GameController.Player.Labor);
        GameObject.Find("ProfileTurnText").GetComponent<Text>().text = "" + GameObject.Find("GameControl").GetComponent<GameController>().PastTurns.NumberOfTurns;
        //Debug.Log(GameObject.Find ("GameControl").GetComponent<GameController>().Player.Labor);
    }

    // Called to update display of player performance
    public void UpdateSystemScoreDisplay() {
        List<string> names = GameController.ParameterNames;
        List<float> est = GameObject.Find("GameControl").GetComponent<GameController>().SystemParameters;
        List<float> min = GameObject.Find("GameControl").GetComponent<GameController>().MinRequiredSystemParameters;

		// Needs to be fixed
        /*for (int i = 0; i < names.Count; i++) {
            GameObject.Find("SystemFeature_" + (1 + i)).GetComponent<Text>().text = Mathf.Round(est[i]) + " / " + Mathf.Round(min[i]);//+ "\t\t" + names[i];
        }*/

		float testedScore = 0, expectedScore = 0;
		testedScore = GameObject.Find ("GameControl").GetComponent<GameController> ().GetTestedScore ();
		expectedScore = GameObject.Find ("GameControl").GetComponent<GameController> ().GetExpectedScore ();

		UpdateTotalScoreDisplay (testedScore, expectedScore);
    }

    // Called to update Player Stat Display
    public void UpdatePlayerStatDisplay() {
        //Player Stats:
        Dictionary<string, int> playerStats = GameController.Player.Stats;
        //for (int i = 0; i < playerStats.Count; i++)
        int i = 0;
        foreach (KeyValuePair<string, int> item in playerStats) {
            string statName = item.Key;//playerStats.Keys.ElementAt(i);
            int statValue = item.Value;//playerStats[ statName ];
            float statCost = GameObject.Find("GameControl").GetComponent<GameController>().PurchaseStatCost(statName);
            GameObject.Find("Stat_" + (1 + i)).GetComponent<Text>().text = statValue + "\t(" + Mathf.Round(statCost) + ")\t" + statName;
            i += 1;
        }
    }

    // Hide/Unhide animation around next-turn button
    public void HideUnHideNextTurnPayAttention(bool activate) {
        if (!activate) {
            GameObject.Find("[Make to pay attention]").transform.localScale = new Vector3(0, 0, 0);
        } else {
            GameObject.Find("[Make to pay attention]").transform.localScale = pay_attention_location;
        }
    }

	// Called to update displacy of player current total score
	public void UpdateTotalScoreDisplay(float tested, float expected) {
		GameObject.Find ("TestedScoreText").GetComponent<Text>().text = tested.ToString();
		GameObject.Find ("ExpectingScoreText").GetComponent<Text>().text = expected.ToString();
	}
}
