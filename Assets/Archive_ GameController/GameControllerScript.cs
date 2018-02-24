using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
///
//Required for Event System
using UnityEngine.SceneManagement;
///

public class GameControllerScript : MonoBehaviour {

	public static GameControllerScript InstanceGameCornoller;
	public int initialPos;
	public int nodeTrim;
	public Text fundText;
	public Text fundTurnText;
	public Text laborText;
	public GameObject node;
	public GameObject centerNode;
	public GameObject nodeConnection;
	public GameObject labor;
	public List<NodeInfo> nodeList = new List<NodeInfo> ();
	public List<string> nodeNameList = new List<string> ();
	public PlayerInfo playerInfo;

	private NodeInfo currentNodeInfo;
	private Transform nodeParentGroup;
	private Transform laborParentGroup;
	private GameObject centerNewNode;
	private GameObject newNode;
	private GameObject newConnection;
	private GameObject newLabor;

	private double EventChance = .5;
	public List<string> drills_run;
	private string[] list_of_drills = {"reliability_cost_drill_level_1","reliability_cost_drill_level_2",
	"reliability_cost_drill_level_3","reliability_cost_drill_level_4","reliability_cost_drill_level_5",
	"concept_sketch_interface","drill_1","drill_2",
	"good_requirement_bad_requirement","technical_readiness_level"};

	public void Append_Drills(string scenename){
		drills_run.Add(scenename);
	}

	//Call whenever a chance for drill is supposed to occur
	public void Chance_For_Drill(){

		///
		//Event system tie-in
		//On random chance (each time a node is purchased) initiate event
		Debug.Log ("Chance for Event");
		int randomNumber = Random.Range(0, 100);
		Debug.Log(randomNumber);
		string scenename = "";
		if (randomNumber < EventChance * 100){
			Debug.Log("Event System Activated.");
			// Select which drill to run
			if (drills_run.Count == list_of_drills.Length){
				//then all drills have been run, select one at random
				randomNumber = Random.Range(0, list_of_drills.Length-1);
				scenename = list_of_drills[randomNumber];
			}else{
				//not all drills have been used. continue through list.
				scenename = list_of_drills[drills_run.Count];
			}
			Append_Drills(scenename);
			// Load New Scene
			// Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
			//Disable and hide persistant elements
			GameObject c = GameObject.Find("NodeGroup");
			CanvasGroup cg = c.GetComponent<CanvasGroup>();
			cg.alpha = 0f; //this makes everything transparent
     		cg.blocksRaycasts = false; //this prevents the UI element to receive input events			
			//c.SetActive(false);
			c = GameObject.Find("MainScene");
			cg = c.GetComponent<CanvasGroup>();
			cg.alpha = 0f; //this makes everything transparent
     		cg.blocksRaycasts = false; //this prevents the UI element to receive input events
			Camera.main.gameObject.active = false; // Disable camera (cannot have more than 2 audio listeners)
			
			SceneManager.LoadScene(scenename, LoadSceneMode.Additive);
		}
		///

	}

	//called from drills when time to return to main game
	//parameter success should be true if drill was a success, fail otherwise
	public void ReturnToMainGame(bool success){
		Debug.Log("controller..."+success);
		//Reward or Punish depending on bool
		if(success){
			playerInfo.fund = playerInfo.fund + 10;
		}else{
			playerInfo.fund = playerInfo.fund - 20;
		}
		
		//Update player print out
		preForNodeGeneration();

		//GameObject[] gos = GameObject.FindGameObjectsWithTag("timer");//"hide_me"); 
		//foreach (GameObject go in gos){
		//	CanvasGroup cg = go.GetComponent<CanvasGroup>();
		//	cg.alpha = 0f;
     	//	cg.blocksRaycasts = false;
			//Destroy(go);
		//}
	}

