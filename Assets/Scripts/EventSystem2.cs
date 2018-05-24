using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.SceneManagement;

public class EventSystem2 : MonoBehaviour {

    // The display for the player's current stats.
	public GameObject StatDisplay;

    // The display for the player's current labor.
	public GameObject LaborDisplay;

    // The display for the player's current funds.
	public GameObject FundsDisplay;

    // The display used by the event for giving user info.
	public GameObject EventText;

    // The option buttons for the different choices.
	public Button Opt1Button;
	public Button Opt2Button;
	public Button Opt3Button;
	public Button Opt4Button;

	private Event[] events = new Event[1];

    // The current event running.
	private Event this_event;

    // The labor for each skill.
	private float skill1Labor = 0.0f;
	private float skill2Labor;
	private float skill3Labor;

    // The cost for each skill.
	private float skill1Cost = 0.0f;
	private float skill2Cost;
	private float skill3Cost;

    // This inner class stores the information about a single event.
	public class Event
	{
		public string EventText { get; set; }

		public string Opt1Text { get; set; }
		public string Opt2Text { get; set; }
		public string Opt3Text { get; set; }
		public string Opt4Text { get; set; }

		public float OptBaseCost { get; set; }
		public float OptBaseLabor { get; set; }

		public int Opt1Type { get; set; }
		public int Opt2Type { get; set; }
		public int Opt3Type { get; set; }

	}

	// Use this for initialization
	void Start () {

		AddListeners();

		// Initialize Events (an example event below)
		Event e = new Event();
		e.EventText = "A core lead engineer has defected to the Dark Side after the Empire kidnapped the engineer's family. Before leaving, the Empire forced the lead to steal plans and delete all local archives. If the plans are not recovered, we risk exposure to core systems which will require a redisign. This will cost resources that the Republic can ill-afford. How will you respond?";

		e.Opt1Text = "Re-aqcuire Lead Engineer and Family\n(Acquisition Process)";
		e.Opt2Text = "Check smuggler syndicate for master thieves to steal back plans\n(Supply Processes)";
		e.Opt3Text = "Hire Bounty Hunter to reclaim assets.\n(Human Resource Management)";
		e.Opt4Text = "Redesign System";

		e.OptBaseCost = 10;
		e.OptBaseLabor = 10;

		e.Opt1Type = 0;
		e.Opt2Type = 1;
		e.Opt3Type = 5;

		events[0] = e;

		this_event = e;
	}

	// Update is called once per frame
	private bool updated = false;
	void Update () {
		if(!updated){
			updated = true;
			EstablishCosts(this_event);
			PrintEvent(this_event);
		}
		PlayerStatDisplay();

	}

	// Add Listeners
	void AddListeners(){
		Opt1Button.onClick.AddListener(opt1);
		Opt2Button.onClick.AddListener(opt2);
		Opt3Button.onClick.AddListener(opt3);
		Opt4Button.onClick.AddListener(opt4);
	}

	// Call to return to main game
	void EndEvent(){
		SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
	}

	// call to notify user that funds/labor is insufficient
	void InsufficientFunds(){

	}

	// Actions
	void opt1(){
		if(CheckCosts(skill1Labor, skill1Cost)){
			GameObject.Find("GameControl").GetComponent<GameController>().Player.Labor -= skill1Labor;
			GameObject.Find("GameControl").GetComponent<GameController>().Player.Funds -= skill1Cost;
			EndEvent();
		}else{
			InsufficientFunds();
		}
	}

	void opt2(){
		if(CheckCosts(skill1Labor, skill1Cost)){
			GameObject.Find("GameControl").GetComponent<GameController>().Player.Labor -= skill2Labor;
			GameObject.Find("GameControl").GetComponent<GameController>().Player.Funds -= skill2Cost;
			EndEvent();
		}else{
			InsufficientFunds();
		}
	}

	void opt3(){
		if(CheckCosts(skill1Labor, skill1Cost)){
			GameObject.Find("GameControl").GetComponent<GameController>().Player.Labor -= skill3Labor;
			GameObject.Find("GameControl").GetComponent<GameController>().Player.Funds -= skill3Cost;
			EndEvent();
		}else{
			InsufficientFunds();
		}
	}

	void opt4(){
		if(CheckCosts(skill1Labor, skill1Cost)){
			GameObject.Find("GameControl").GetComponent<GameController>().Player.Labor -= this_event.OptBaseLabor;
			GameObject.Find("GameControl").GetComponent<GameController>().Player.Funds -= this_event.OptBaseCost;
			EndEvent();
		}else{
			GameObject.Find("GameControl").GetComponent<GameController>().Player.Labor = 0;
			GameObject.Find("GameControl").GetComponent<GameController>().Player.Funds -= 2*this_event.OptBaseCost;
		}
	}

	// Check if sufficient funds and labor exist for option selection
	bool CheckCosts(float laborCost, float fundsCost){
		if(!(GameObject.Find("GameControl").GetComponent<GameController>().Player.Labor >= laborCost)){
			return false;
		}
		if(!(GameObject.Find("GameControl").GetComponent<GameController>().Player.Funds >= fundsCost)){
			return false;
		}
		return true;
	}

	// Establish costs
	void EstablishCosts(Event e){
			Dictionary<string, int> playerStats = GameObject.Find("GameControl").GetComponent<GameController>().Player.Stats;
			float skill1 = playerStats.Values.ElementAt(e.Opt1Type) + 1;
			float skill2 = playerStats.Values.ElementAt(e.Opt2Type) + 1;
			float skill3 = playerStats.Values.ElementAt(e.Opt3Type) + 1;

			skill1Labor = e.OptBaseLabor / skill1;
			skill2Labor = e.OptBaseLabor / skill2;
			skill3Labor = e.OptBaseLabor / skill3;

			skill1Cost = e.OptBaseCost / skill1;
			skill2Cost = e.OptBaseCost / skill2;
			skill3Cost = e.OptBaseCost / skill3;

	}

	// Print Event to screen
	void PrintEvent(Event e){
		EventText.GetComponent<Text>().text = e.EventText;
		Opt1Button.GetComponentInChildren<Text>().text = e.Opt1Text + "\t Labor ("+skill1Labor+")\t Funds ("+skill1Cost+")";
		Opt2Button.GetComponentInChildren<Text>().text = e.Opt2Text + "\t Labor ("+skill2Labor+")\t Funds ("+skill2Cost+")";;
		Opt3Button.GetComponentInChildren<Text>().text = e.Opt3Text + "\t Labor ("+skill3Labor+")\t Funds ("+skill3Cost+")";;
		Opt4Button.GetComponentInChildren<Text>().text = e.Opt4Text + "\t Labor ("+e.OptBaseLabor+")\t Funds ("+e.OptBaseCost+")";;
	}

	// Displays Player Stats in text window
	void PlayerStatDisplay(){

		Dictionary<string, int> playerStats = GameObject.Find("GameControl").GetComponent<GameController>().Player.Stats;

		string stats = "lvl\tstat:";
		foreach (KeyValuePair<string, int> item in playerStats) {
				//todo: check if object is active or not instead of try/except
				string statName = item.Key;//playerStats.Keys.ElementAt(i);
				float statValue = item.Value;
				stats += "\n" + statValue + "\t" + statName;

		}

		StatDisplay.GetComponent<Text>().text = stats;
		LaborDisplay.GetComponent<Text>().text = ""+GameObject.Find("GameControl").GetComponent<GameController>().Player.Labor;
		FundsDisplay.GetComponent<Text>().text = ""+GameObject.Find("GameControl").GetComponent<GameController>().Player.Funds;
	}



}
