using System.Collections;
using System;

[Serializable]
public class AttributeInfo {

	public int ID;
	public string name;
	public int value;
	public bool isActive;

	public AttributeInfo(int _ID, string _name, int _value, bool _isActive) {
		ID = _ID;
		name = _name;
		value = _value;
		isActive = _isActive;
	}

	public int getID() {
		return ID;
	}

	public string getName() {
		return name;
	}

	public int getValue() {
		return value;
	}

	public bool getIsActive() {
		return isActive;
	}

	public void UpdateInfo(string _name, int _value, bool _isActive) {
		name = _name;
		value = _value;
		isActive = _isActive;
	}
}
