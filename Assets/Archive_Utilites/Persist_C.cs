using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Persist_C : MonoBehaviour 
{
   private static Persist_C instance = null; 
   public static Persist_C Instance 
    {
      get
      {
         if(instance == null)
         {
            instance = FindObjectOfType<Persist_C>();
            if( instance == null) 
            {
             GameObject go = new GameObject();
              go.name = "Persist_C";
              instance = go.AddComponent<Persist_C>(); 
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
       }
    }
}