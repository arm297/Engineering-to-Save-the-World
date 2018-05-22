using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The data stored by the node. 
 */
public class NodeData {
    // The number of nodes currently created.
    private static int numNodes = 0;

    // The unique index of this node
    public int IDX { get; set; }
    
    // The name of this node
    public string Name { get; set; }

    // The x index of this node
    public int X { get; set; }

    // The y index of this node
    public int Y { get; set; }

    // The actual cost of this node
    public float CostActual { get; set; }

    // The current estimated cost of this node
    public float CostEstimated { get; set; }

    // The actual parameters of this node
    public List<float> ParameterActuals { get; set; }

    // The current estimated parameters of this node
    public List<float> ParameterEstimated { get; set; }

    // The name of the parameters of this node
    public List<string> ParameterNames { get; set; }

    // Whether this node is purchaseable
    public bool Purchaseable { get; set; }

    // Whether this node has been purchased
    public bool Purchased { get; set; }

    // Whether this node is currently visitable
    public bool Visible { get; set; }

    // Whether this node is currently obscured
    public bool Obscured { get; set; }

    // Whether this node is currently testable
    public bool Testable { get; set; }

    // Whether this node is ready to be tested
    public bool TestReady { get; set; }

    // Whether this node has been tested
    public bool Tested { get; set; }

    // Whether this node is broken
    public bool Broken { get; set; }

    // The cost to fix this node, if broken
    public float CostToFix { get; set; }

    // The indices of this node's parents
    public List<int> Parents { get; set; }

    // The indices of this node's required parents
    public List<int> RequiredParents { get; set; }

    // The indices of this nodes children
    public List<int> Children { get; set; }

    // The current probability of this node to fail
    public float ProbabilityToFail { get; set; }

    // The expected reliability of this node's parents
    public float ParentExpectedReliability { get; set; }

    // The labor cost for this node
    public float LaborCost { get; set; }

    // Whether this node is required to win the game
    public bool SystReq { get; set; }

    // TODO: What is this?
    public int ObscuredRank { get; set; }

    // Constructs a new node with the given position, settings
    // its parameters according to the base cost and labor.
    // The initial node has no required parents or children.
    public NodeData(int x, int y, float baseCost, float baseLabor) {
        IDX = numNodes++;
        Name = "node " + IDX;
        X = x;
        Y = y;
        CostActual = baseCost * Random.Range(0.8f, 1.5f);
        CostEstimated = CostActual * Random.Range(0.5f, 1.1f);
        ParameterActuals = new List<float>{
                    Random.Range(0.0f,5.0f),
                    Random.Range(0.0f,5.0f),
                    Random.Range(0.0f,5.0f),
                    Random.Range(0.0f,5.0f)
        };
        ParameterEstimated = new List<float>{
                    ParameterActuals[0] * Random.Range(.95f,1.3f),
                    ParameterActuals[0] * Random.Range(.95f,1.3f),
                    ParameterActuals[0] * Random.Range(.95f,1.3f),
                    ParameterActuals[0] * Random.Range(.95f,1.3f)
        };
        ParameterNames = new List<string>{
                    "Parameter A",
                    "Parameter B",
                    "Parameter C",
                    "Parameter D"
        };
        CostToFix = CostActual * Random.Range(.2f, .7f);
        Parents = new List<int>();
        RequiredParents = new List<int>();
        Children = new List<int>();
        ProbabilityToFail = Random.Range(.01f, .3f);
        Obscured = true;
        ObscuredRank = 3;
        ParentExpectedReliability = 1;
        LaborCost = baseLabor * Random.Range(0.8f, 1.5f);
		Tested = false;
		TestReady = false;
    }

}
