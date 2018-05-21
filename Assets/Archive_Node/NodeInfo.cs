using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NodeInfo {

	//Node Identification
	public int direction; //N = 0, E = 1, S = 2, W = 3, Center = 4;
	public int level;
	public int order;
	public string name;

	//Node information
	public bool isHidden; //Hidden = 1 or true, Not Hidden = 0 or false;
	public bool isPurchased;
	public bool isLaborSet;
	public bool isNewAttribute;
	public bool isTested;

	public int laborSet;

	//Resources
	public bool isFinalNode;
	public int minimumFund;
	public int minimumLabor;
	public List<AttributeElementInfo> attributeElementInfoList;

	//Connection information
	public List<string> childNodeName;

	//Constructor
	public NodeInfo (int _direction, int _level, int _order, string _name, bool _isHidden) {
		direction = _direction;
		level = _level;
		order = _order;
		name = _name;
		isHidden = _isHidden;
		isPurchased = false;
		isLaborSet = false;
		isNewAttribute = true;
		laborSet = 0;
		childNodeName = new List<string> ();
		isFinalNode = false;
		isTested = false;
		minimumFund = 0;
		minimumLabor = 0;
		setNewAttributes ();
	}
	public NodeInfo (int _direction, int _level, int _order, bool _isHidden) {
		direction = _direction;
		level = _level;
		order = _order;
		name = _direction + " " + _level + " " + _order;
		isHidden = _isHidden;
		isPurchased = false;
		isLaborSet = false;
		isNewAttribute = true;
		laborSet = 0;
		childNodeName = new List<string> ();
		isFinalNode = false;
		isTested = false;
		minimumFund = 0;
		minimumLabor = 0;
		setNewAttributes ();
	}

	//Methods
	//set child nodes' names
	public void setChildNodeName(List<string> _childNodeName) {
		childNodeName = new List<string> (_childNodeName);
	}

	//add child on child node names
	public void addChildNodeName(string _name) {
		if(!childNodeName.Contains(_name)) {
			childNodeName.Add (_name);
		}
	}

	//change the child node name
	public void changeChildNodeName(string _orignalName, string _newName) {
		childNodeName.Remove (_orignalName);
		childNodeName.Add (_newName);
	}

	public void removeChildNodeName(string _name) {
		childNodeName.Remove (_name);
	}

	//set method for isLaborSet
	public void setisLaborSet (bool _isLaborSet) {
		isLaborSet = _isLaborSet;
	}


	//get method for isLaborSet
	public bool geisLaborSet () {
		return isLaborSet;
	}

	public string ToStringChildrenList() {
		string result = "";

		foreach (string child in childNodeName) {
			result += child + " ";
		}

		return result;
	}

	override
	public string ToString() {

		string directionStr = "";

		switch (direction) {
		case 0:
			directionStr = "North";
			break;
		case 1:
			directionStr = "East";
			break;
		case 2:
			directionStr = "South";
			break;
		case 3:
			directionStr = "West";
			break;
		default:
			directionStr = "Center";
			break;
		}

		return "Direction : " + directionStr + ", Level : " + level + ", Order : " + order + ", name: " + name;
	}

	//Attribute Controller
	public void setNewAttributes() {
		attributeElementInfoList = new List<AttributeElementInfo> ();
		List<AttributeInfo> attributeInfoList = GameControllerScript.InstanceGameCornoller.playerInfo.playerAttributes;
		foreach (AttributeInfo attributeInfo in attributeInfoList) {

			attributeElementInfoList.Add(new AttributeElementInfo(attributeInfo, 0, 0, 0));
		}
	}

	public void updateAttributes() {
		List<AttributeInfo> attributeInfoList = new List<AttributeInfo> (GameControllerScript.InstanceGameCornoller.playerInfo.playerAttributes);
		bool isRemain = false;
		List<AttributeElementInfo> tempAttributeElementInfoForRemoveList = new List<AttributeElementInfo> ();

		foreach (AttributeElementInfo tempAttributeElementInfo in attributeElementInfoList) {
			foreach (AttributeInfo tempAttributeInfo in attributeInfoList) {
				if (tempAttributeElementInfo.parentAttribute == tempAttributeInfo) {
					attributeInfoList.Remove (tempAttributeInfo);
					isRemain = true;
					break;
				}
			}
			if (!isRemain) {
				tempAttributeElementInfoForRemoveList.Add (tempAttributeElementInfo);
			}
			isRemain = false;
		}

		foreach (AttributeElementInfo tempAttributeElementInfoForRemove in tempAttributeElementInfoForRemoveList) {
			attributeElementInfoList.Remove (tempAttributeElementInfoForRemove);
		}

		AttributeInfo tempParentAttributeInfo = null;

		foreach (AttributeInfo tempAttributeInfo in attributeInfoList) {
			foreach (AttributeInfo tempAttributeInfoFromProfile in GameControllerScript.InstanceGameCornoller.playerInfo.playerAttributes) {
				if (tempAttributeInfo == tempAttributeInfoFromProfile) {
					tempParentAttributeInfo = tempAttributeInfoFromProfile;
					break;
				}
			}
			attributeElementInfoList.Add (new AttributeElementInfo (tempParentAttributeInfo, 0, 0, 0));
			tempParentAttributeInfo = null;
		}
	}
}