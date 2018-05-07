using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndLoadScript : MonoBehaviour {

	public InputField userNameInputField;

	public void savedButtonPressed() {
		if (userNameInputField.text != "") {
			GameObject.Find("GameController").GetComponent<GameControllerScript>().playerInfo.UpdataNodeList (GameObject.Find("GameController").GetComponent<GameControllerScript>().nodeList);
			saved ();
		} else {
			Debug.Log ("Please enter the user name.");
		}
	}

	public void loadedButtonPressed() {

		if (userNameInputField.text != "") {
			loaded ();
		} else {
			Debug.Log ("Please enter the user name.");
		}
	}

	void saved() {
		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream savedFile = File.Create (Application.persistentDataPath + "/" + userNameInputField.text + ".dat");

		binaryFormatter.Serialize (savedFile, GameObject.Find("GameController").GetComponent<GameControllerScript>().playerInfo);
		savedFile.Close ();

		//Print Node Infos Start--------------------------------------
		Debug.Log ("----------");
		Debug.Log ("Nodes");
		foreach (NodeInfo node in GameObject.Find("GameController").GetComponent<GameControllerScript>().nodeList) {
			Debug.Log ("Node " + node.name);
			Debug.Log ("isHidden " + node.isHidden.ToString ());
			Debug.Log ("Children");

			if (node.childNodeName != null) {
				foreach (string name in node.childNodeName) {
					Debug.Log (name);
				}
			}

			Debug.Log ("----------");
		}
		//Print Node Infos End--------------------------------------

		Debug.Log ("File Saved!\n" + Application.persistentDataPath);
	}

	void loaded() {
		//Destroy the current nodes
		GameObject[] currentNodes = GameObject.FindGameObjectsWithTag ("Node");
		GameObject[] currentNodeConnections = GameObject.FindGameObjectsWithTag ("NodeConnection");

		Destroy (GameObject.Find("4 0 0"));

		for (int i = 0; i < currentNodes.Length; i++) {
			Destroy (currentNodes [i]);
		}

		for (int i = 0; i < currentNodeConnections.Length; i++) {
			Destroy (currentNodeConnections [i]);
		}

		//Loading the file
		if (File.Exists (Application.persistentDataPath + "/" + userNameInputField.text + ".dat")) {
			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream loadedFile = File.Open (Application.persistentDataPath + "/" + userNameInputField.text + ".dat", FileMode.Open);

			PlayerInfo data = (PlayerInfo)binaryFormatter.Deserialize (loadedFile);
			loadedFile.Close ();

			Debug.Log ("File Loaded!\n");
			for (int i = 0; i < data.playerNodeList.Count; i++) {
				Debug.Log (data.playerNodeList [i].ToString () + "\n");
			}
			GameObject.Find("GameController").GetComponent<GameControllerScript>().playerInfo = data;
			GameObject.Find("GameController").GetComponent<GameControllerScript>().nodeList = GameObject.Find("GameController").GetComponent<GameControllerScript>().playerInfo.playerNodeList;
		} else {
			Debug.Log ("Cannot find the use name");
		}

		GameUtilityScript.InstanceGameUtility.currentMode = "Manual";

		//Reload the node
		GameObject.Find("GameController").GetComponent<GameControllerScript>().Reload ();
	}
}