	void Start () {

		if(1==0){//game_started){

		}
		else{//if(!game_started){
		///
			InstanceGameCornoller = this;

			nodeParentGroup = GameObject.Find ("NodeGroup").transform;
			laborParentGroup = GameObject.Find ("LaborGroup").transform;



			if (nodeList.Count == 0) {
				//New Game Method
				Debug.Log ("Loading the New Game");

				//Create a New PlayerInfo
				playerInfo = new PlayerInfo (200, 0, 20);
				fundText.text = (playerInfo.fund).ToString();
				fundTurnText.text = (playerInfo.fundTurn).ToString ();
				laborText.text = (playerInfo.labor).ToString();

				///
				//Event System mods: Need playerInfo updated
				//playerInfo = GameObject
				updateFundAndLabor();
				///

				centerNewNode = Instantiate (centerNode, new Vector3 (0 + initialPos, 0 + initialPos, -50), Quaternion.identity, nodeParentGroup) as GameObject;
				centerNewNode.name = "4 0 0";

				NodeInfo centerNodeInfo = new NodeInfo (4, 0, 0, centerNewNode.name, false);
				centerNodeInfo.childNodeName = new List<string> {"0 1 0", "1 1 0", "2 1 0", "3 1 0"};
				nodeList.Add (centerNodeInfo);


				//New Game initicial four node
				for (int i = 0; i < 4; i++) {
					switch (i) {
					case 0:
						newNode = Instantiate (node, new Vector3 (0 + initialPos, nodeTrim + initialPos, -50), Quaternion.identity, nodeParentGroup) as GameObject;
						break;
					case 1:
						newNode = Instantiate (node, new Vector3 (nodeTrim + initialPos, 0 + initialPos, -50), Quaternion.identity, nodeParentGroup) as GameObject;
						break;
					case 2:
						newNode = Instantiate (node, new Vector3 (0 + initialPos, -(nodeTrim) + initialPos, -50), Quaternion.identity, nodeParentGroup) as GameObject;
						break;
					case 3:
						newNode = Instantiate (node, new Vector3 (-(nodeTrim) + initialPos, 0 + initialPos, -50), Quaternion.identity, nodeParentGroup) as GameObject;
						break;
					default:
						Debug.Log ("Fail To Generate");
						break;
					}
					newNode.name = i + " 1 0";

					currentNodeInfo = new NodeInfo (i, 1, 0, newNode.name, false);
					nodeList.Add (currentNodeInfo);
					newNode.GetComponentInChildren<Canvas> ().GetComponentInChildren<InputField> ().text = currentNodeInfo.minimumFund.ToString();

					newConnection = Instantiate(nodeConnection, new Vector3(0, 0, 0), Quaternion.identity, nodeParentGroup) as GameObject;
					newConnection.name = centerNodeInfo.name + "<>" + newNode.name;

					Connection newConnectionComponent = newConnection.GetComponent<Connection> ();
					newConnectionComponent.SetTargets (centerNewNode.transform as RectTransform, generateNode(newNode, true).transform as RectTransform);
					newConnectionComponent.SetPoints(i, (i + 2) % 4);
				}

				//newLabor = Instantiate (labor, new Vector3 (200, 200, -60), Quaternion.identity, laborParentGroup) as GameObject;
			} else {
				//Loading the Game Method
				Debug.Log ("Loading the Game");

				preForNodeGeneration ();

				generateNodeTree (true);
			}
		}
	}

	public int GetCurrentFund() {
		return playerInfo.fund;
	}

	public void SubstractFund(int _fund) {
		playerInfo.fund -= _fund;
	}

	public List<NodeInfo> GetNodeList() {
		return nodeList;
	}

	public void preForNodeGeneration() {
		fundText.text = (playerInfo.fund).ToString();
		fundTurnText.text = (playerInfo.fundTurn).ToString ();
		laborText.text = (playerInfo.labor).ToString();

		foreach (NodeInfo temp in nodeList) {
			nodeNameList.Add (temp.name);
		}

		NodeInfo nodeInfo = nodeList [0];
		int[] parentPos = GenerateNewNode.InstanceGenerateNewNode.changeAbpos (nodeInfo);
		Vector3 vec3ParentPos = new Vector3 (parentPos [0] * nodeTrim + initialPos, parentPos [1] * nodeTrim + initialPos, -50);
		newNode = Instantiate (centerNode, vec3ParentPos, Quaternion.identity, nodeParentGroup) as GameObject;
		newNode.name = nodeInfo.name;
		nodeNameList.Remove (nodeInfo.name);
	}

