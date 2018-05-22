using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

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

	public Sprite TestReadyButtonMouseOver;
	public Sprite TestReadyButtonNormal;
	public Sprite TestReadyButtonTested;

	// To Control NodeTypePanel
	public GameObject LaborCost;
	public GameObject FundsCost;
	public GameObject Purchased;
	public GameObject Tested;
	public List<GameObject> Feautures = new List<GameObject>();

	public AudioSource PurchaseSound;
	public int PurchaseSoundEffect = 0; // idx of purchase soundeffect from audiocontroller
	public int FailedSoundEffect = 1; // idx of failed soundeffect from audiocontroller

	// Use this for initialization
	void Start () {
		if(idx >= 0 && idx < GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList.Count ){
			InitializeNode();
			//purchase = gameObject.transform.Find("Purchase").GetComponent<Button>();
			purchase.onClick.AddListener(PurchaseNode);
			//test = gameObject.transform.Find("Test").GetComponent<Button>();
			test.onClick.AddListener(TestNode);

			// Initailize NodeInfo
			UpdateNodeInfo ();
			NodeInfoPos = gameObject.transform.Find("NodeInfoPanel").transform;
			gameObject.transform.Find("NodeInfoPanel").transform.localScale = new Vector3(0, 0, 0);

			if(GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Obscured) {
				HidePurchaseButton();
			}
		}
	}

	void Update() {
		UpdateNodeInfo ();
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
		//Debug.Log("Mouse enter");
		if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Visible
			&& !GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Obscured) {
			Debug.Log ("Show info");
			gameObject.transform.Find("NodeInfoPanel").transform.localScale = new Vector3(2, 2, 2);;//NodeInfoPos.localScale;
			isOver = true;
		}
		if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Purchaseable) {
			//gameObject.transform.Find("Purchase").GetComponent<Image>().sprite = PurchaseButtonMouseOver;
		}
		else if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList [idx].Purchased
			&& GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList [idx].Testable) {
			//gameObject.transform.Find ("Purchase").GetComponent<Image> ().sprite = TestReadyButtonMouseOver;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//Debug.Log("Mouse exit");
		gameObject.transform.Find("NodeInfoPanel").transform.localScale = new Vector3(0, 0, 0);
		isOver = false;

		if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Purchaseable) {
			//gameObject.transform.Find("Purchase").GetComponent<Image>().sprite = PurchaseButtonNormal;
		}

		else if (GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList [idx].Purchased
			&& GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList [idx].Testable) {
			gameObject.transform.Find ("Purchase").GetComponent<Image> ().sprite = TestReadyButtonNormal;
		}
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
	}


	// Send purchase request and idx to GameController
	// Or Control Logic Here with understanding that it could be moved to GameController
	// Send purchase request and idx to GameController
	// Or Control Logic Here with understanding that it could be moved to GameController
	public void PurchaseNode(){

		// Use GameController convienence function to purchase node idx
		Debug.Log("Can we purchase node?");
		GameObject.Find("GameMusic").GetComponent<AudioController>().SoundEffect = PurchaseSoundEffect;
		string return_note = GameObject.Find("GameControl").GetComponent<GameController>().PurchaseNode(idx);

		//if insufficient funds, recommend next Turn
		if(return_note == "Insufficient Funds"){
			GameObject.Find("Render_MainGame").GetComponent<MainGame_Renderer>().HideUnHideNextTurnPayAttention(true);
			GameObject.Find("GameMusic").GetComponent<AudioController>().SoundEffect = FailedSoundEffect;
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
		List<NodeData> nodeList = GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList;

		foreach (NodeData eachNode in nodeList) {
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

	void UpdateNodeInfo() {
		FundsCost.GetComponent<Text>().text = ""+Mathf.Round(GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].CostActual);
		LaborCost.GetComponent<Text> ().text = ""+Mathf.Round(GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].LaborCost);

		int i = 0;
		foreach(GameObject f in Feautures){
			//try{
				f.GetComponent<Text>().text = ""+Mathf.Round(GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].ParameterActuals[i]);
			//}catch(NullReferenceException e){
					//Debug.Log(e);
			//}
			i += 1;
		}


		//Purchased.GetComponent<Text> ().text = ""+GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Purchased;
		//Tested.GetComponent<Text> ().text = ""+GameObject.Find ("GameControl").GetComponent<GameController> ().NodeList[idx].Tested;
	}
}
