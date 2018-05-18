using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames {

    /**
     * The behavior for the criterion display. 
     */
    public class CriterionWeightDisplay : MonoBehaviour {

        // The first input field.
        public InputField field1;

        // The second input field.
        public InputField field2;

        // The field for displaying the relation between the criteria.
        [SerializeField]
        private Text criteriaRelationField;

        // The field for displaying the labor.
        [SerializeField]
        private Text laborText;

        // The delegate function to handle the listener.
        public delegate void ClickButtonFunction(string v1, string v2);

        // Function called on button click.
        public ClickButtonFunction ButtonListenerFunction;


        // Use this for initialization
        void Start() {

        }

        // The function called when the enter button is clicked.
        public void EnterButtonListener() {
            if (ButtonListenerFunction != null) {
                string criterion1 = field1.text.Trim();
                string criterion2 = field2.text.Trim();
                ButtonListenerFunction(criterion1, criterion2);
            }
        }

        // Display the labor used.
        public void DisplayLaborUsed(float laborUsed) {
            laborText.text = "Labor Used: " + laborUsed;
        }

        // Display the message showing criterion relation.
        public void AddCriterionRelation(string newMessage) {
            criteriaRelationField.text += newMessage + "\n";   
        }

        public void ClearCriteriaRelationField() {
            criteriaRelationField.text = "";
        }
    }
}
