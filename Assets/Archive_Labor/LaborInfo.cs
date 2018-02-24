using System.Collections.Generic;
using System;

[Serializable]
public class LaborInfo{

	private string name;
	private int level;

	public LaborInfo(string _name, int _level) {
		name = _name;
		level = _level;
	}
}
