using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

<<<<<<< ec09ac56e94f15e760085bb46990a110a8f08d47
public class NodeListener : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{

=======
public class NodeListener : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
	 {
>>>>>>> gui elements
	// The ONLY piece of data copied and stored (for identification)
	public int idx;
	public Button purchase; // the purchase button
	public Button test;  // the test button
	private bool Initialized = false;
	public Transform NodeInfoPos;

	// To control aesthetics of Purchase button
	public Sprite PurchaseButtonMouseOver;
	public Sprite PurchaseButtonNormal;
	public Sprite PurchaseButtonBought;

<<<<<<< ec09ac56e94f15e760085bb46990a110a8f08d47
	public Sprite TestReadyButtonMouseOver;
	public Sprite TestReadyButtonNormal;
	public Sprite TestReadyButtonTested;

	// Use this for initialization
	void Start () {
=======
	// To Control NodeTypePanel
	public GameObject LaborCost;
	public GameObject FundsCost;
	public GameObject Purchased;
	public GameObject Tested;


	// Use this for initialization
	void Start () {
		if(idx >= 0 && idx < GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList.Count ){
>>>>>>> gui elements
		InitializeNode();
		//purchase = gameObject.transform.Find("Purchase").GetComponent<Button>();
		purchase.onClick.AddListener(PurchaseNode);
		//test = gameObject.transform.Find("Test").GetComponent<Button>();
		test.onClick.AddListener(TestNode);
<<<<<<< ec09ac56e94f15e760085bb46990a110a8f08d47
		// Initailize NodeInfo
		UpdateNodeInfo();
		gameObject.transform.Find("NodeInfo").transform.localScale = new Vector3(0, 0, 0);
		//gameObject.transform.Find("NodeInfo").GetComponent<TextMesh>().text = NodeInfo;

=======

		// Initailize NodeInfo
		FundsCost.GetComponent<Text>().text = ""+Mathf.Round(GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].CostActual);
		LaborCost.GetComponent<Text> ().text = ""+Mathf.Round(GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].LaborCost);
		Purchased.GetComponent<Text> ().text = ""+GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Purchased;
		Tested.GetComponent<Text> ().text = ""+GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Tested;
		NodeInfoPos = gameObject.transform.Find("NodeInfoPanel").transform;
		gameObject.transform.Find("NodeInfoPanel").transform.localScale = new Vector3(0, 0, 0);

		/*
		string NodeInfo = "";
		NodeInfo += "Cost:\t\t\t\t\t" + Mathf.Round(100*GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].CostActual)/100;
		NodeInfo += "";
		int count = 0;
		foreach(string f in GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].ParameterNames){
			NodeInfo += "\n" + f + ":\t\t" + Mathf.Round(100*GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].ParameterActuals[count])/100;
			count += 1;
		}
		NodeInfo += "\nTested:\t\t\t\t" + GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Tested;
		NodeInfo += "\nPurchased:\t\t" + GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Purchased;

		NodeInfoPos = gameObject.transform.Find("NodeInfo").transform;
		gameObject.transform.Find("NodeInfoPanel").transform.localScale = new Vector3(0, 0, 0);
		gameObject.transform.Find ("NodeInfo").GetComponent<Text> ().text = NodeInfo;
		//gameObject.transform.Find("NodeInfo").GetComponent<TextMesh>().text = NodeInfo;
		*/
>>>>>>> gui elements
		if(GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Obscured){
			HidePurchaseButton();
		}
	}
<<<<<<< ec09ac56e94f15e760085bb46990a110a8f08d47
		

	private bool isOver = false;

