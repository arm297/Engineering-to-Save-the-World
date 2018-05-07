using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurnScript : MonoBehaviour {

	public void pressNextTurnButton() {
		PlayerInfo currentPlayer = GameObject.Find("GameController").GetComponent<GameControllerScript>().playerInfo;

		currentPlayer.fund += currentPlayer.fundTurn;
		currentPlayer.labor = 20;

		GameObject.Find("GameController").GetComponent<GameControllerScript>().updateFundAndLaborGC ();
	}
}
