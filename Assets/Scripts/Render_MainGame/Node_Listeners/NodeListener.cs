using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeListener : MonoBehaviour {

	// The ONLY piece of data copied and stored (for identification)
	public int idx;

	// Use this for initialization
	void Start () {
		Button purchase = gameObject.transform.Find("Purchase").GetComponent<Button>();
		purchase.onClick.AddListener(PurchaseNode);
		Button test = gameObject.transform.Find("Test").GetComponent<Button>();
		test.onClick.AddListener(TestNode);	
	}

	// Send purchase request and idx to GameController
	// Or Control Logic Here with understanding that it could be moved to GameController
	void PurchaseNode(){
		//todo: check if adequate funds exist
		//todo: subtract node cost from funds
		print(idx);
		// Mark Node as purchased
		GameObject.Find("GameControl").GetComponent<GameController>().NodeList[idx].Purchased = true;
		// Search for neighbors and mark as 'Visible' (handled in GameController.NodeNeighboorhoodCheck())
		GameObject.Find("GameControl").GetComponent<GameController>().NodeNeighborhoodCheck(idx);
	}

	// Method tests node. May result in either this node breaking, or other untested nodes.
	// todo
	void TestNode(){

	}
}
