using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class reliability_calculator : MonoBehaviour {

	private double reliability = 0;
	private GameObject[] nodes;
	private double[] rnodes;
	private int[] blocks;
	private bool IsCalculatable = false;
	public double permissable_error = .0001;
	public bool TargetReached = false; // go to true when reliability is within permissable_error of target
	public double target_reliability = .97;
	public double target_cost = 15;
	public int level_id; // if level 1, then 1...different reliability calcs for each level

	// Use this for initialization
	void Start () {
		nodes = GameObject.FindGameObjectsWithTag("snap_into");
		rnodes = new double[nodes.Length];
		blocks = new int[nodes.Length];
		for(int i = 0; i < nodes.Length; i++){
			blocks[i] = nodes[i].GetComponent<drag_drop>().block;
		}
	}
	
	// Update is called once per frame
	void Update () {
		double price = 0; // running sum of all blocks used
		IsCalculatable = true;
		for(int i = 0; i < nodes.Length; i++){
			if(nodes[i].GetComponent<drag_drop>().ContainsBlock){
				//get reliability
				rnodes[i] = nodes[i].GetComponent<drag_drop>().reliability;
				price += nodes[i].GetComponent<drag_drop>().cost;
			}else{
				IsCalculatable = false;
				
				//Fill in data with 1 reliability and 0 cost to calculate partial performance
				rnodes[i] = 1;
				price += 0;
				
				//break;
			}
		}



		//calculate
		//if(IsCalculatable){
		if(1==1){
			if(level_id == 10){
				int[] i1_through_i15 = new int[15];
				for(int i = 1; i < 16; i++){
					i1_through_i15[i-1] = Array.IndexOf(blocks,i);
					if(i1_through_i15[i-1] < 0){
						//block i not found
						print("not found:" + i);
					}
				}
				double[] rs = new double[15];
				for(int i = 1; i < 16; i++){
					rs[i-1] = rnodes[i1_through_i15[i-1]];
				}
				double R = rs[0]*rs[12]*rs[10]* (1-(1-rs[7]*rs[8])*(1-rs[13]*rs[14])*rs[9]*rs[11])*(1-rs[1]*(1-rs[2]*rs[3])*(1-rs[6]*rs[7])*rs[4]);
				reliability = R;
				gameObject.GetComponent<TextMesh>().text = "Reliability: " +Math.Round(reliability,3) + "\nCost: "+price;
				if(reliability - target_reliability > -permissable_error && target_cost >= price){
					TargetReached = true;
				}
			}else if(level_id == 1){
				int[] i1_through_i8 = new int[8];
				for(int i = 1; i < 9; i++){
					i1_through_i8[i-1] = Array.IndexOf(blocks,i);
					if(i1_through_i8[i-1] < 0){
						//block i not found
						print("not found:" + i);
					}
				}
				double[] rs = new double[8];
				for(int i = 1; i < 9; i++){
					rs[i-1] = rnodes[i1_through_i8[i-1]];
				}
				double R = rs[0]*rs[7]*(1-(1-rs[1])*(1-rs[4]))*(1-(1-rs[2])*(1-rs[5]))*(1-(1-rs[3])*(1-rs[6]));//   rs[0]*rs[12]*rs[10]* (1-(1-rs[7]*rs[8])*(1-rs[13]*rs[14])*rs[9]*rs[11])*(1-rs[1]*(1-rs[2]*rs[3])*(1-rs[6]*rs[7])*rs[4]);
				reliability = R;
				gameObject.GetComponent<TextMesh>().text = "Reliability: " +Math.Round(reliability,3) + "\nCost: "+price;
				if(reliability - target_reliability > -permissable_error && target_cost >= price){
					TargetReached = true;
				}
			}else if(level_id == 2){
				int[] i1_through_i11 = new int[11];
				for(int i = 1; i < 12; i++){
					i1_through_i11[i-1] = Array.IndexOf(blocks,i);
					if(i1_through_i11[i-1] < 0){
						//block i not found
						print("not found:" + i);
					}
				}
				double[] rs = new double[11];
				for(int i = 1; i < 12; i++){
					rs[i-1] = rnodes[i1_through_i11[i-1]];
				}
				double R = rs[0]*rs[7]*(1-(1-rs[1])*(1-rs[4])*(1-rs[8]))*(1-(1-rs[2])*(1-rs[5])*(1-rs[9]))*(1-(1-rs[3])*(1-rs[6])*(1-rs[10]));//   rs[0]*rs[12]*rs[10]* (1-(1-rs[7]*rs[8])*(1-rs[13]*rs[14])*rs[9]*rs[11])*(1-rs[1]*(1-rs[2]*rs[3])*(1-rs[6]*rs[7])*rs[4]);
				reliability = R;
				gameObject.GetComponent<TextMesh>().text = "Reliability: " +Math.Round(reliability,3) + "\nCost: "+price;
				if(reliability - target_reliability > -permissable_error && target_cost >= price){
					TargetReached = true;
				}
			}

		}
		
		if(!IsCalculatable){
			//gameObject.GetComponent<TextMesh>().text = "Incomplete.";
			gameObject.GetComponent<TextMesh>().text = "Incomplete Reliability: " +Math.Round(reliability,3) + "\n Incomplete Cost: "+price;
			TargetReached = false;
		}
		
	}
}
