using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Return_to_Main_Button : MonoBehaviour {

	Button myButton;
	public GameObject GameController; // Reference GameController for Script
	public GameObject GO_with_ScoreScript; //GameObject that has script with score element

	void Awake()
	{
		myButton = GetComponent<Button>(); // <-- you get access to the button component here
	
		myButton.onClick.AddListener( () => {myFunctionForOnClickEvent("stringValue", 4.5f);} );  // <-- you assign a method to the button OnClick event here
		//myButton.onClick.AddListener(() => {myAnotherFunctionForOnClickEvent("stringValue", 3);}); // <-- you can assign multiple methods
	}

	void myFunctionForOnClickEvent(string argument1, float argument2)
	{
		//Return to main game

		Camera.main.gameObject.active = false; // Disable camera (cannot have more than 2 audio listeners)
		//GameController.GetComponent<GameControllerScript>().ReturnToMainGame();
		Debug.Log("Returning to main game...");
		SceneManager.LoadScene("MainGame",LoadSceneMode.Single);//Additive);

        //TODO: Reenable all features when game controller is fixed.
 
		//GameObject c = GameObject.Find("NodeGroup");
		//CanvasGroup cg = c.GetComponent<CanvasGroup>();
		//cg.alpha = 1f;
     	//cg.blocksRaycasts = true;
		//GameObject c = GameObject.Find("MainGame");
		//Canvas cg = c.GetComponent<Canvas>();
		//cg.alpha = 1f; //this makes everything transparent
     	//cg.blocksRaycasts = true; //this prevents the UI element to receive input events
		bool success = GO_with_ScoreScript.GetComponent<game_controller>().success;
		Debug.Log("return to main game..."+success);
		//GameController.GetComponent<GameControllerScript>().ReturnToMainGame();
		//Camera.main.gameObject.active = true; // Disable camera (cannot have more than 2 audio listeners)

		//c.SetActive(false);
		//b.SetActive(false);
	}

		// Use this for initialization
	void Start () {
    	gameObject.SetActive(false);
		GameController = GameObject.Find("GameController");
		//Debug.Log("Found GameController?");
		//GameControllerScript gcs = GameController.GetComponent<GameControllerScript>();
		//Debug.Log("Found,");
	}
}