	public void generateNodeTree(bool isShowAll) {
		newNode = generateNode (newNode, isShowAll);
	}

	public void Reload() {

		///
		//Event System Management: When switching back to main game.
		Debug.Log("Reloading...");
		//		


		Start ();
	}

	public void updateFundAndLabor () {
		playerInfo.fund = int.Parse (fundText.text);
		playerInfo.fundTurn = int.Parse (fundTurnText.text);
		playerInfo.labor = int.Parse (laborText.text);
	}

	public void updateFundAndLaborGC() {
		fundText.text = (playerInfo.fund).ToString ();
		fundTurnText.text = (playerInfo.fundTurn).ToString ();
		laborText.text = (playerInfo.labor).ToString ();
	}

	public GameObject generateNode(GameObject _node, bool isShowAll) {
		NodeInfo nodeInfo = null;
		GameObject newNodeG = null;

		for (int i = 0; i < nodeList.Count; i++) {
			if (_node.name == nodeList [i].name) {
				nodeInfo = nodeList [i];
				break;
			}
		}

		List<string> childNodeNames = nodeInfo.childNodeName;
		NodeInfo childNodeInfo = null;
		if (childNodeNames != null) {
			for (int i = 0; i < childNodeNames.Count; i++) {
				for (int j = 0; j < nodeList.Count; j++) {
					if (childNodeNames [i] == nodeList [j].name) {
						childNodeInfo = nodeList [j];
						break;
					}
				}
				if ((!childNodeInfo.isHidden) || isShowAll) {
					if (!nodeNameList.Contains (childNodeNames [i])) {
						Debug.Log ("Already there");
						newConnection = Instantiate (nodeConnection, new Vector3 (0, 0, 0), Quaternion.identity, nodeParentGroup) as GameObject;
						newConnection.name = nodeInfo.name + "<>" + childNodeNames [i];

						int currentDirection = nodeInfo.direction;
						Connection newConnectionComponent = newConnection.GetComponent<Connection> ();
						newConnectionComponent.SetTargets (_node.transform as RectTransform, GameObject.Find (childNodeNames [i]).transform as RectTransform);
						newConnectionComponent.SetPoints (currentDirection, (currentDirection + 2) % 4);
					} else {
						int[] pos = GenerateNewNode.InstanceGenerateNewNode.changeAbpos (childNodeInfo);
						Vector3 vec3Pos = new Vector3 (pos [0] * nodeTrim + initialPos, pos [1] * nodeTrim + initialPos, -50);

						newNodeG = Instantiate (node, vec3Pos, Quaternion.identity, nodeParentGroup) as GameObject;
						newNodeG.name = childNodeInfo.name;

						newNodeG.GetComponent<GenerateNewNode> ().updateInfo ();
						nodeNameList.Remove (childNodeInfo.name);

						newConnection = Instantiate (nodeConnection, new Vector3 (0, 0, 0), Quaternion.identity, nodeParentGroup) as GameObject;
						newConnection.name = nodeInfo.name + "<>" + newNodeG.name;

						int currentDirection = childNodeInfo.direction;
						Connection newConnectionComponent = newConnection.GetComponent<Connection> ();
						newConnectionComponent.SetTargets (_node.transform as RectTransform, generateNode (newNodeG, isShowAll).transform as RectTransform);
						newConnectionComponent.SetPoints (currentDirection, (currentDirection + 2) % 4);

						if (childNodeInfo.isPurchased == true) {
							newConnectionComponent.setColor (Color.yellow, Color.yellow);
							if (childNodeInfo.isLaborSet == true) {
								newConnectionComponent.setColor (Color.red, Color.red);
							}
						}
					}
				}
			}
		}
		return _node;
	}
}
