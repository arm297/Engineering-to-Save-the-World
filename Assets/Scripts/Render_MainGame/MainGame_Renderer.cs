using UnityEditor;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainGame_Renderer : MonoBehaviour {

    // Define Parameters
    [Header("Canvas Settings")]
    public int CanvasHeight;
    public int CanvasWidth;

    [Header("Node Settings")]
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

    [Header("Display Settings")]
    public Button EndTurn;
    public Button LoadMinigame;
    public Sprite PurchasedImage;
    public Sprite SystReqImage;
    public Sprite ObscuredImage;
    public GameObject Victory_Panel;
    public GameObject Defeat_Panel;

	public InputField weight4;
	public InputField weight3;
	public InputField weight2;
	public InputField weight1;

    // List of potential node images
    // The size of NodeImages needs to match or exceed number of Feautures possible in node
    public Sprite[] NodeImages = new Sprite[5];

    // Variables to control minigame (a sub window within main game)
    // MG = MiniGame
    [Header("Minigame Settings")]
    public Button[] MG_feature_buttons = new Button[4];
    public GameObject[] MG_feature_animations = new GameObject[4];
    private Vector3[] MG_feature_animations_orig_loc = new Vector3[4];
    private string[] MG_button_conversion = {"MiniGameF1", "MiniGameF2", "MiniGameF3", "MiniGameF4"};
    private int MG_first_button = -1;
    private int MG_second_button = -1;
    public Button MG_request_to_compare;
    private Vector3 MG_request_to_compare_location;
    public Image MG_result;
    public Vector3 MG_result_location;
    public GameObject MG_panel;

    /*
    public string MG_first_button_name = "";
    public string MG_second_button_name = "";
    public int MG_buttons_selected = 0;
    private Vector3 MG_first_button_animation_location;
    private Vector3 MG_second_button_animation_location;
    public Button MG_request_to_compare;
    private Vector3 MG_request_to_compare_location;
    public List<Button> MG_buttons = new List<Button>();
    */

    // Troubleshooting
    [Header("Debug Settings")]
    public bool DisplayAllNodes = false;
    public bool RespawnNodes = false;

    private bool stat_purchase_added = false;
    // Use this for initialization
    private Vector3 pay_attention_location; // location of pay attention graphic

    void Start() {
        pay_attention_location = GameObject.Find("[Make to pay attention]").transform.localScale;
        HideUnHideNextTurnPayAttention(false);
        RespawnNodes = true;
        EndTurn.onClick.AddListener(EndTurnListener);
        //LoadMinigame.onClick.AddListener(GameObject.Find("GameControl").GetComponent<GameController>().LoadMinigame);

        int i = 0;
        foreach (Button b in MG_feature_buttons){
          b.onClick.AddListener(MG_selection);
          MG_feature_animations_orig_loc[i] = MG_feature_animations[i].transform.localScale;
          MG_feature_animations[i].transform.localScale = new Vector3(0, 0, 0);
          i += 1;
        }
        MG_request_to_compare.onClick.AddListener(MG_compare);
        MG_request_to_compare_location = MG_request_to_compare.transform.localScale;
        MG_request_to_compare.transform.localScale = new Vector3(0, 0, 0);
        MG_result_location = MG_result.transform.localScale;
        MG_result.transform.localScale = new Vector3(0, 0, 0);

        MG_panel.SetActive(false);

        // Add Listeners for Purchase Stat buttons
        Dictionary<string, int> playerStats = GameObject.Find("GameControl").GetComponent<GameController>().Player.Stats;

        //for (int i = 0; i < playerStats.Count; i++)
        i = 0;
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
		weight1.text = GameObject.Find ("GameControl").GetComponent<GameController> ().Player.ExpectedResourceCriterion [0].ToString();
		weight2.text = GameObject.Find ("GameControl").GetComponent<GameController> ().Player.ExpectedResourceCriterion [1].ToString();
		weight3.text = GameObject.Find ("GameControl").GetComponent<GameController> ().Player.ExpectedResourceCriterion [2].ToString();
		weight4.text = GameObject.Find ("GameControl").GetComponent<GameController> ().Player.ExpectedResourceCriterion [3].ToString();

    }

    // Listeners
    public void EndTurnListener(){
    			HideUnHideNextTurnPayAttention(false);
    	GameObject.Find("GameControl").GetComponent<GameController>().CommitTurn();

      // Check with GameControl to determine if game is still active or not
      // If game is over, display either victory or defeat panel
      if(GameObject.Find("GameControl").GetComponent<GameController>().GameOver){

        // Game has ended...Display victory or defeat panel
        if(GameObject.Find("GameControl").GetComponent<GameController>().GameVictory){
          Victory_Panel.SetActive(true);
        }else{
          Defeat_Panel.SetActive(true);
        }

        // Print Score Breakdown
        string score_breakdown = "Score Breakdown";

        score_breakdown += "\nTested Score:\t"+GameObject.Find ("GameControl").GetComponent<GameController> ().GetTestedScore ();
    		score_breakdown += "\nExpected Score:\t"+GameObject.Find ("GameControl").GetComponent<GameController> ().GetExpectedScore ();
        score_breakdown += "\nMinimum Features Met:\t"+GameObject.Find ("GameControl").GetComponent<GameController> ().SufficientFeatures();
        if (!GameObject.Find("GameControl").GetComponent<GameController>().GameVictory){
          if(GameObject.Find ("GameControl").GetComponent<GameController> ().SufficientFeatures()){
            score_breakdown += "Missing System Required Nodes!";
          }
        }

        GameObject.Find("Score_Breakdown").GetComponent<Text>().text = score_breakdown;

        // Return to Start Listener
        GameObject.Find("ReturnToStart").GetComponent<Button>().onClick.AddListener(ReturnToStart);

      }

      Update();

    }

    // Return user to start screen
    public void ReturnToStart(){
      SceneManager.LoadScene("Start_Screen", LoadSceneMode.Single);
    }

    // MG listeners
    // This function listens for feature buttons in minigame for comparison
    // It will put the selected feature into either MG_first_button or MG_second_button
    // And it will activate animations accordingly
    public void MG_selection(){

      // Get the identity of the button that was pressed
      string button_name = EventSystem.current.currentSelectedGameObject.name;
      int button_pressed = Array.IndexOf(MG_button_conversion, button_name);
      //Debug.Log(button_pressed);

      // Logic: If two buttons were already pressed, then replace first button with second
      // and replace second with first
      // else populate unpopulated button
      if(MG_first_button == button_pressed){
        MG_first_button = -1;
      }else if (MG_second_button == button_pressed){
        MG_second_button = -1;
      }else if (MG_first_button != -1 && MG_second_button != -1){
        MG_first_button = MG_second_button;
        MG_second_button = button_pressed;
      }else if (MG_first_button == -1){
        MG_first_button = button_pressed;
      }else{
        MG_second_button = button_pressed;
      }

      // reset all animations and turn on based off selected buttons
      for(int i = 0; i < 4; i++){
        MG_feature_animations[i].transform.localScale = new Vector3(0, 0, 0);
      }
      if (MG_first_button != -1){
        MG_feature_animations[MG_first_button].transform.localScale = MG_feature_animations_orig_loc[MG_first_button];
      }
      if (MG_second_button != -1){
        MG_feature_animations[MG_second_button].transform.localScale = MG_feature_animations_orig_loc[MG_second_button];
      }

      // If two items selected for compare, then activate compare tool
      // Logic: if two buttons are selected, then activate compare option
      if (MG_first_button != -1 && MG_second_button != -1){
        MG_request_to_compare.transform.localScale = MG_request_to_compare_location;
      }else{
        MG_request_to_compare.transform.localScale = new Vector3(0, 0, 0);
      }

    }

    public void MG_compare(){
      // check labor
      if (GameObject.Find("GameControl").GetComponent<GameController>().Player.Labor >=
          GameObject.Find("GameControl").GetComponent<GameController>().Player.CostToPlayMiniGame){
            GameObject.Find("GameControl").GetComponent<GameController>().Player.Labor -= GameObject.Find("GameControl").GetComponent<GameController>().Player.CostToPlayMiniGame;

            // of the two, which is most important
            float first_val = GameObject.Find("GameControl").GetComponent<GameController>().Player.ActualResourceCriterion[MG_first_button];
            float second_val = GameObject.Find("GameControl").GetComponent<GameController>().Player.ActualResourceCriterion[MG_second_button];
            Image val_image = MG_feature_buttons[MG_first_button].GetComponent<Image>();
            if(second_val > first_val){
              val_image = MG_feature_buttons[MG_second_button].GetComponent<Image>();
            }
            // display the more important feature
            MG_result.transform.localScale = MG_result_location;
            MG_result.sprite = val_image.sprite;
            // reset selections
            MG_first_button = -1;
            MG_second_button = -1;
        }else{
          //Insufficient Labor
          InsufficientLabor();
        }

    }

    // Call this method when insufficient funds are available
    void InsufficientLabor(){
      HideUnHideNextTurnPayAttention(true);
    }

    // Update is called once per frame
    void Update() {

        //if listeners weren't added, add them now:
        if (!stat_purchase_added) {
            // Add Listeners for Purchase Stat buttons
            Dictionary<string, int> playerStats = GameObject.Find("GameControl").GetComponent<GameController>().Player.Stats;

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

		GameObject.Find ("GameControl").GetComponent<GameController> ().Player.ExpectedResourceCriterion [0] = float.Parse (weight1.text);
		GameObject.Find ("GameControl").GetComponent<GameController> ().Player.ExpectedResourceCriterion [1] = float.Parse (weight2.text);
		GameObject.Find ("GameControl").GetComponent<GameController> ().Player.ExpectedResourceCriterion [2] = float.Parse (weight3.text);
		GameObject.Find ("GameControl").GetComponent<GameController> ().Player.ExpectedResourceCriterion [3] = float.Parse (weight4.text);


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
			if(!node.Testable && !node.Tested)
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
                    GameObject line = CreateNodeConnection(Nodes[gameObject_idx], node2, chil, 1, 5);
                    Lines.Add(line);
                }
            }

            // Draw Parents
            Color nonReq = Color.blue;
            foreach (int jdx in parents) {
                int gameObject_idx = NodeIDX.IndexOf(jdx);
                if (gameObject_idx >= 0) {
                    GameObject line = CreateNodeConnection(Nodes[gameObject_idx], node2, nonReq, 5, 1);
                    Lines.Add(line);
                }
            }

            Color req = Color.red;
            foreach (int jdx in requirements) {
                int gameObject_idx = NodeIDX.IndexOf(jdx); //find the requirement in list of node gameobjects
                if (gameObject_idx >= 0) {
                    GameObject line = CreateNodeConnection(Nodes[gameObject_idx], node2, req, 10, 1);
                    Lines.Add(line);
                }
            }


            i += 1;
        }
    }

    // Takes the 2 node GameObjects
    // Draws a line between the nodes
    public GameObject CreateNodeConnection(GameObject Node1, GameObject Node2, Color col, int thickness1, int thickness2) {
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
        lineRenderer.SetWidth(thickness1, thickness2);
        return lineGameObject;

    }

    // Called to update the Render_MainGame.Utility.Profile.(Fund&Labor)
    public void UpdateProfile() {
        GameObject.Find("ProfileFundText").GetComponent<Text>().text = "" + Mathf.Round(GameObject.Find("GameControl").GetComponent<GameController>().Player.Funds);
        GameObject.Find("ProfileLaborText").GetComponent<Text>().text = "" + Mathf.Round(GameObject.Find("GameControl").GetComponent<GameController>().Player.Labor);
        GameObject.Find("ProfileTurnText").GetComponent<Text>().text = "" + GameObject.Find("GameControl").GetComponent<GameController>().PastTurns.NumberOfTurns;
        //Debug.Log(GameObject.Find ("GameControl").GetComponent<GameController>().Player.Labor);
    }

    // Called to update display of player performance
    public void UpdateSystemScoreDisplay() {
        List<string> names = GameController.ParameterNames;
        List<float> est = GameObject.Find("GameControl").GetComponent<GameController>().SystemParameters;
        List<float> min = GameObject.Find("GameControl").GetComponent<GameController>().MinRequiredSystemParameters;

		// Needs to be fixed
        for (int i = 0; i < names.Count; i++) {
            GameObject.Find("SystemFeature_" + (1 + i)).GetComponent<Text>().text = Mathf.Round(est[i]) + " / " + Mathf.Round(min[i]);//+ "\t\t" + names[i];
        }

		float testedScore = 0, expectedScore = 0;
		testedScore = GameObject.Find ("GameControl").GetComponent<GameController> ().GetTestedScore ();
		expectedScore = GameObject.Find ("GameControl").GetComponent<GameController> ().GetExpectedScore ();

		UpdateTotalScoreDisplay (testedScore, expectedScore);
    }

    // Called to update Player Stat Display
    public void UpdatePlayerStatDisplay() {
        //Player Stats:
        Dictionary<string, int> playerStats = GameObject.Find("GameControl").GetComponent<GameController>().Player.Stats;
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
