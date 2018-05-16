using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Persist_D : MonoBehaviour 
{
   private static Persist_D instance = null; 
   public static Persist_D Instance 
    {
      get
      {
         if(instance == null)
         {
            instance = FindObjectOfType<Persist_D>();
            if( instance == null) 
            {
             GameObject go = new GameObject();
              go.name = "Persist_D";
              instance = go.AddComponent<Persist_D>(); 
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