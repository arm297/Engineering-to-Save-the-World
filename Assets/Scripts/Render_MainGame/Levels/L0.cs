using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
	This class offers an example of creating a level for MainGame.
	Step 1: Create Nodes
	Step 2: Specify Connections
	Step 3: Specify Initial Visibility
	Step 4: Alter Node Befefits
	Step 5: Set System Required Nodes
*/
public class L0 : MonoBehaviour {

	// This is the List of Nodes (i.e. the map) for MainGame
	public List<NodeData> NodeList_L0 = new List<NodeData>();

	// This method will populate the map
	public void PopulateMap(){
		// Step 1: Create Nodes
		// In this Example we will create 5 Nodes
		NodeData n1 = new NodeData(0, 0, 10f, 3f);
		NodeData n2 = new NodeData(1, 1, 7f, 5f);
		NodeData n3 = new NodeData(3, 1, 11f, 2f);
		NodeData n4 = new NodeData(6, 0, 12f, 3f);
		NodeData n5 = new NodeData(6, 2, 13f, 7f);
		// Specify index of nodes (this should match order in eventual NodeList)
		n1.IDX = 0;
		n2.IDX = 1;
		n3.IDX = 2;
		n4.IDX = 3;
		n5.IDX = 4;

		// Step 2: Specify Connections
		n1.Children.Add(n2.IDX);
		n2.Parents.Add(n1.IDX);

		n2.Children.Add(n3.IDX);
		n3.Parents.Add(n2.IDX);

		n3.Children.Add(n4.IDX);
		n3.Children.Add(n5.IDX);
		n4.RequiredParents.Add(n3.IDX);
		n5.Parents.Add(n3.IDX);

		n4.RequiredParents.Add(n5.IDX);
		n5.Children.Add(n4.IDX);

		// Step 3: Specify initial visibility
		n1.Visible = true;
		n1.Purchaseable = true;
		n1.Obscured = false;

		n2.Visible = false;
		n2.Purchaseable = false;
		n2.Obscured = true;

		n3.Visible = false;
		n3.Purchaseable = false;
		n3.Obscured = false;

		n4.Visible = false;
		n4.Purchaseable = false;
		n4.Obscured = false;

		n5.Visible = false;
		n5.Purchaseable = false;
		n5.Obscured = false;

		// Step 4: Alter Node Benefits
		n4.ParameterActuals = new List<float>{
				15f, 0f, 12f, 9f
		};

		n4.ParameterEstimated = new List<float>{
				12f, 5f, 13f, 0f
		};

		n4.ParameterNames = new List<string>{
				"Detection", "Defense", "Strike Capability", "Manueverability"
		};

		// Step 5: Set Syst Reqs
		n4.SystReq = true;

		// Finish by adding nodes to list
		NodeList_L0.Add(n1);
		NodeList_L0.Add(n2);
		NodeList_L0.Add(n3);
		NodeList_L0.Add(n4);
		NodeList_L0.Add(n5);
	}


}
