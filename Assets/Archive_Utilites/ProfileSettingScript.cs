using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSettingScript : MonoBehaviour {

	public static ProfileSettingScript Instance;
	public InputField nameInputField;
	public InputField fundInputField;
	public InputField laborInputField;
	public Transform attributesGroupSheild;
	public GameObject attributeElement;

	private List<AttributeInfo> playerAttributeList = new List<AttributeInfo>();
	private Transform currentSettingBox;
	private Transform newAttributeElement;
	private PlayerInfo currentPlayerInfo;
	private List<GameObject> attributeGameObjectList;

	void Start () {
		Instance = this;
		attributeGameObjectList = new List<GameObject> ();
		currentPlayerInfo = GameObject.Find("GameController").GetComponent<GameControllerScript>().playerInfo;
		playerAttributeList = currentPlayerInfo.playerAttributes;

		nameInputField.text = currentPlayerInfo.name;
		fundInputField.text = currentPlayerInfo.fund.ToString();
		laborInputField.text = currentPlayerInfo.labor.ToString();

		(attributesGroupSheild as RectTransform).sizeDelta = new Vector2 (200, playerAttributeList.Count * 25 + 5);

		foreach (AttributeInfo tempAttributeElement in playerAttributeList) {
			newAttributeElement = Instantiate (attributeElement.transform, new Vector3(0, 0, 0), Quaternion.identity, attributesGroupSheild);
			newAttributeElement.name = playerAttributeList.IndexOf(tempAttributeElement) + " Attribute";
			newAttributeElement.GetComponent<AttributeElementScript> ().setInputField (tempAttributeElement.getName(), tempAttributeElement.getValue(), tempAttributeElement.getIsActive());
			newAttributeElement.GetComponent<AttributeElementScript> ().setID (tempAttributeElement.getID ());
		}
	}

	public void pressCancelButton() {
		currentSettingBox = transform;

		Destroy (currentSettingBox.gameObject);
	}

	public void pressApplyButton() {
		currentSettingBox = transform;

		currentPlayerInfo.name = nameInputField.text;
		currentPlayerInfo.fund = int.Parse (fundInputField.text);
		currentPlayerInfo.labor = int.Parse (laborInputField.text);

		attributeGameObjectList = new List<GameObject> (GameObject.FindGameObjectsWithTag("AttributeElement"));

		bool isRemain = false;
		List<AttributeInfo> tempAttributeInfoForRemoveList = new List<AttributeInfo> ();

		foreach (AttributeInfo tempAttributeElement in playerAttributeList) {
			foreach (GameObject attributeGameObject in attributeGameObjectList) {
				if (tempAttributeElement.getID () == attributeGameObject.GetComponent<AttributeElementScript> ().getID ()) {
					tempAttributeElement.UpdateInfo (attributeGameObject.GetComponent<AttributeElementScript> ().getName (), attributeGameObject.GetComponent<AttributeElementScript> ().getValue (), attributeGameObject.GetComponent<AttributeElementScript> ().getIsActive ());
					attributeGameObjectList.Remove (attributeGameObject);
					isRemain = true;
					break;
				}
			}
			if (!isRemain) {
				tempAttributeInfoForRemoveList.Add (tempAttributeElement);
			}
			isRemain = false;
		}

		foreach (AttributeInfo tempAttributeInfoForRemove in tempAttributeInfoForRemoveList) {
			playerAttributeList.Remove (tempAttributeInfoForRemove);
		}

		foreach (GameObject attributeGameObject in attributeGameObjectList) {
			playerAttributeList.Add (new AttributeInfo (attributeGameObject.GetComponent<AttributeElementScript>().getID(), attributeGameObject.GetComponent<AttributeElementScript>().getName(), attributeGameObject.GetComponent<AttributeElementScript>().getValue(), attributeGameObject.GetComponent<AttributeElementScript>().getIsActive()));
		}

		foreach (NodeInfo tempNodeInfo in GameObject.Find("GameController").GetComponent<GameControllerScript>().nodeList) {
			tempNodeInfo.updateAttributes ();
		}

		GameUtilityScript.InstanceGameUtility.updateResourceElement ();
		GameUtilityScript.InstanceGameUtility.UpdateScore();

		Destroy (currentSettingBox.gameObject);
	}

	public void pressAddAttributeButton() {
		attributeGameObjectList = new List<GameObject> (GameObject.FindGameObjectsWithTag("AttributeElement"));

		newAttributeElement = Instantiate (attributeElement.transform, new Vector3(0, 0, 0), Quaternion.identity, attributesGroupSheild);
		newAttributeElement.name = "Attribute" + attributeGameObjectList.Count;
		newAttributeElement.GetComponent<AttributeElementScript> ().setInputField ("New", 0, true);
		newAttributeElement.GetComponent<AttributeElementScript> ().setNewID ();

		attributeGameObjectList = new List<GameObject> (GameObject.FindGameObjectsWithTag("AttributeElement"));
		(attributesGroupSheild as RectTransform).sizeDelta = new Vector2 (200, attributeGameObjectList.Count * 25 + 5);
	}
}