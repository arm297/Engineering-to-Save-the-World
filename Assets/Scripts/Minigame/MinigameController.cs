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

        // The criterion weight display.
        [SerializeField]
        private CriterionWeightDisplay display;

        // The labor used during each guess.
        [SerializeField]
        private float laborPerGuess = 1f;

        // The current amount of labor used.
        private float laborUsed = 0f;
        
        // Use this for initialization
        void Start() {
            display.DisplayLaborUsed(0f);
            display.DisplayLaborLeft(GameController.Player.Labor);
            display.ButtonListenerFunction += GuessAction;
        }

        // Release all resources held by this controller.
        private void OnDestroy() {
            display.ButtonListenerFunction -= GuessAction;
        }

        // Returns to the main game.
        public void ReturnToMainGame() {
            Debug.Log("Returning to main game");
            SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
        }

        // Runs the action performed on a guess.
        public void GuessAction(string criterion1, string criterion2) {
            Comparison comp = CompareCriteria(criterion1, criterion2);
            if (GameController.Player.Labor < laborPerGuess) {
                display.AddCriterionRelation("You do not have enough labor.");
            }
            else if (comp == Comparison.INVALID) {
                display.AddCriterionRelation("Invalid input");
            }
            else {
                GameController.Player.Labor -= laborPerGuess;
                laborUsed += laborPerGuess;
                display.DisplayLaborUsed(laborUsed);
                display.DisplayLaborLeft(GameController.Player.Labor);
                string message = criterion1 + " " + ComparisonToString(comp)
                                + " " + criterion2;
                display.AddCriterionRelation(message);
            }
            
        }

        // Compares the values of the criteria and returns a comparison of their
        // relative different.
        public Comparison CompareCriteria(string criterion1, string criterion2) {
            int index1 = getCriterionIndex(criterion1);
            int index2 = getCriterionIndex(criterion2);
            int nCriteria = GameController.Player.ActualResourceCriterion.Count;
            if (index1 < 0 || index2 < 0 || index1 >= nCriteria 
                || index2 >= nCriteria) {
                return Comparison.INVALID;
            }
            float cval1 = GameController.Player.ActualResourceCriterion[index1];
            float cval2 = GameController.Player.ActualResourceCriterion[index2];
            float criteriaDiff = cval1 - cval2;
            if (criteriaDiff < -0.1f) {
                return Comparison.LLT;
            }
            else if (criteriaDiff < -0.05f) {
                return Comparison.LT;
            }
            else if (criteriaDiff < 0.05f) {
                return Comparison.EQ;
            }
            else if (criteriaDiff < 0.1f) {
                return Comparison.GT;
            }
            else {
                return Comparison.GGT;
            }
        }

        // The comparison value of getting the two features.
        public enum Comparison {
            GGT,    // Much greater than (>>)
            GT,     // Greater than (>)
            EQ,     // Approximately equal (~)
            LT,     // Less than (<)
            LLT,    // Much less than (<<)
            INVALID, // Not a valid comparison.
        }

        // Gets the index of the criterion from the string.
        public static int getCriterionIndex(string criterion) {
            if (criterion.Length > 1) {
                return -1;
            }
            char criterionChar = criterion.ToUpper()[0];
            return criterionChar - 65;
        }

        // Gets the string representation of the comparison.
        public static string ComparisonToString(Comparison c) {
            switch(c) {
                case Comparison.GGT:
                    return ">>";
                case Comparison.GT:
                    return ">";
                case Comparison.EQ:
                    return "~";
                case Comparison.LT:
                    return "<";
                case Comparison.LLT:
                    return "<<";
            }
            return null;
        }
    }
}
