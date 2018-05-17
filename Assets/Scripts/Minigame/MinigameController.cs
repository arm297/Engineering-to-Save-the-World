using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Minigames {
    /**
     * The controller for the minigames. Controls the guessing system of the
     * minigames, and requires using different stats for this action.
     */
    public class MinigameController : MonoBehaviour {

        // Function types
        public delegate void ActionFunction();

        // Function to be called at the beginning of the drill.
        public ActionFunction InitMinigame;

        // Handle the guess actions.
        public ActionFunction HandleGuess;

        // Whether this game is currently active.
        private bool isActive = false;

        // The labor used during each guess.
        [SerializeField]
        private float laborPerGuess = 1f;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }



        public void ReturnToMainGame() {
            Debug.Log("Returning to main game");
            SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
        }
    }
}
