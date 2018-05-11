using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Persist_A : MonoBehaviour 
{
   private static Persist_A instance = null;
   private int switch_count = 0; 
   public static Persist_A Instance 
    {
      get
      {
         if(instance == null)
         {
            instance = FindObjectOfType<Persist_A>();
            if( instance == null) 
            {
             GameObject go = new GameObject();
              go.name = "Persist_A";
              instance = go.AddComponent<Persist_A>(); 
              DontDestroyOnLoad(go); 
            }
         }
        return instance;
        }
     }
    void Awake()
     {
       if(instance == null ) 
       {
         instance = this;
         DontDestroyOnLoad(this.gameObject);
       }
      else
       {
         Destroy(gameObject);
         switch_count += 1;
         Debug.Log("switch_count"+switch_count);
       }
    }
}
	
	/*public bool created = false;

	// Use this for initialization
	void Start () {

		if (!created) {
			// this is the first instance - make it persist
			DontDestroyOnLoad(this.gameObject);
			created = true;
		} else {
			// this must be a duplicate from a scene reload - DESTROY!
			Destroy(this.gameObject);
			Debug.Log("destroying...");
			Camera.main.gameObject.active = true;
		} 

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	*/

