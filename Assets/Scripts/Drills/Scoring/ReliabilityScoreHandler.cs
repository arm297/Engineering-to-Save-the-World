using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Drills {

    /**
     * This class computes the reliability score for the reliability drill,
     * and displays the reliability and cost during the drill.
     */
    public class ReliabilityScoreHandler : ScoreHandler {

        // The reliability drill level.
        [SerializeField]
        private uint levelID;

        // The text for the total current cost.
        private Text costText;

        // The text for the total current reliability.
        private Text reliabilityText;

        // Total current reliability.
        private double reliability = 0f;

        // Total current cost.
        private double cost = 0f;

        // Target reliability.
        private readonly double targetReliability = 0.97f;

        // Target cost.
        private readonly double targetCost = 15;

        // The error allowed before failure.
        private readonly double permissibleError = 0.0001f;

        // Whether the target reliability has been reached.
        private bool targetReached = false;

        // The nodes of the reliability diagram.
        private List<ReliabilityNode> nodes;

        // The labels for the reliability drill.
        private List<ReliabilityLabel> labels;

        // Use this for initialization
        void OnEnable() {
            // Get labels and nodes.
            GameObject[] nodeObjects = GameObject.FindGameObjectsWithTag("snap_into");
            nodes = nodeObjects.Select(n => n.GetComponent<ReliabilityNode>()).ToList();
            GameObject[] labelObjects = GameObject.FindGameObjectsWithTag("block");
            labels = labelObjects.Select(l => l.GetComponent<ReliabilityLabel>()).ToList();

            // Add listeners to update reliability and cost.
            foreach (ReliabilityNode node in nodes) {
                Debug.Log(node);
                node.OnBlockPlaced += DisplayCurrentStats;
                node.OnBlockRemoved += DisplayCurrentStats;
            }

            ComputeCost();
            ComputeReliability();

            reliabilityText = GameObject.Find("CurrentReliabilityText").GetComponent<Text>();
            costText = GameObject.Find("CostText").GetComponent<Text>();
            reliabilityText.text += "" + Math.Round(reliability, 3);
            costText.text += "" + cost;

            // Display target reliability and label.
            GameObject.Find("TargetReliabilityText").GetComponent<Text>().text =
                "Target Reliability: " + Math.Round(targetReliability, 3);
            GameObject.Find("TargetCostText").GetComponent<Text>().text =
                "Target Cost: " + targetCost;
        }

        // Update is called once per frame
        void Update() {


        }

        // Release all held resources.
        private void OnDisable() {
            foreach (ReliabilityNode node in nodes) {
                node.OnBlockPlaced -= DisplayCurrentStats;
                node.OnBlockRemoved -= DisplayCurrentStats;

            }
        }

        // Release all held resources.
        private void OnDestroy() {
            foreach (ReliabilityNode node in nodes) {
                if (node != null) {
                    node.OnBlockPlaced -= DisplayCurrentStats;
                    node.OnBlockRemoved -= DisplayCurrentStats;
                }
            }
        }

        // Computes the score for the reliability drill.
        public override float ComputeScore() {
            ComputeCost();
            ComputeReliability();
            return IsVictory() ? 1.0f : 0.0f; 
        }

        // Computes the target score for the reliability drill.
        public override float ComputeMaxScore() {
            return 1.0f;
        }

        // Computes the total cost of the reliability drill.
        private void ComputeCost() {
            cost = 0;
            foreach (ReliabilityNode node in nodes) {
                if (node.IsFilled()) {
                    cost += ((ReliabilityLabel)node.containedObject).cost;
                }
            }
        }

        // Computes the total reliability of the drill.
        private void ComputeReliability() {
            reliability = 0;
            List<double> nodeReliabilities = new List<double>();
            foreach (ReliabilityNode node in nodes) {
                if (node.IsFilled()) {
                    nodeReliabilities.Add(((ReliabilityLabel)node.containedObject).reliability);
                } else {
                    nodeReliabilities.Add(1f);
                }
            }
            List<int> indices = new List<int>();
            List<double> rs = new List<double>();
            int nodeCount = 0;
            switch (levelID) {
                case 1:
                    nodeCount = 9;
                    break;
                case 2:
                    nodeCount = 12;
                    break;
                case 10:
                    nodeCount = 16;
                    break;
            }
            for (int i = 1; i < nodeCount; i++) {
                Debug.Log(i);
                indices.Add(nodes.FindIndex(n => n.blockID.Equals(i)));
            }
            foreach (int index in indices) {
                Debug.Log(index);
                rs.Add(nodeReliabilities[index]);
            }
            reliability = GetLevelReliability(rs, levelID);
        }


        // Compute the reliability for a given set of reliabilites and level.
        public double GetLevelReliability(List<double> rs, uint level) {
            switch (level) {
                case 1:
                    return rs[0] * rs[7] * (1 - (1 - rs[1]) * (1 - rs[4])) * (1 - (1 - rs[2]) * (1 - rs[5])) * (1 - (1 - rs[3]) * (1 - rs[6]));
                case 2:
                    return rs[0] * rs[7] * (1 - (1 - rs[1]) * (1 - rs[4]) * (1 - rs[8])) * (1 - (1 - rs[2]) * (1 - rs[5]) * (1 - rs[9])) * (1 - (1 - rs[3]) * (1 - rs[6]) * (1 - rs[10]));
                case 10:
                    return rs[0] * rs[12] * rs[10] * (1 - (1 - rs[7] * rs[8]) * (1 - rs[13] * rs[14]) * rs[9] * rs[11]) * (1 - rs[1] * (1 - rs[2] * rs[3]) * (1 - rs[6] * rs[7]) * rs[4]);
                default:
                    throw new IndexOutOfRangeException("No reliability calculation for this level");
            }
        }

        // Display the final score info of the reliability level.
        public override void DisplayScoreInfo() {
            ComputeReliability();
            ComputeCost();
            statusText.text = IsVictory() ? "You win!" : "You lose!";
            scoreText.text = "Final cost: " + cost + "\n" + "Target cost: " + targetCost + "\n";
            scoreText.text += "Final reliability: " + Math.Round(reliability, 3) + "\n"
                + "Target Reliability: " + Math.Round(targetReliability, 3) + "\n";
        }

        // Display the current reliability and cost.
        public void DisplayCurrentStats() {
            ComputeReliability();
            ComputeCost();
            reliabilityText.text = "Reliability: " + Math.Round(reliability, 3);
            costText.text = "Cost: " + cost;
        }

        // Whether or not the result is a victory.
        private bool IsVictory() {
            foreach (BlockContainer node in nodes) {
                if (node.IsEmpty()) {
                    return false;
                }
            }
            return (reliability > targetReliability -permissibleError) && (targetCost >= cost);
        }
    }

}