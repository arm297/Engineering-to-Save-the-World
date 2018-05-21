using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUtilityScript : MonoBehaviour {

	public static GameUtilityScript InstanceGameUtility;

	public string currentMode;
	public Text MessageBox;
	public Text Score_Min;
	public Text Score_Max;
	public Button TestButton;
	public GameObject profileSetting;
	public GameObject resourceLabel;
	public Transform resourceElementGroup;

	private int testPrice;
	private string nodeName;
	private Vector3 pos;
	private NodeInfo nodeInfo;
	private GameObject newProfileSetting;
	private GameObject newResourceElement;
	private GameObject[] ManualGroups;
	private List<GameObject> TestNodes = new List<GameObject> ();
	private List<AttributeInfo> playerAttributeList = new List<AttributeInfo>();

	private string MESSAGE_SPACE = " ";
	private string MESSAGE_NOTENOUGHFUND = "Not enough fund";
	private string MESSAGE_FORTEST = "for testing";

	void Start () {
		InstanceGameUtility = this;
		currentMode = "manual";

		//Presetting
		updatePlayerAttributeList();
		buildResourceLabel ();
		ResetScore ();
	}

	public void ResetScore() {
		Score_Min.text = "0";
		Score_Max.text = "0";
	}

	public void updatePlayerAttributeList() {
		playerAttributeList = GameControllerScript.InstanceGameCornoller.playerInfo.playerAttributes;
	}

	public void buildResourceLabel() {
		foreach (AttributeInfo attributeInfo in playerAttributeList) {
			newResourceElement = Instantiate (resourceLabel, new Vector3 (0, 0, -60), Quaternion.identity, resourceElementGroup) as GameObject;
			newResourceElement.name = attributeInfo.getName ();
			newResourceElement.GetComponent<ResourceLabelScript> ().UpdateLabel (attributeInfo);
		}
	}

	public void updateResourceElement() {
		GameObject[] currentResourceLabels = GameObject.FindGameObjectsWithTag ("ResourceLabel");
		for (int i = 0; i < currentResourceLabels.Length; i++) {
			Destroy (currentResourceLabels [i]);
		}

		updatePlayerAttributeList();
		buildResourceLabel ();
		testButtonUpdate ();
	}

	public void pressModeChangeButton () {
		if (currentMode == "game") {

			//Show all Node
			GameObject[] currentNodes = GameObject.FindGameObjectsWithTag ("Node");
			GameObject[] currentNodeConnections = GameObject.FindGameObjectsWithTag ("NodeConnection");

			Destroy (GameObject.Find("4 0 0"));

			for (int i = 0; i < currentNodes.Length; i++) {
				Destroy (currentNodes [i]);
			}

			for (int i = 0; i < currentNodeConnections.Length; i++) {
				Destroy (currentNodeConnections [i]);
			}

			GameControllerScript.InstanceGameCornoller.preForNodeGeneration ();
			GameControllerScript.InstanceGameCornoller.generateNodeTree (true);

			ManualGroups = GameObject.FindGameObjectsWithTag ("ManualGroup");

			foreach (GameObject ManualGroup in ManualGroups) {
				pos = ManualGroup.transform.position;
				pos.z = -50;
				ManualGroup.transform.position = pos;
			}

			currentMode = "manual";
		} else {

			//Show only non-hidden Node
			GameObject[] currentNodes = GameObject.FindGameObjectsWithTag ("Node");
			GameObject[] currentNodeConnections = GameObject.FindGameObjectsWithTag ("NodeConnection");

			Destroy (GameObject.Find("4 0 0"));

			for (int i = 0; i < currentNodes.Length; i++) {
				Destroy (currentNodes [i]);
			}

			for (int i = 0; i < currentNodeConnections.Length; i++) {
				Destroy (currentNodeConnections [i]);
			}

			GameControllerScript.InstanceGameCornoller.preForNodeGeneration ();
			GameControllerScript.InstanceGameCornoller.generateNodeTree (false);

			ManualGroups = GameObject.FindGameObjectsWithTag ("ManualGroup");

			foreach (GameObject ManualGroup in ManualGroups) {
				pos = ManualGroup.transform.position;
				pos.z = -400;
				ManualGroup.transform.position = pos;
			}

			currentMode = "game";
		}
	}

	public void pressProfileSettingButton() {
		newProfileSetting = Instantiate (profileSetting, new Vector3 (0, 0, -60), Quaternion.identity, null) as GameObject;
		newProfileSetting.name = "ProfileSetting";
	}

	public void pressHireLaborButton() {
		GameControllerScript.InstanceGameCornoller.playerInfo.fundTurn -= 1;
		GameControllerScript.InstanceGameCornoller.playerInfo.labor += 1;

		GameControllerScript.InstanceGameCornoller.updateFundAndLaborGC ();
	}

	public void pressFireLaborButton() {
		GameControllerScript.InstanceGameCornoller.playerInfo.fund -= 10;
		GameControllerScript.InstanceGameCornoller.playerInfo.fundTurn += 1;
		GameControllerScript.InstanceGameCornoller.playerInfo.labor -= 1;

		GameControllerScript.InstanceGameCornoller.updateFundAndLaborGC ();
	}

	public void pressTestButton() {
		if (testPrice <= GameControllerScript.InstanceGameCornoller.GetCurrentFund ()) {
			GameControllerScript.InstanceGameCornoller.SubstractFund (testPrice);
			foreach (GameObject eachNode in TestNodes) {
				eachNode.GetComponent<NewNodeScript> ().getNodeInfo ().isTested = true;
			}
			updateResourceElement ();
		} else {
			MessageBoxUpdate (MESSAGE_NOTENOUGHFUND + MESSAGE_SPACE + MESSAGE_FORTEST);
		}
	}

	public void testButtonUpdate() {
		testPrice = 0;

		GameObject[] AllNodes = GameObject.FindGameObjectsWithTag ("Node");
		TestNodes = new List<GameObject> ();
		NodeInfo eachNodeInfo;

		foreach (GameObject eachNode in AllNodes) {
			eachNodeInfo = eachNode.GetComponent<NewNodeScript> ().getNodeInfo ();
			if (eachNodeInfo.isLaborSet && (!eachNodeInfo.isTested)) {
				TestNodes.Add (eachNode);
			}
		}

		testPrice = TestNodes.Count * 10;

		TestButton.GetComponentInChildren<Text> ().text = "Test Price: " + testPrice.ToString ();;
	}

	public void MessageBoxUpdate(string message) {
		MessageBox.text = message;
	}

	public void UpdateScore() {
		int minScore = 0, maxScore = 0;

		GameObject[] currentResourceLabels = GameObject.FindGameObjectsWithTag ("ResourceLabel");
		foreach (GameObject eachCurrentResourceLabels in currentResourceLabels) {
			MessageBoxUpdate (eachCurrentResourceLabels.GetComponent<ResourceLabelScript> ().GetWeight ().ToString());
			minScore += (eachCurrentResourceLabels.GetComponent<ResourceLabelScript> ().GetMinValue () * eachCurrentResourceLabels.GetComponent<ResourceLabelScript> ().GetWeight ());
			maxScore += (eachCurrentResourceLabels.GetComponent<ResourceLabelScript> ().GetMaxValue () * eachCurrentResourceLabels.GetComponent<ResourceLabelScript> ().GetWeight ());
			Debug.Log (eachCurrentResourceLabels.GetComponent<ResourceLabelScript> ().GetMinValue ().ToString ());
			Debug.Log (eachCurrentResourceLabels.GetComponent<ResourceLabelScript> ().GetMaxValue ().ToString ());
		}
		Debug.Log (minScore.ToString ());
		Debug.Log (maxScore.ToString ());

		Score_Min.text = minScore.ToString ();
		Score_Max.text = maxScore.ToString ();
	}
}