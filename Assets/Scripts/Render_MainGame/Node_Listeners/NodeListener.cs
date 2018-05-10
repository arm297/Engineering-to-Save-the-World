using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodeListener : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
	 {
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

	// To Control NodeTypePanel
	public GameObject LaborCost;
	public GameObject FundsCost;
	public GameObject Purchased;
	public GameObject Tested;

	public AudioSource PurchaseSound;

	// Use this for initialization
	void Start () {
		if(idx >= 0 && idx < GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList.Count ){
		InitializeNode();
		//purchase = gameObject.transform.Find("Purchase").GetComponent<Button>();
		purchase.onClick.AddListener(PurchaseNode);
		//test = gameObject.transform.Find("Test").GetComponent<Button>();
		test.onClick.AddListener(TestNode);

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
		if(GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Obscured){
			HidePurchaseButton();
		}
	}
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


	// Send purchase request and idx to GameController
	// Or Control Logic Here with understanding that it could be moved to GameController
	void PurchaseNode(){
		Debug.Log("Playing Sound");
		PurchaseSound.Play();


		// Use GameController convienence function to purchase node idx
		Debug.Log("Purchasing Node");
		string return_note = GameObject.Find("GameControl").GetComponent<GameController>().PurchaseNode(idx);

		//if insufficient funds, recommend next Turn
		if(return_note == "Insufficient Funds"){
			GameObject.Find("Render_MainGame").GetComponent<MainGame_Renderer>().HideUnHideNextTurnPayAttention(true);
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
