using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drills
{

    public class DrillController : MonoBehaviour
    {
        // Function types
        [HideInInspector]
        public delegate void InitFunction();

        // Function to be called at the beginning of the drill.
        [HideInInspector]
        public InitFunction Init;

        // Whether this game has started.
        private bool started = false;

        // Whether this game is currently active.
        private bool isActive = false;

        // The timer for this game.
        [SerializeField]
        private Timer gameTimer;

        // The start menu for this game.
        [SerializeField]
        private GameObject startMenu;

        // The end button for this game.
        [SerializeField]
        private GameObject endGameButton;

        // The button to return to main.
        [SerializeField]
        private GameObject returnToMainButton;

        // The answer game objects.
        private GameObject[] answers;

        // The final score of this game.
        private float score = 0;

        // Use this for initialization
        void Start()
        {
            answers = GameObject.FindGameObjectsWithTag("answer");
            HideAnswers();
            endGameButton.SetActive(false);
            returnToMainButton.SetActive(false);
            if (Init != null)
            {
                Init();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        // Release all resources held by the Drill Controller.
        private void OnDestroy()
        {
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
            startMenu.SetActive(false);
            endGameButton.SetActive(true);
        }

        // Handle drill ending for game.
        public void EndGame()
        {
            gameTimer.isActive = false;
            endGameButton.SetActive(false);
            returnToMainButton.SetActive(true);
            ShowAnswers();
        }

        // Hide answers for initial game setup.
        private void HideAnswers()
        {
            foreach(GameObject go in answers)
            {
                go.SetActive(false);
            }
        }

        // Display answers at end of drill.
        private void ShowAnswers()
        {
            foreach (GameObject go in answers)
            {
                go.SetActive(true);
            }
        }

        // Compute the score of the game.
        private float ComputeScore()
        {
            return score;
        }

    }

}
