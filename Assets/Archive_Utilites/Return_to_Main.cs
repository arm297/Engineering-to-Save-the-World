using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Return_to_Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		    Camera.main.gameObject.active = false;
			SceneManager.LoadScene("Master_Scene", LoadSceneMode.Additive);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
