/*
Description:
This class controls scene and event management and stores Node & Player data.
This is the only script to be sustained across scenes.
Author: Brighid Meredith 15:01 2/16/2018
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {

    ///////////////////////////////////
    // PUBLIC PARAMTERS

    public List<NodeData> NodeList = new List<NodeData>();
    public PlayerProfile Player;
    public TurnData PastTurns;

    public static DrillScore LastDrillScore = new DrillScore();
    public int Height = 100;
    public int Width = 100;
    public float Sparsity = .2f; // Higher sparsity rate means more holes on map
    public float InitialFunds = 1000;
    public float InitialLabor = 20;
    public float InitialFame = 0;
    public float EventChance = 0.3f;
    public float EventChance2 = 0.1f;
    public bool NodeChange = false; // switches to true when a node is changed. Responsibility belongs to calling function.
    public float MaxEuclideanDistance = 3.0f; // maximum euclidean distance between parent and child node
    public float ParentChance = 0.5f; // chance that an existing node within distance of new node is a parent nodes
    public float RequirementsToParent = .2f; // chance that a parent is a requirement to purchase new node
    public float ExpectedUntestedNodeReliability = 0.98f; // 1 = no penalty, 0 = ultimate penalty
    public float InitialLaborPerTurn = 20.0f;  // Amount of Fund Change that occurs each turn (are funds renewed or depleted?)
    public int MaxNumberOfTurns = 10;  // Number of Turns allowed in game
    public int MaxPurchaseablePath = 20; // Longest Path considered purchaseable
    public static List<string> ParameterNames = new List<string>{
        "Parameter A",
        "Parameter B",
        "Parameter C",
        "Parameter D"
    };
    public List<float> SystemParameters = new List<float> { 0.0f, 0.0f, 0.0f, 0.0f };
    public List<float> MinRequiredSystemParameters = new List<float> { 100.0f, 200.0f, 150.0f, 40.0f };
    public bool GameOver = false;
    public bool GameVictory = false;

    // For Player Stats
    public static List<string> StatNames = new List<string>{
        "Acquisition Processes",
        "Supply Processes",
        "Life Cycle Model Management",
        "InfrastructureManagement",
        "PortfolioManagement",
        "Human Resource Management",
        "Knowledge & Information Management"
    };
    public float StatBaseCost = 1.0f;
    public float StatCostScalar = 1.0f;

    // For drills.
    public float DrillChance = 0.3f;
    public float BaseScorePercent = 0.5f; //The score to compare for stat increases.

    // LOADABLE SCENES
    // List of Event-Drill Scenes (all of which may be loaded)
    private string[] list_of_drills = {
        "AssignmentQuestions1",
        "Concept Sketch Interface",
        "GoodRequirementBadRequirement",
        "RelliabilityCostDrill1",
        "ReliabilityCostDrill2"
    };
    private string MainGame = "MainGame";
    private string UI_Menu = "UI_Menu";
    private string EventSystem2_scenename = "EventSystem2";

    // Tracks the name and score of the last drill run.
    public class DrillScore {
        public float Score { get; set; }
        public float MaxScore { get; set; }
        public Dictionary<string, int> StatsToIncrease { get; set; } //Stats modified by drills and how much.

        public Dictionary<string, int> StatsToReset { get; set; } // The previous stat increases to be reset.
        public bool ActiveStatChange { get; set; }

        public bool OnTurnEnd { get; set; } // Whether this Drill was called at the turn end.
    }

    ///////////////////////////////////////
    // INITIALIZATION

    // Use this for initialization
    void Start() {
        // Call Node Initialization
        Random.InitState(System.DateTime.Now.Millisecond);
        InitializeNodeList();
        Player = new PlayerProfile(InitialFunds, InitialLabor, InitialFame, StatNames);
        PastTurns = new TurnData(InitialLaborPerTurn);
        InitializeDrillScoreStats();
        //LoadScene(MainGame);
    }

    ///////////////////////////////////////
    // SCENE MANAGEMENT

    // Method takes a scene name and attempts to load, clearing everything but GameController
    void LoadScene(string scene_name) {
        SceneManager.LoadScene(scene_name, LoadSceneMode.Single);
    }

    //////////////////////////////////////////
    // Methods for handling drills.

    // Initializes the drill scoring structure.
    void InitializeDrillScoreStats() {
        LastDrillScore.StatsToIncrease = new Dictionary<string, int>();
        LastDrillScore.StatsToReset = new Dictionary<string, int>();
    }

    // Roll to determine whether to load a drill, and which drill to decide.
    // If returned string is null, then no drill is loaded.
    string GetDrillToLoad() {
        if (Random.Range(0.0f, 1.0f) > EventChance) {
            return null;
        }
        return list_of_drills[(int)Random.Range(0f, list_of_drills.Length - 1)];
    }

    // Roll to determine wither EventSystem2.0 is in effect
    string LoadEvent() {
        if (Random.Range(0.0f, 1.0f) > EventChance2) {
            return null;
        }
        LoadScene(EventSystem2_scenename);
        return null;
    }

    // Randomly determine whether to load the drill, and load the drill based
    // on the result of the scene.
    bool ChanceForDrill() {
        string drillScene = GetDrillToLoad();
        if (drillScene == null) {
            return false;
        }
        LoadScene(drillScene);
        UpdateDrillStatIncreases();
        return true;
    }

    // Return to the main scene and handle stat updates.
    public void UpdateDrillStatIncreases() {
        float OffSetScorePercent = LastDrillScore.Score / LastDrillScore.MaxScore;
        string StatToChange = StatNames[(int)Random.Range(0f, ((float)StatNames.Count - 1))];
        if (!LastDrillScore.StatsToIncrease.ContainsKey(StatToChange)) {
            LastDrillScore.StatsToIncrease.Add(StatToChange, (int)(10 * OffSetScorePercent));
        }
        else {
            LastDrillScore.StatsToIncrease[StatToChange] += (int)(10 * OffSetScorePercent);
        }
    }

    void UpdateDrillStatModifications() {
        Dictionary<string, int> modifiedStats = new Dictionary<string, int>();
        foreach (KeyValuePair<string, int> statInc in LastDrillScore.StatsToIncrease) {
            if (Player.Stats[statInc.Key] + statInc.Value < 0) {
                modifiedStats.Add(statInc.Key, -Player.Stats[statInc.Key]);
                Player.Stats[statInc.Key] = 0;
            } else {
                Player.Stats[statInc.Key] += statInc.Value;
            }
        }
        foreach(KeyValuePair<string, int> statRes in LastDrillScore.StatsToReset) {
                Player.Stats[statRes.Key] = Mathf.Max(0, Player.Stats[statRes.Key] - statRes.Value);
        }
        LastDrillScore.StatsToReset.Clear();
        foreach (KeyValuePair<string, int> statInc in LastDrillScore.StatsToIncrease) {
            if (modifiedStats.ContainsKey(statInc.Key)) {
                LastDrillScore.StatsToReset.Add(statInc.Key, modifiedStats[statInc.Key]);
            }
            else {
                LastDrillScore.StatsToReset.Add(statInc.Key, statInc.Value);
            }
        }
        LastDrillScore.StatsToIncrease.Clear();
    }

    //////////////////////////////////////////
    // METHODS FOR CONTROLLING MINIGAME initialization.
    //public void LoadMinigame() {
        //if (Player.Labor == 0) {
        //    return;
        //}
    //    Debug.Log("load new scene");
    //    SceneManager.LoadScene("CriterionGuess", LoadSceneMode.Single);
    //}

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
        for (int i = 0; i <= X; i++) {
            // Loop through vertical
            for (int j = 0; j <= Y; j++) {

                // Initialize node data
                NodeData n = new NodeData(i, j, BaseCost, BaseLabor);

                // Chance that Node is destroyed depends on sparsity
                if (Random.Range(0.0f, 1.0f) < Sparsity) {
                    //Do not add or consider relations. Never existed...
                    n.IDX = -n.IDX;
                } else {
                    // Identify Parent, RequiredParents, Children
                    int npidx = 0;
                    foreach (NodeData _ in NodeList) {
                        NodeData np = NodeList[npidx];
                        float y_dist = n.Y - np.Y;
                        float x_dist = n.X - np.X;
                        float distance = Mathf.Sqrt(y_dist * y_dist + x_dist * x_dist);
                        // Node within distance of new node
                        if (distance <= MaxEuclideanDistance && np.IDX >= 0) {
                            // Node is a parent of new node
                            if (ParentChance > Random.Range(0.0f, 1.0f)) {
                                NodeList[npidx].Children.Add(n.IDX);
                                //np.Children.Add(n.IDX);
                                // Node is a requirement to purchase new node
                                if (RequirementsToParent > Random.Range(0.0f, 1.0f)) {
                                    n.RequiredParents.Add(np.IDX);
                                    // Node is a non-requirement to purchase new node
                                } else {
                                    n.Parents.Add(np.IDX);
                                }
                            }
                        }
                        npidx += 1;
                    }

                }
                NodeList.Add(n);
            }
        }
        CalculateSystemFeautures();

        //Select n_starting_purchaseable nodes to make purchaseable and visible
        NodeList[0].Purchaseable = true;
        NodeList[0].Visible = true;
        NodeList[0].Obscured = true;
        int ki = 0;
        while (ki < n_starting_purchaseable) {
            int ji = 0;
            foreach (NodeData n in NodeList) {
                //for( int i = 1; i < n_starting_purchaseable; i++){
                int x_pos = (int)Random.Range(0, X - 1);
                int y_pos = (int)Random.Range(0, Y - 1);
                int idx = x_pos * Y + y_pos;
                if (n.IDX == idx) {
                    NodeList[idx].Purchaseable = true;
                    NodeList[idx].Visible = true;
                    NodeList[idx].Obscured = false;
                    ki += 1;
                }
                ji += 1;
            }

        }

        //Set System Requirements
        foreach (NodeData n in NodeList) {
            int chance = (int)Random.Range(0, 30);
            if (chance <= 1 && n.IDX > 10) {
                int idx = n.IDX;
                NodeList[idx].SystReq = true;
                NodeList[idx].Visible = true;

            }
        }
        // Check that all SystReqs are purchaseable.
        WinnableGame(-1, 0);
        ObscuredVisiblityNeighborSetter();
    }

    // Returns the first neighbor within Euclidean Distance to a given Node
    // Returns either a valid idx or -1
    public int GetSingleEuclideanNeighbor(int idx) {
        NodeData n = NodeList[idx];
        int npidx = 0;
        foreach (NodeData _ in NodeList) {
            NodeData np = NodeList[npidx];
            float y_dist = n.Y - np.Y;
            float x_dist = n.X - np.X;
            float distance = Mathf.Sqrt(y_dist * y_dist + x_dist * x_dist);
            // Node within distance of new node
            if (distance <= MaxEuclideanDistance && np.IDX >= 0) {
                return npidx;
            }
        }
        return -1;
    }

    // Ensure Game is winnable with SystReqs
    // For each SystReq, check that a purchaseable path exists
    public void WinnableGame(int idx, int depth) {
        if (depth >= MaxPurchaseablePath && idx >= 0) {
            NodeList[idx].Purchaseable = true;
            NodeList[idx].Visible = true;
            NodeList[idx].Obscured = false;
            return;
        } else {
            depth += 1;
        }
        if (idx == -1) {
            // Initial call
            foreach (NodeData n in NodeList) {
                if (n.SystReq && !n.Purchaseable) {
                    idx = n.IDX;
                    // Continue to chain up, adding parents, until a purchaseable node is found
                    if (n.Parents.Count + n.RequiredParents.Count < 1) {
                        // No Parents -- Unwinnable. Add a nearby Parent
                        int neighbor = GetSingleEuclideanNeighbor(idx);
                        if (neighbor == -1) {
                            // No valid neighbors; Ensure that node[idx] is not a SystReq
                            NodeList[idx].Purchaseable = true;
                            NodeList[idx].Visible = true;
                            NodeList[idx].Obscured = false;
                        } else {
                            // Make neighbor a required parent of node and move up
                            NodeList[idx].RequiredParents.Add(neighbor);
                            NodeList[neighbor].Children.Add(idx);
                            WinnableGame(neighbor, depth);
                        }
                    } else {
                        // Parents exist. Move up to each required parent and single parent
                        foreach (int rparent in n.RequiredParents) {
                            WinnableGame(rparent, depth);
                        }
                        foreach (int parent in n.Parents) {
                            WinnableGame(parent, depth);
                            break;
                        }
                    }
                }
            }
        } else if (!NodeList[idx].Purchaseable) {
            // A recursive Call. Ensure the node is on a purchaseable path
            NodeData n = NodeList[idx];
            if (n.Parents.Count + n.RequiredParents.Count < 1) {
                // No Parents -- Unwinnable. Add a nearby Parent
                int neighbor = GetSingleEuclideanNeighbor(idx);
                if (neighbor == -1) {
                    // No valid neighbors; Ensure that node is purchaseable.
                    NodeList[idx].Purchaseable = true;
                    NodeList[idx].Visible = true;
                    NodeList[idx].Obscured = false;
                } else {
                    // Make neighbor a required parent of node and move up
                    NodeList[idx].RequiredParents.Add(neighbor);
                    NodeList[neighbor].Children.Add(idx);
                    WinnableGame(neighbor, depth);
                }
            } else {
                // Parents exist. Move up to each required parent and single parent
                foreach (int rparent in n.RequiredParents) {
                    WinnableGame(rparent, depth);
                }
                foreach (int parent in n.Parents) {
                    WinnableGame(parent, depth);
                    break;
                }
            }
        }
    }


    // This Method will review all nodes in NodeList and check for nodes with purchased and purchaseable neighbors
    // If neighbor is purchased -- set purchaseable to true, set visible to true, set obsured to false
    // If neighbor is purchaseable -- set visible to true
    public void NodeNeighborhoodCheck() {
        int idx = 0;  // Why doesn't C# have enumerate?!
        foreach (var node in NodeList) {
            if (node.Purchased) {
                List<int> neighbors = NodeList[idx].Children;
                foreach (int idxj in neighbors) {
                    NodeList[idxj].Purchaseable = true;
                    NodeList[idxj].Visible = true;
                    NodeList[idxj].Obscured = false;
                }
            } else if (node.Purchaseable) {
                List<int> neighbors = NodeList[idx].Children;
                foreach (int idxj in neighbors) {
                    NodeList[idxj].Visible = true;
                }
            }

            idx = idx + 1;
        }
    }

    // same as above, but given a specific node to check instead of a loop
    public void NodeNeighborhoodCheck(int idx) {
        //foreach (var node in NodeList) {
        if (1 == 1) {
            NodeData node = NodeList[idx];
            if (node.Purchased) {
                List<int> neighbors = node.Children;
                foreach (int idxj in neighbors) {
                    if (!NodeList[idxj].Purchased) {
                        // Identify if requirements are met for node idxj
                        // Assume requirements are met until unmet requirement is found
                        NodeList[idxj].Purchaseable = true;
                        NodeList[idxj].Obscured = false;
                        List<int> requirements = NodeList[idxj].RequiredParents;
                        foreach (int r in requirements) {
                            if (!NodeList[r].Purchased) {
                                NodeList[idxj].Purchaseable = false;
                                NodeList[idxj].Obscured = false;
                                break;
                            }
                        }
                        // regardless of purchaseable or not, set to visible
                        NodeList[idxj].Visible = true;
                    }
                }
            } else if (node.Purchaseable) {
                List<int> neighbors = NodeList[idx].Children;
                foreach (int idxj in neighbors) {
                    NodeList[idxj].Visible = true;
                }
            }
        }
    }


    // Go through all Nodes
    // for Visible non-obscured nodes : Make all non-visible parents visible & obscured
    public void ObscuredVisiblityNeighborSetter() {
        int idx = 0;  // Why doesn't C# have enumerate?!
        foreach (var node in NodeList) {
            if (node.Visible && !node.Obscured) {
                List<int> neighbors = NodeList[idx].Parents;
                foreach (int idxj in neighbors) {
                    if (!NodeList[idxj].Visible) {
                        NodeList[idxj].Visible = true;
                        NodeList[idxj].Obscured = true;
                    }
                }
                neighbors = NodeList[idx].RequiredParents;
                foreach (int idxj in neighbors) {
                    if (!NodeList[idxj].Visible) {
                        NodeList[idxj].Visible = true;
                        NodeList[idxj].Obscured = true;
                    }
                }
            }
            idx = idx + 1;
        }
    }

    // Get List of Neighbors (By Cartesian Space)
    // Requires index of original node that we are checking vicinity for
    // FYI:
    // Vertical Neigbhors : idx += 1 and idx -= 1,
    // Horizontal Neighbors : idx += Height
    List<int> LiteralNeighborFinder(int idx) {
        List<int> neighbors = new List<int>();
        int x = NodeList[idx].X;
        int y = NodeList[idx].Y;
        int idxj = 0;
        foreach (var node in NodeList) {
            if (idxj != idx
            && NodeList[idxj].X <= x + 1
            && NodeList[idxj].X >= x - 1
            && NodeList[idxj].Y <= y + 1
            && NodeList[idxj].Y >= y - 1) {
                neighbors.Add(idxj);
            }
            idxj = idxj + 1;
        }
        return neighbors;
    }

    //////////////////////////////////////////////////////////////////////
    // Functions that alter GameController Data

	// Called for getting expected total score
	public float GetExpectedScore() {
		float expectedScore = 0;

		foreach (NodeData eachNode in NodeList) {
			if (eachNode.Purchased) {
				for (int i = 0; i < Player.ExpectedResourceCriterion.Count; i++) {
					expectedScore += (Player.ExpectedResourceCriterion[i] * eachNode.ParameterEstimated[i]);
				}
			}
		}

		return expectedScore;
	}

	// Called for getting tested total score
	public float GetTestedScore() {
		float testedScore = 0;

		foreach (NodeData eachNode in NodeList) {
			if (eachNode.Tested) {
				for (int i = 0; i < Player.ExpectedResourceCriterion.Count; i++) {
					testedScore += (Player.ExpectedResourceCriterion [i] * eachNode.ParameterActuals [i]);
				}
			}
		}

		return testedScore;
	}

    // Given the index of the node, check if purchaseable. If so, check if adequate funds exist. If so, purchase.
    public string PurchaseNode(int idx) {
        if (ChanceForDrill()) {
            return "Drill Run Instead";
        }
        if (NodeList[idx].Purchaseable) {
            if (NodeList[idx].CostActual <= Player.Funds
            && NodeList[idx].LaborCost <= Player.Labor
            ) {
                Player.Funds = Player.Funds - NodeList[idx].CostActual;
                Player.Labor = Player.Labor - NodeList[idx].LaborCost;
                NodeList[idx].Purchaseable = false;
				NodeList[idx].Purchased = true;
                NodeList[idx].Obscured = false;
				NodeList[idx].Testable = true;

                NodeNeighborhoodCheck(idx);
                NodeChange = true;
                // calculate expected reliability based on parent state upon purchase
                float parentStateOnPurchase = AssessParentState(idx);
                NodeList[idx].ParentExpectedReliability = parentStateOnPurchase;
                // append TurnData with node idx purchase
                PastTurns.CurrentTurnNodesBought.Add(idx);
                CalculateSystemFeautures();
                ObscuredVisiblityNeighborSetter();
                return "Purchased Node ";
            } else {
                return "Insufficient Funds";
            }
        } else {
            return "Node not Purchaseable.";
        }
    }

    // Returns a float between 0 and 1 which correlates to the completeness of TESTING
    // of the parents at time of purchase
    public float AssessParentState(int idx) {
        // Comine both RequiredParents and Parents into one list
        // Calculation as follows: Reliability = R = ExpectedUntestedNodeReliability
        // In Series R = Ri * Rj * Rk * ... * Rn
        // In Parralel R = 1 - (1 - Ri)*(1 - Rk)* ...

        List<int> parents = NodeList[idx].Parents;
        parents.AddRange(NodeList[idx].RequiredParents);
        float runningMult = 1.0f; // For parents in parallel (i.e. loop below)
        foreach (int parentIDX in parents) {
            // Stop recursive function once a parent is found with non-Tested
            if (!NodeList[parentIDX].Tested && NodeList[parentIDX].Purchased) {
                runningMult *= (1 - ExpectedUntestedNodeReliability * AssessParentState(parentIDX));
            }
        }
        return runningMult;
    }

    // Moves Current Turn Data into past turn
    // Updates Player with changed labor and funds
    // Resets Current Turn Data
    public void CommitTurn() {
        Player.Funds += PastTurns.FundChangePerTurn;
        Player.Labor = PastTurns.LaborPerTurn;
        PastTurns.UpdateForTurnEnd();
        UpdateDrillStatModifications();
        LoadEvent();
        //Debug.Log(Player.Labor);

		float totalTestCost = 0;

		foreach (NodeData eachNode in NodeList) {
			if (eachNode.TestReady) {
				totalTestCost += eachNode.LaborCost;
				eachNode.TestReady = false;
				eachNode.Tested = true;
			}
		}

        if (PastTurns.NumberOfTurns >= MaxNumberOfTurns
                || Player.Funds <= 0.0f) {
            // Begin End of game routine
            EndGame();
        }
        CalculateSystemFeautures();
        ChanceForDrill();
    }

    // updates global for parameter values at system level
    public void CalculateSystemFeautures() {

      // Reset System Features
      SystemParameters = new List<float> { 0.0f, 0.0f, 0.0f, 0.0f };
        foreach (NodeData node in NodeList) {
            if (node.Purchased) {
                if (node.ParameterActuals.Count != SystemParameters.Count) {
                    SystemParameters = node.ParameterActuals;
                } else {
                    int count = 0;
                    foreach (float val in node.ParameterActuals) {
                        SystemParameters[count] += val;
                        count += 1;
                    }
                }
            }
        }
    }

    // Determine if minimum system requirements have been Method
    // Returns true if min reqs have been met, false otherwise
    public bool CheckMinSystRequirements() {
        // Go through all nodes and look for any unpurchased System Requirements
        foreach (NodeData n in NodeList) {
            if (n.IDX >= 0 && n.SystReq && !n.Purchased) {
                return false;
            }
        }
        // Compare System Features with minimum required features
        CalculateSystemFeautures();
        int i = 0;
        if (Player.Funds <= 0.0f) {
            return false;
        }
        foreach (float val in SystemParameters) {
            if (MinRequiredSystemParameters[i] > val) {
                return false;
            }
            i += 1;
        }
        return true;
    }

    // Determine Cost to purchase more stat, based off existing stat
    public float PurchaseStatCost(string statName) {
        int lvl = Player.Stats[statName];
        return StatBaseCost * Mathf.Exp(StatCostScalar * lvl);
    }

    // Purchase Stat
    public string PurchasePlayerStat(string statName) {

        // Cost to improve stat
        float cost = PurchaseStatCost(statName);
        if (Player.Labor >= cost) {
            Player.Stats[statName] += 1;
            Player.Labor -= cost;
            return "Purchased";
        } else {
            return "Insuficient Labor";
        }
    }

    ///////////////////////////////////////////////////////////////////////////
    // Functions to handle ending the game.
    public bool SufficientFeatures() {
        for(int i = 0; i < SystemParameters.Count; i++) {
            if (SystemParameters[i] < MinRequiredSystemParameters[i]) {
                return false;
            }
        }
        return true;
    }


    // End of Game
    // Tally up Score
    // Victory or Defeat
    public void EndGame() {
      GameVictory = CheckMinSystRequirements();
      GameOver = true;

    }

}
