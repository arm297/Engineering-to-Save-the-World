using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Drills { 

public class ReturnToMain : MonoBehaviour {

	// The Controller for this drill.
	public DrillController drillController;

	// The End Game button.
	private Button endGameButton;

	// Use this for initialization
	void Start () {
		endGameButton = GetComponent<Button>();
		endGameButton.onClick.AddListener(drillController.ReturnToMainGame);
	}
}

}