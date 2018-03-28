/*
Description:
This class controls scene and event management and stores Node & Player data.
This is the only script to be sustained across scenes.
Author: Brighid Meredith 15:01 2/16/2018
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {

	///////////////////////////////////
	// PUBLIC PARAMTERS

	public List<NodeData> NodeList = new List<NodeData>();
	public PlayerProfile Player = new PlayerProfile();
	public TurnData PastTurns = new TurnData();
	public int Height = 100;
	public int Width = 100;
	public float InitialFunds = 1000;
	public float InitialLabor = 20;
	public float InitialFame = 0;
	public float EventChance = 0.5f;
	public bool NodeChange = false; // switches to true when a node is changed. Responsibility belongs to calling function.
	public float MaxEuclideanDistance = 3.0f; // maximum euclidean distance between parent and child node
	public float ParentChance = 0.5f; // chance that an existing node within distance of new node is a parent nodes
	public float RequirementsToParent = .2f; // chance that a parent is a requirement to purchase new node
	public float ExpectedUntestedNodeReliability = 0.98f; // 1 = no penalty, 0 = ultimate penalty
	public float InitialLaborPerTurn = 20.0f;  // Amount of Fund Change that occurs each turn (are funds renewed or depleted?)
	public int MaxNumberOfTurns = 10;  // Number of Turns allowed in game


	// LOADABLE SCENES
	// List of Event-Drill Scenes (all of which may be loaded)
	private string[] list_of_drills = {
		"reliability_cost_drill_level_1",
		"reliability_cost_drill_level_2",
		"reliability_cost_drill_level_3",
		"reliability_cost_drill_level_4",
		"reliability_cost_drill_level_5",
		"concept_sketch_interface",
		"drill_1",
		"drill_2",
		"good_requirement_bad_requirement",
		"technical_readiness_level"};
	private string MainGame = "MainGame";
	private string UI_Menu = "UI_Menu";

	///////////////////////////////////
	// DATA STRUCTURES

	//The below class stores Node Data. It is callable through GameObject.
    public class NodeData
    {
        public int IDX { get; set; }
				public string Name { get; set; }
				public int X { get; set; }
				public int Y { get; set; }
				public float CostActual { get; set; }
				public float CostEstimated { get; set; }
				public List<float> ParameterActuals { get; set; }
				public List<float> ParameterEstimated { get; set; }
				public List<string> ParameterNames { get; set; }
				public bool Purchased { get; set; }
				public bool Visible { get; set; }
				public bool Obscured {get; set; }
				public bool Purchaseable { get; set; }
				public bool Tested { get; set; }
				public bool Broken { get; set; }
				public float CostToFix { get; set; }
				public List<int> Parents {get; set; }
				public List<int> RequiredParents {get; set; }
				public List<int> Children {get; set; }
				public float ProbabilityToFail {get; set; }
				public float ParentExpectedReliability {get; set; }
				public float LaborCost { get; set; }
    }

	// the below class stores turn data as well as refreshable resources.
		public class TurnData
		{
			public float NumberOfTurns { get; set;}
			public float LaborPerTurn { get; set;}
			public float FundChangePerTurn { get; set;}
			public List<int> CurrentTurnNodesBought {get; set;}
			public List<int> CurrentTurnNodesTested {get; set;}
			//public List<int> CurrentTurnEventsOccurred {get; set;}
			public List<List<int>> NodesBoughtByTurn { get; set;}
			public List<List<int>> NodesTestedByTurn { get; set;}
			//public List<List<string>> EventsOccurredByTurn {get; set;}
		}

	//Stores player data
	//todo: Display Name, Title, Fame in game
	//todo: Allow player to set Name
	//todo: Award Title and Fame for succesful Events
	//todo: Base level of opportunity on Fame And/Or Title
	public class PlayerProfile
	{
		public float Funds { get; set; }
		public float Labor { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public float Fame { get; set; }
	}

	///////////////////////////////////////
	// INITIALIZATION

	// Use this for initialization
	void Start () {
		// Call Node Initialization
		InitializeNodeList();
		InitializePlayerProfile();
		InitializeTurnData();
		LoadScene(MainGame);
	}

	///////////////////////////////////////
	// SCENE MANAGEMENT

	// Method takes a scene name and attempts to load, clearing everything but GameController
	void LoadScene(string scene_name){
		SceneManager.LoadScene(scene_name, LoadSceneMode.Single);
	}

	//////////////////////////////////////////
	// METHODS CONTROLLING DATA STRUCTURES

	// Method InitializeNodeList is called once at start of game
	// Creates nodes and populates list
	// Sets handful of nodes as Purchaseable and Visible
	void InitializeNodeList() {
		//todo: Add additional knobs to control game initiation (as opposed to manual entries).
		// Parameters used in generation of node

		float BaseCost = 10;
		float BaseLabor = 5;

		// Populate Grid (X wide and Y deep)
		int X = Width;
		int Y = Height;

		int n_starting_purchaseable = 5;  // Initial number of visible nodes at start of game

		// Loop through horizontal
		int nodeIndex = 0; // index
		for (int i = 0; i <= X; i++)
        {
			// Loop through vertical
			for (int j = 0; j <= Y; j++)
			{
				NodeData n = new NodeData();
				n.IDX = nodeIndex;
				nodeIndex += 1;
				n.Name = "node "+n.IDX;
				n.X = i;
				n.Y = j;
				n.CostActual = BaseCost * Random.Range(0.8f, 1.5f);
				n.CostEstimated = n.CostActual * Random.Range(0.5f,1.1f);
				n.ParameterActuals = new List<float>{
					Random.Range(0.0f,5.0f),
					Random.Range(0.0f,5.0f),
					Random.Range(0.0f,5.0f),
					Random.Range(0.0f,5.0f)};
				n.ParameterEstimated = new List<float>{
					n.ParameterActuals[0] * Random.Range(.95f,1.3f),
					n.ParameterActuals[0] * Random.Range(.95f,1.3f),
					n.ParameterActuals[0] * Random.Range(.95f,1.3f),
					n.ParameterActuals[0] * Random.Range(.95f,1.3f)
				};
				n.ParameterNames = new List<string>{
					"Parameter A",
					"Parameter B",
					"Parameter C",
					"Parameter D"
				};
				n.Purchased = false;
				n.Visible = false;
				n.Purchaseable = false;
				n.Tested = false;
				n.Broken = false;
				n.CostToFix = n.CostActual * Random.Range(.2f,.7f);
				n.Parents = new List<int>();
				n.RequiredParents = new List<int>();
				n.Children = new List<int>();
				n.ProbabilityToFail = Random.Range(.01f,.3f);
				n.Obscured = true;
				n.ParentExpectedReliability = 1;
				n.LaborCost = BaseLabor * Random.Range(0.8f, 1.5f);

				// Identify Parent, RequiredParents, Children
				int npidx = 0;
				foreach(NodeData _ in NodeList){
						NodeData np = NodeList[npidx];
						float y_dist = n.Y - np.Y;
						float x_dist = n.X - np.X;
						float distance = Mathf.Sqrt(y_dist*y_dist + x_dist*x_dist);
						// Node within distance of new node
						if(distance <= MaxEuclideanDistance){
							// Node is a parent of new node
							if (ParentChance > Random.Range(0.0f, 1.0f)){
									NodeList[npidx].Children.Add(n.IDX);
									//np.Children.Add(n.IDX);
									// Node is a requirement to purchase new node
									if(RequirementsToParent > Random.Range(0.0f, 1.0f)){
										n.RequiredParents.Add(np.IDX);
									// Node is a non-requirement to purchase new node
									}else{
										n.Parents.Add(np.IDX);
									}
							}
						}
						npidx += 1;
				}

				NodeList.Add(n);
			}
		}


		//Select n_starting_purchaseable nodes to make purchaseable and visible
		NodeList[0].Purchaseable = true;
		NodeList[0].Visible = true;
		NodeList[0].Obscured = true;
		for( int i = 1; i < n_starting_purchaseable; i++){
			int x_pos = (int)Random.Range(0,X-1);
			int y_pos = (int)Random.Range(0,Y-1);
			int idx = x_pos * Y + y_pos;
			NodeList[idx].Purchaseable = true;
			NodeList[idx].Visible = true;
			NodeList[idx].Obscured = false;
		}

	}

	// This Method will review all nodes in NodeList and check for nodes with purchased and purchaseable neighbors
	// If neighbor is purchased -- set purchaseable to true, set visible to true, set obsured to false
	// If neighbor is purchaseable -- set visible to true
	public void NodeNeighborhoodCheck(){
		int idx = 0;  // Why doesn't C# have enumerate?!
		foreach (var node in NodeList) {
			if(node.Purchased){
				List<int> neighbors = NodeList[idx].Children;
				foreach (int idxj in neighbors){
					NodeList[idxj].Purchaseable = true;
					NodeList[idxj].Visible = true;
					NodeList[idxj].Obscured = false;
				}
			}else if(node.Purchaseable){
				List<int> neighbors = NodeList[idx].Children;
				foreach (int idxj in neighbors){
					NodeList[idxj].Visible = true;
				}
			}

			idx = idx + 1;
		}
	}

	// same as above, but given a specific node to check instead of a loop
	public void NodeNeighborhoodCheck(int idx){
		//foreach (var node in NodeList) {
		if(1==1){
			NodeData node = NodeList[idx];
			if(node.Purchased){
				List<int> neighbors = node.Children;
				foreach (int idxj in neighbors){
					if(!NodeList[idxj].Purchased)
					{
						// Identify if requirements are met for node idxj
						// Assume requirements are met until unmet requirement is found
						NodeList[idxj].Purchaseable = true;
						NodeList[idxj].Obscured = false;
						List<int> requirements = NodeList[idxj].RequiredParents;
						foreach(int r in requirements){
							if (!NodeList[r].Purchased){
								NodeList[idxj].Purchaseable = false;
								NodeList[idxj].Obscured = true;
								break;
							}
						}
						// regardless of purchaseable or not, set to visible
						NodeList[idxj].Visible = true;
					}
				}
			}else if(node.Purchaseable){
				List<int> neighbors = NodeList[idx].Children;
				foreach (int idxj in neighbors){
					NodeList[idxj].Visible = true;
				}
			}
		}
	}

	// Get List of Neighbors (By Cartesian Space)
	// Requires index of original node that we are checking vicinity for
	// FYI:
	// Vertical Neigbhors : idx += 1 and idx -= 1,
	// Horizontal Neighbors : idx += Height
	List<int> LiteralNeighborFinder(int idx){
		List<int> neighbors = new List<int>();
		int x = NodeList[idx].X;
		int y = NodeList[idx].Y;
		int idxj = 0;
		foreach (var node in NodeList) {
			if ( idxj != idx
			&&	NodeList[idxj].X <= x+1
			&& NodeList[idxj].X >= x-1
			&& NodeList[idxj].Y <= y+1
			&& NodeList[idxj].Y >= y-1){
				neighbors.Add(idxj);
			}
			idxj = idxj + 1;
		}
		return neighbors;
	}

	// Populate Initial Player Data
	void InitializePlayerProfile(){
		Player.Funds = InitialFunds;
		Player.Labor = InitialLabor;
		Player.Name = "todo";
		Player.Title = "Project Manager";
		Player.Fame = InitialFame;
	}

	// Initialize Turn Data
	void InitializeTurnData(){
		PastTurns.LaborPerTurn = InitialLaborPerTurn;
		PastTurns.FundChangePerTurn = 0.0f;
		PastTurns.CurrentTurnNodesBought = new List<int>();
		PastTurns.CurrentTurnNodesTested = new List<int>();
		PastTurns.NodesBoughtByTurn = new List<List<int>>();
		PastTurns.NodesTestedByTurn = new List<List<int>>();

		PastTurns.NumberOfTurns = 0;
	}

	//////////////////////////////////////////////////////////////////////
	// Functions that alter GameController Data

	// Given the index of the node, check if purchaseable. If so, check if adequate funds exist. If so, purchase.
	public string PurchaseNode(int idx){
		if(NodeList[idx].Purchaseable){
			if(NodeList[idx].CostActual <= Player.Funds
			&& NodeList[idx].LaborCost <= Player.Labor
			){
				Player.Funds = Player.Funds - NodeList[idx].CostActual;
				Player.Labor = Player.Labor - NodeList[idx].LaborCost;
				NodeList[idx].Purchased = true;
				NodeList[idx].Purchaseable = false;
				NodeNeighborhoodCheck(idx);
				NodeChange = true;
				// calculate expected reliability based on parent state upon purchase
				float parentStateOnPurchase = AssessParentState(idx);
				NodeList[idx].ParentExpectedReliability = parentStateOnPurchase;
				// append TurnData with node idx purchase
				PastTurns.CurrentTurnNodesBought.Add(idx);
				return "Purchased Node ";
			}else{
				return "Insufficient Funds";
			}
		}else{
			return "Node not Purchaseable.";
		}
	}

	// Returns a float between 0 and 1 which correlates to the completeness of TESTING
	// of the parents at time of purchase
	public float AssessParentState(int idx){
			// Comine both RequiredParents and Parents into one list
			// Calculation as follows: Reliability = R = ExpectedUntestedNodeReliability
			// In Series R = Ri * Rj * Rk * ... * Rn
			// In Parralel R = 1 - (1 - Ri)*(1 - Rk)* ...

			List<int> parents = NodeList[idx].Parents;
			parents.AddRange(NodeList[idx].RequiredParents);
			float runningMult = 1.0f; // For parents in parallel (i.e. loop below)
			foreach(int parentIDX in parents){
				// Stop recursive function once a parent is found with non-Tested
				if (!NodeList[parentIDX].Tested && NodeList[parentIDX].Purchased){
					runningMult *= (1 - ExpectedUntestedNodeReliability * AssessParentState(parentIDX));
				}
			}
			return runningMult;
	}

	// Moves Current Turn Data into past turn
	// Updates Player with changed labor and funds
	// Resets Current Turn Data
	public void CommitTurn(){
		Player.Funds += PastTurns.FundChangePerTurn;
		Player.Labor = PastTurns.LaborPerTurn;
		PastTurns.NodesBoughtByTurn.Add(PastTurns.CurrentTurnNodesBought);
		PastTurns.NodesTestedByTurn.Add(PastTurns.CurrentTurnNodesTested);
		PastTurns.CurrentTurnNodesBought = new List<int>();
		PastTurns.CurrentTurnNodesTested = new List<int>();
		PastTurns.NumberOfTurns = 1 + PastTurns.NumberOfTurns;
		//Debug.Log(Player.Labor);
		if (PastTurns.NumberOfTurns >= MaxNumberOfTurns
				|| Player.Funds <= 0.0f){
			// Begin End of game routine
				EndGame();
		}
	}

	// End of Game
	// Tally up Score
	// Victory or Defeat
	public void EndGame(){

	}

}
