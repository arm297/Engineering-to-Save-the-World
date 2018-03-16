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
	public int Height = 100;
	public int Width = 100;
	public float InitialFunds = 1000;
	public float InitialLabor = 20;
	public float InitialFame = 0;
	public float EventChance = 0.5f;
	public bool NodeChange = false; // switches to true when a node is changed. Responsibility belongs to calling function.


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
		public List<int> Children {get; set; }
		public float ProbabilityToFail {get; set; }
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

		float BaseCost = 1;
		
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
				n.Children = new List<int>();
				n.ProbabilityToFail = Random.Range(.01f,.3f);
				n.Obscured = true;

				NodeList.Add(n);
			}
		}

		//Select n_starting_purchaseable nodes to make purchaseable and visible
		for( int i = 0; i < n_starting_purchaseable; i++){
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
				List<int> neighbors = NeighborFinder(idx);
				foreach (int idxj in neighbors){
					NodeList[idxj].Purchaseable = true;
					NodeList[idxj].Visible = true;
					NodeList[idxj].Obscured = false;
				}
			}else if(node.Purchaseable){
				List<int> neighbors = NeighborFinder(idx);
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
				List<int> neighbors = NeighborFinder(idx);
				foreach (int idxj in neighbors){
					if(!NodeList[idxj].Purchased)
					{
						NodeList[idxj].Purchaseable = true;
						NodeList[idxj].Visible = true;
						NodeList[idxj].Obscured = false;
					}	
				}
			}else if(node.Purchaseable){
				List<int> neighbors = NeighborFinder(idx);
				foreach (int idxj in neighbors){
					NodeList[idxj].Visible = true;
				}
			}
		}
	}

	// Get List of Neighbors
	// Requires index of original node that we are checking vicinity for
	// FYI: 
	// Vertical Neigbhors : idx += 1 and idx -= 1, 
	// Horizontal Neighbors : idx += Height
	List<int> NeighborFinder(int idx){
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

	//////////////////////////////////////////////////////////////////////
	// Functions that alter GameController Data

	// Given the index of the node, check if purchaseable. If so, check if adequate funds exist. If so, purchase.
	public string PurchaseNode(int idx){
		if(NodeList[idx].Purchaseable){
			if(NodeList[idx].CostActual <= Player.Funds){
				Player.Funds = Player.Funds - NodeList[idx].CostActual;
				NodeList[idx].Purchased = true;
				NodeList[idx].Purchaseable = false;
				NodeNeighborhoodCheck(idx);
				NodeChange = true;
				// Populate Parent Information
				// Get List of Neighboring Nodes
				List<int> neighbors = NeighborFinder(idx);
				// Get List of Purchased non-broken Neighbors
				List<int> parents = new List<int>();
				foreach (int idxj in neighbors){
					if(NodeList[idxj].Purchased && !NodeList[idxj].Broken){
						parents.Add(idxj);
					}
				}
				// Save Parents to node idx
				NodeList[idx].Parents = parents;

				return "Purchased Node ";
			}else{
				return "Insufficient Funds";
			}
		}else{
			return "Node not Purchaseable.";
		}
	}

}



