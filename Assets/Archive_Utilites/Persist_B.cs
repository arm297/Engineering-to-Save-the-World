using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Persist_B : MonoBehaviour 
{
   private static Persist_B instance = null; 
   public static Persist_B Instance 
    {
      get
      {
         if(instance == null)
         {
            instance = FindObjectOfType<Persist_B>();
            if( instance == null) 
            {
             GameObject go = new GameObject();
              go.name = "Persist_B";
              instance = go.AddComponent<Persist_B>(); 
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
