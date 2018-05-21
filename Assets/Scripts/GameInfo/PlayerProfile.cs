using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * The stats of the player.
 */
public class PlayerProfile {

    // The current funds avaliable
    public float Funds { get; set; }

    // The current labor available
    public float Labor { get; set; }

    // The name of the player
    public string Name { get; set; }

    // The player's game title
    public string Title { get; set; }

    // The current fame of the player
    public float Fame { get; set; }

    // The individual player stats
    public Dictionary<string, int> Stats { get; set; }

    // The actual resources required
    public List<float> ActualResourceCriterion { get; set; }

    // The expected resources required
    public List<float> ExpectedResourceCriterion { get; set; }

    public float CostToPlayMiniGame { get; set; }

    //Stores player data
    //todo: Display Name, Title, Fame in game
    //todo: Allow player to set Name
    //todo: Award Title and Fame for succesful Events
    //todo: Base level of opportunity on Fame And/Or Title
    public PlayerProfile(float initialFunds, float initialLabor,
                         float initialFame, List<string> statNames) {
        Funds = initialFunds;
        Labor = initialLabor;
        Name = "todo";
        Title = "Project Manager";
        Fame = initialFame;
        Stats = new Dictionary<string, int>();
        foreach (string stat in statNames) {
            Stats.Add(stat, 0);
        }
        ExpectedResourceCriterion = new List<float> {
            Random.Range (0.0f, 5.0f),
            Random.Range (0.0f, 5.0f),
            Random.Range (0.0f, 5.0f),
            Random.Range (0.0f, 5.0f)
        };
        ActualResourceCriterion = new List<float> {
            Random.Range (0.0f, 5.0f),
            Random.Range (0.0f, 5.0f),
            Random.Range (0.0f, 5.0f),
            Random.Range (0.0f, 5.0f)
        };
        CostToPlayMiniGame = 3;
    }
}
