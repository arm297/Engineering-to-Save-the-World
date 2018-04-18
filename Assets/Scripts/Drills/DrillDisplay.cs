using System.Collections;
using System.Collections.Generic;
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
	private GameObject[] answers;

	// Use this for initialization
	void Start() {
		answers = GameObject.FindGameObjectsWithTag("answer");
		endGameButton.gameObject.SetActive(false);
		endMenu.SetActive(false);
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
	}

	// Display all answers.
	private void ShowAnswers() {
		foreach(GameObject answer in answers) {
			answer.SetActive(true);
		}
	} 

	// Hide all answers.
	private void HideAnswers() {
		foreach (GameObject answer in answers) {
			answer.SetActive(false);
		}
	}
}

}