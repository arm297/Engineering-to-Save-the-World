using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class intro_script : MonoBehaviour {

	public Button start; // the start button
	public int TimesPressed = 0;  // number of times the start button was pressed
	public bool isScrolling = false;
	public bool isFinished = false;
	private float rotation;
	public GameObject deactivateThis;
	public GameObject hideThis;
	public GameObject hideThis2;


	public AudioSource soundTrack; // sound track
	public AudioSource vader;
	public float TimeForIntro = 90f;
	public float TimeForDarthVader = 5f;
	public float Timer = 0;

	// Use this for initialization
	void Start () {
				start.onClick.AddListener(GoButtonPressed);
				isScrolling = false;
				rotation = gameObject.transform.rotation.eulerAngles.x;
				//rotation = gameObject.GetComponentInParent().eulerAngles.x;
				Debug.Log("Parent rotation: " + rotation);
	}

	// function for go button
	// first press--display text
	// second press--load main game
	public void GoButtonPressed(){
		TimesPressed += 1;

		if(TimesPressed == 1){
			//Start Text
			isScrolling = true;
			deactivateThis.SetActive(false);
			soundTrack.Play();
		}else{
			//Load Main Game
		}
	}

	// Update is called once per frame
	void Update () {
			// Check for starting or stopping
			if(Input.GetKeyDown(KeyCode.A))
			{
					isScrolling = !isScrolling;
			}

			// Check if the user wants to quit the application
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				EndIntro();
			}

			if (isFinished){
				Timer += Time.deltaTime;
				if (Timer > TimeForDarthVader){
						SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
				}
			}

			// If we are scrolling, perform update action
			if (isScrolling)
			{

				Timer += Time.deltaTime;

				if (Timer > TimeForIntro)
				{
					EndIntro();
				}
			 // Get the current transform position of the panel
			 Vector3 _currentUIPosition = gameObject.transform.position;

			 // Increment the Y value of the panel
			 Vector3 _incrementYPosition =
				new Vector3(_currentUIPosition.x ,
										_currentUIPosition.y + 1.5f * Mathf.Sin(Mathf.Deg2Rad * rotation),
										_currentUIPosition.z + 1.5f * Mathf.Cos(Mathf.Deg2Rad * rotation));

			 // Change the transform position to the new one
			 gameObject.transform.position = _incrementYPosition;
		 }
	}

	public void EndIntro(){
		hideThis.transform.localScale = new Vector3(0, 0, 0);
		hideThis2.transform.localScale = new Vector3(0, 0, 0);
		soundTrack.Stop();
		vader.Play();
		isFinished = true;
		isScrolling = false;
		Timer = 0;

	}
}
