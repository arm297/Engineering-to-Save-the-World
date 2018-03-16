using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Persist : MonoBehaviour 
{
   private static Persist instance = null; 
   public static Persist Instance 
    {
      get
      {
         if(instance == null)
         {
            instance = FindObjectOfType<Persist>();
            if( instance == null) 
            {
             GameObject go = new GameObject();
              go.name = "Persist";
              instance = go.AddComponent<Persist>(); 
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
