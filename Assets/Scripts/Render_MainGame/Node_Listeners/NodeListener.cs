using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodeListener : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler{

	// The ONLY piece of data copied and stored (for identification)
	public int idx;
	public Button purchase; // the purchase button
	public Button test;  // the test button
	private bool Initialized = false;
	public Transform NodeInfoPos;

	// Use this for initialization
	void Start () {
		InitializeNode();
		//purchase = gameObject.transform.Find("Purchase").GetComponent<Button>();
		purchase.onClick.AddListener(PurchaseNode);
		//test = gameObject.transform.Find("Test").GetComponent<Button>();
		test.onClick.AddListener(TestNode);
		// Initailize NodeInfo
		string NodeInfo = "Node Info:";
		NodeInfo += "\nCost:\t" + GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].CostActual;
		NodeInfo += "\nFeature Information";
		int count = 0;
		foreach(string f in GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].ParameterNames){
			NodeInfo += "\n" + f + ":\t" + GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].ParameterActuals[count];
			count += 1;
		}
		NodeInfo += "\nTested:\t" + GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Tested;
		NodeInfo += "\nPurchased:\t" + GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Purchased;

		NodeInfoPos = gameObject.transform.Find("NodeInfo").transform;
		gameObject.transform.Find("NodeInfo").transform.localScale = new Vector3(0, 0, 0);
		gameObject.transform.Find("NodeInfo").GetComponent<TextMesh>().text = NodeInfo;

	}

	private bool isOver = false;

	public void OnPointerEnter(PointerEventData eventData)
	{
			Debug.Log("Mouse enter");
			gameObject.transform.Find("NodeInfo").transform.localScale = new Vector3(10,10,10);//NodeInfoPos.localScale;
			isOver = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
			Debug.Log("Mouse exit");
			gameObject.transform.Find("NodeInfo").transform.localScale = new Vector3(0, 0, 0);
			isOver = false;
	}

	// Send purchase request and idx to GameController
	// Or Control Logic Here with understanding that it could be moved to GameController
	void PurchaseNode(){
		// Use GameController convienence function to purchase node idx
		GameObject.Find("GameControl").GetComponent<GameController>().PurchaseNode(idx);
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

	// When "Test" button pressed, it may result in either this node breaking, or other untested nodes.
	public void TestNode(){
		List<GameController.NodeData> nodeList = GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList;

		foreach (GameController.NodeData eachNode in nodeList) {
			if (eachNode.Purchased) {
				// Purchased node condition
			}

			if (eachNode.Tested) {
				// Tested node condition
			}

			if (eachNode.Broken) {
				// Broken node condition
			}
		}
	}

}
