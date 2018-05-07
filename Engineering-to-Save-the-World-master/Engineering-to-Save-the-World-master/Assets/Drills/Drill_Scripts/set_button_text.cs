using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class set_button_text : MonoBehaviour {


	Button myButton;


	void Awake()
	{
		myButton = GetComponent<Button>(); // <-- you get access to the button component here
	
		myButton.onClick.AddListener( () => {myFunctionForOnClickEvent("stringValue", 4.5f);} );  // <-- you assign a method to the button OnClick event here
		//myButton.onClick.AddListener(() => {myAnotherFunctionForOnClickEvent("stringValue", 3);}); // <-- you can assign multiple methods
	}
	
	void myFunctionForOnClickEvent(string argument1, float argument2)
	{
		// your code goes here
		
		print(argument1 + ", " + argument2.ToString());
		GameObject timer = GameObject.FindWithTag("timer");
		timer.GetComponent<game_controller>().game_started = true;
		GameObject start_scene = GameObject.FindWithTag("start_scene");
		Destroy(start_scene);
	}

	// Use this for initialization
	void Start () {
    	Text yourButtonText = transform.Find("Text").GetComponent<Text>();
		yourButtonText.text = "Start";
	}
}
