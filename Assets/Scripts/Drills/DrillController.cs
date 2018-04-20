using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Drills
{

    /*
    The Controller for the drills. This class handles the transitions between
    drill states.
    */
    public class DrillController : MonoBehaviour
    {
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

        // Use this for initialization
        void Start() {
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
        public void BeginGame()
        {
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
            display.EndGame();
            scoreCalculator.DisplayScoreInfo();
        }

        void ReturnToMainGame() {
            //Camera.main.gameObject.SetActive(false);
            Debug.Log("Returning to main game...");
            SceneManager.LoadScene("MainGame", LoadSceneMode.Single);

        }

    }

}