	public void OnPointerEnter(PointerEventData eventData)
	{
		//Debug.Log("Mouse enter");
		if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Visible
			&& !GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Obscured) {

			gameObject.transform.Find("NodeInfo").transform.localScale = new Vector3(10,10,10);//NodeInfoPos.localScale;
			isOver = true;
		}
		if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Purchaseable) {
			gameObject.transform.Find("Purchase").GetComponent<Image>().sprite = PurchaseButtonMouseOver;
		}
		else if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList [idx].Purchased
		    && GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList [idx].Testable) {
			gameObject.transform.Find ("Purchase").GetComponent<Image> ().sprite = TestReadyButtonMouseOver;
		}
		UpdateNodeInfo();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//Debug.Log("Mouse exit");
		gameObject.transform.Find("NodeInfo").transform.localScale = new Vector3(0, 0, 0);
		isOver = false;

		if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Purchaseable) {
			gameObject.transform.Find("Purchase").GetComponent<Image>().sprite = PurchaseButtonNormal;
		}

		else if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList [idx].Purchased
			&& GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList [idx].Testable) {
			gameObject.transform.Find ("Purchase").GetComponent<Image> ().sprite = TestReadyButtonNormal;
		}
		UpdateNodeInfo();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		// Purchase?
		if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Purchaseable) {
			gameObject.transform.Find("Purchase").GetComponent<Image>().sprite = TestReadyButtonNormal;
			PurchaseNode();
		} else if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList [idx].Purchased
			&& GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList [idx].Testable) {
			Debug.Log ("Change Test Status");
			if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList [idx].TestReady == false) {
				Debug.Log ("Testing ready");
				gameObject.transform.Find ("Purchase").GetComponent<Image> ().sprite = TestReadyButtonTested;
				GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList [idx].TestReady = true;
			} else {
				Debug.Log ("Not testing ready");
				gameObject.transform.Find ("Purchase").GetComponent<Image> ().sprite = TestReadyButtonNormal;
				GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList [idx].TestReady = false;
			}
		}
		UpdateNodeInfo();
	}

	void UpdateNodeInfo() {
		string NodeInfo = "";
		NodeInfo += "Cost:\t\t\t\t\t" + Mathf.Round(100*GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].CostActual)/100;
		NodeInfo += "";
		int count = 0;
		foreach(string f in GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].ParameterNames){
			NodeInfo += "\n" + f + ":\t\t" + Mathf.Round(100*GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].ParameterActuals[count])/100;
			count += 1;
		}
		NodeInfo += "\nTested:\t\t\t\t" + GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Tested;
		NodeInfo += "\nTestReady:\t\t\t" + GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].TestReady;
		NodeInfo += "\nTestable:\t\t\t" + GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Testable;
		NodeInfo += "\nPurchased:\t\t" + GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Purchased;

		NodeInfoPos = gameObject.transform.Find("NodeInfo").transform;
		gameObject.transform.Find ("NodeInfo").GetComponent<Text> ().text = NodeInfo;
	}

=======
	}

	private bool isOver = false;

	void OnMouseEnter()
	{
			Debug.Log("Entered");
	}


	public void OnPointerDown( PointerEventData eventData )
	{
	}

	public void OnPointerUp( PointerEventData eventData )
	{
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
					Debug.Log("Entered");
			//Debug.Log("Mouse enter");
			if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Visible
			&& !GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Obscured){
				gameObject.transform.Find("NodeInfoPanel").transform.localScale = new Vector3(2,2,2);//NodeInfoPos.localScale;
				isOver = true;
			}
			if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Purchaseable){
				//gameObject.transform.Find("Purchase").GetComponent<Image>().sprite = PurchaseButtonMouseOver;
			}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
			//Debug.Log("Mouse exit");
			gameObject.transform.Find("NodeInfoPanel").transform.localScale = new Vector3(0, 0, 0);
			isOver = false;

			if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Purchaseable){
				//gameObject.transform.Find("Purchase").GetComponent<Image>().sprite = PurchaseButtonNormal;
			}
	}

	public void OnPointerClick(PointerEventData eventData)
	    {

				// Purchase?
				if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Purchaseable){
  				//gameObject.transform.Find("Purchase").GetComponent<Image>().sprite = PurchaseButtonBought;
					PurchaseNode();
  			}
	   }

>>>>>>> gui elements

	// Send purchase request and idx to GameController
	// Or Control Logic Here with understanding that it could be moved to GameController
	void PurchaseNode(){
		// Use GameController convienence function to purchase node idx
		Debug.Log("Purchasing Node");
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
