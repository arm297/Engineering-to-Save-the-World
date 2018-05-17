using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames {
    /**
     * The controller for the minigames. Controls the guessing system of the
     * minigames, and requires using different stats for this action.
     */
    public class MinigameController : MonoBehaviour {

        // Function types
        [HideInInspector]
        public delegate void InitFunction();

        // Function to be called at the beginning of the drill.
        [HideInInspector]
        public InitFunction InitMinigame;

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
    }
}
