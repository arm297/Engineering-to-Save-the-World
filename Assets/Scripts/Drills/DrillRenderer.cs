using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drills {

public class DrillRenderer : MonoBehaviour {

	// The start menu for this game.
    [SerializeField]
    private GameObject startMenu;

	// The end button for this game.
    [SerializeField]
    private GameObject endGameButton;

	// The return to main game button.
	[SerializeField]
	private GameObject returnToMainButton;

	// The answer object for this game if any.
	private GameObject[] answers;

	// Use this for initialization
	void Start() {
		answers = GameObject.FindGameObjectsWithTag("answer");
		endGameButton.SetActive(false);
		returnToMainButton.SetActive(false);
	}

	public void BeginGame() {
		startMenu.SetActive(false);
		endGameButton.SetActive(true);
	}

	public void EndGame() {
		endGameButton.SetActive(false);
		returnToMainButton.SetActive(true);
	}
}

}