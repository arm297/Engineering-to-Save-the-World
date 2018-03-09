using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeListener : MonoBehaviour {

	// The ONLY piece of data copied and stored (for identification)
	public int idx;
	public Button purchase; // the purchase button
	public Button test;  // the test button
	private bool Initialized = false;

	// Use this for initialization
	void Start () {
		InitializeNode();
		//purchase = gameObject.transform.Find("Purchase").GetComponent<Button>();
		purchase.onClick.AddListener(PurchaseNode);
		//test = gameObject.transform.Find("Test").GetComponent<Button>();
		test.onClick.AddListener(TestNode);	
	}

	// Send purchase request and idx to GameController
	// Or Control Logic Here with understanding that it could be moved to GameController
	void PurchaseNode(){
		// Use GameController convienence function to purchase node idx
		GameObject.Find("GameControl").GetComponent<GameController>().PurchaseNode(idx);
	}

	// Method tests node. May result in either this node breaking, or other untested nodes.
	// todo
	void TestNode(){
		List<GameController.NodeData> nodeList = GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList;

		foreach (GameController.NodeData eachNode in nodeList) {
			if (eachNode.Purchased) {
				// Purchased condition
			}
		}
	}

	// Abstracted Initialization, as Start() will not run before end of MainGame_Renderer
	//  and as MainGame_Renderer requires buttons to be initialized prior to displaying buttons appropriately
	public void InitializeNode(){
		if(!Initialized){
			purchase = gameObject.transform.Find("Purchase").GetComponent<Button>();
			test = gameObject.transform.Find("Test").GetComponent<Button>();
			Initialized = true;
		}
	}

	// Hides Purchase Button
	public void HidePurchaseButton(){
		gameObject.transform.Find("Purchase").transform.localScale = new Vector3(0, 0, 0);
	}

	// Hides Test Button
	public void HideTestButton(){
		gameObject.transform.Find("Test").transform.localScale = new Vector3(0, 0, 0);
	}	

}
