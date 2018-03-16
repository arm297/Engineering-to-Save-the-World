using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Persist_E : MonoBehaviour 
{
   private static Persist_E instance = null; 
   public static Persist_E Instance 
    {
      get
      {
         if(instance == null)
         {
            instance = FindObjectOfType<Persist_E>();
            if( instance == null) 
            {
             GameObject go = new GameObject();
              go.name = "Persist_E";
              instance = go.AddComponent<Persist_E>(); 
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
