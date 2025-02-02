﻿using System.Collections.Generic;
using System;

[Serializable]
public class PlayerInfo {

	public string name;
	public int fund;
	public int fundTurn;
	public int labor;
	public List<NodeInfo> playerNodeList = new List<NodeInfo> ();
	public List<AttributeInfo> playerAttributes = new List<AttributeInfo> ();

	public PlayerInfo (int _fund, int _fundTurn, int _labor) {
		name = "Sample";
		fund = _fund;
		fundTurn = _fundTurn;
		labor = _labor;
		playerAttributes.Add (new AttributeInfo(11111, "Attribute 1", 1, true));
	}

	public PlayerInfo (string _name, int _fund, int _fundTurn, int _labor) {
		name = _name;
		fund = _fund;
		fundTurn = _fundTurn;
		labor = _labor;
		playerAttributes.Add (new AttributeInfo(11111, "Attribute 1", 1, true));
	}
		
	public void UpdataNodeList(List<NodeInfo> _playerNodeList) {
		playerNodeList = _playerNodeList;
	}
}