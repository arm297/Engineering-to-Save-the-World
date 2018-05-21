using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Drills {

    public class DrillDisplay : MonoBehaviour {

        // The start menu for this game.
        [SerializeField]
        private GameObject startMenu;

        // The end button for this game.
        [SerializeField]
        private Button endGameButton;

        // The menu displayed at the end of the game.
        [SerializeField]
        private GameObject endMenu;

        // The answer object for this game if any.
        private List<GameObject> answers;

        // Use this for initialization
        void Start() {
            endMenu.SetActive(false);
            answers = new List<GameObject>(GameObject.FindGameObjectsWithTag("answer"));
            endGameButton.gameObject.SetActive(false);
            HideAnswers();
        }

        // Begin execution for the game.
        public void BeginGame() {
            startMenu.SetActive(false);
            endGameButton.gameObject.SetActive(true);
        }

        // End execution of the game.
        public void EndGame() {
            endGameButton.gameObject.SetActive(false);
            endMenu.SetActive(true);
            ShowAnswers();
        }

        // Display all answers.
        private void ShowAnswers() {
            answers.ForEach(a => a.SetActive(true));
        }

        // Hide all answers.
        private void HideAnswers() {
            answers.ForEach(a => a.SetActive(false));
        }
    }

}