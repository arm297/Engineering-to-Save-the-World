using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The data for the current turn. 
 */
public class TurnData {
    public float NumberOfTurns { get; set; }
    public float LaborPerTurn { get; set; }
    public float FundChangePerTurn { get; set; }
    public List<int> CurrentTurnNodesBought { get; set; }
    public List<int> CurrentTurnNodesTested { get; set; }
    public List<List<int>> NodesBoughtByTurn { get; set; }
    public List<List<int>> NodesTestedByTurn { get; set; }

    // Creates initial turn info.
    public TurnData(float initialLaborPerTurn) {
        LaborPerTurn = initialLaborPerTurn;
        FundChangePerTurn = 0.0f;
        CurrentTurnNodesBought = new List<int>();
        CurrentTurnNodesTested = new List<int>();
        NodesBoughtByTurn = new List<List<int>>();
        NodesTestedByTurn = new List<List<int>>();
    }

    // Updates the turn data at the end of the turn.
    // Adds the nodes bought and tested on this turn to the total list of
    // bought and tested nodes.
    public void UpdateForTurnEnd() {
        NodesBoughtByTurn.Add(CurrentTurnNodesBought);
        NodesTestedByTurn.Add(CurrentTurnNodesTested);
        CurrentTurnNodesBought.Clear();
        CurrentTurnNodesTested.Clear();
        NumberOfTurns++;
    }
}
