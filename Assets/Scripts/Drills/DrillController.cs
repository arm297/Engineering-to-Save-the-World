﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Drills {

    /*
    The Controller for the drills. This class handles the transitions between
    drill states.
    */
    public class DrillController : MonoBehaviour {
        // Function types
        [HideInInspector]
        public delegate void InitFunction();

        // Function to be called at the beginning of the drill.
        [HideInInspector]
        public InitFunction InitDrill;

        // Whether this game has started.
        private bool started = false;

        // Whether this game is currently active.
        private bool isActive = false;

        [Header("Drill Components")]
        // The timer for this game.
        [SerializeField]
        private Timer gameTimer;

        // The display for the drill.
        [SerializeField]
        private DrillDisplay display;

        // The score handler for computing the score.
        [SerializeField]
        private ScoreHandler scoreCalculator;

        // The final score of the drill.
        private float finalScore = 0f;

        // The maximum score of the drill.
        private float maxScore = 0f;

        // Use this for initialization
        void Start() {
            // If a drill init method is provided by component, then call.
            if (InitDrill != null) {
                InitDrill();
            }
        }

        // Update is called once per frame
        void Update() {

        }

        // Release all resources held by the Drill Controller.
        private void OnDestroy() {
            if (started) {
                gameTimer.OnTimerEnd -= EndGame;
            }
        }

        // Set up game for Drill Controller.
        public void BeginGame() {
            gameTimer.OnTimerEnd += EndGame;
            gameTimer.isActive = true;
            started = true;
            isActive = true;
            display.BeginGame();

        }

        // Handle drill ending for game.
        public void EndGame() {
            gameTimer.isActive = false;
            finalScore = scoreCalculator.ComputeScore();
            maxScore = scoreCalculator.ComputeMaxScore();
            display.EndGame();
            scoreCalculator.DisplayScoreInfo();
        }

        // Loads back the main scene and readds the scoring data.
        public void ReturnToMainGame() {
            if (GameController.LastDrillScore != null) { // Useful for running drill standalone.
                GameController.LastDrillScore.Score = scoreCalculator.score;
                GameController.LastDrillScore.MaxScore = scoreCalculator.maxScore;
            }
            SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
            GameObject.Find("GameControl").GetComponent<GameController>().UpdateDrillStatIncreases();
        }

    }

}