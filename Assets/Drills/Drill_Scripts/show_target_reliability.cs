using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class show_target_reliability : MonoBehaviour {

	public GameObject reliability_block;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<TextMesh>().text = "Target Reliability: " 
		+ reliability_block.GetComponent<reliability_calculator>().target_reliability 
		+ "\nTarget Cost: " + reliability_block.GetComponent<reliability_calculator>().target_cost;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
