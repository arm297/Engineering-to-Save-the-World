using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnToMainGameButton : MonoBehaviour {

    // Reference to return button.
    Button returnButton;
    // Drill game controller.
    GameObject gameController;
    // Score computation object.
    public GameObject scoreCalculator;

    // Initialize return button listener.
    void Awake ()
    {
        returnButton = gameController.GetComponent<Button>();
        returnButton.onClick.AddListener(ReturnToMainGame);
    }

	// Use this for initialization
	void Start()
    {
        gameObject.SetActive(false);
        gameController = GameObject.Find("GameController");
	}

        // 
    void ReturnToMainGame() 
    {
        //Camera.main.gameObject.SetActive(false);
        Debug.Log("Returning to main game...");
        SceneManager.LoadScene("MainGame", LoadSceneMode.Single);

    }

}
