using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initialize_drag_drop_drill : MonoBehaviour {

	public string[] items;
	public string[] regions;
	public string[][] items_to_region;
	public int[] items_belongs_to_wich_regions;
	//public double top;
	//public double left;
	public GameObject copy_this_item;
	public GameObject copy_this_region;
	
	private float y_offset = (float)0.1; 
	public float x_offset; 
	public float z_offset; 


	// Use this for initialization
	private List<GameObject> items_GameObject = new List<GameObject>();
	private List<GameObject> regions_GameObject = new List<GameObject>();
	//private List<GameObject> regions;
	void Start () {

		//build array of positions for items and regions to take
		//
		//for items
		Vector3 itemPos = copy_this_item.transform.position;
		Vector3[] itemPositions = new Vector3[items.Length];
		float itemx = itemPos.x;
		float itemy = itemPos.y;
		float itemz = itemPos.z;
		for(int i = 0; i < items.Length; i++){
			itemx -= x_offset;
			itemy += y_offset;
			itemz -= z_offset;
			Vector3 new_item_pos = new Vector3(itemx, itemy, itemz);
			itemPositions[i] = new_item_pos;
		}
		//randomize item positions
		reshuffle(itemPositions);

		//same for regions
		Vector3 regionPos = copy_this_region.transform.position;
		Vector3[] regionPositions = new Vector3[regions.Length];
		itemx = regionPos.x;
		itemy = regionPos.y;
		itemz = regionPos.z;
		for(int i = 0; i < regions.Length; i++){
			itemx -= x_offset;
			itemy += y_offset;
			itemz -= z_offset;
			Vector3 new_item_pos = new Vector3(itemx, itemy, itemz);	
			regionPositions[i] = new_item_pos;
		}
		 
		 //instantiate items
		items_GameObject = instantiate_quad(copy_this_item, items, itemPositions);
		Destroy(copy_this_item);

		//instantiate regions
		regions_GameObject = instantiate_quad(copy_this_region, regions, regionPositions);
		Destroy(copy_this_region);

		//test score
		score_user();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//instantiate region or item
	List<GameObject> instantiate_quad(GameObject original, string[] items, Vector3[] itemPositions){
		List<GameObject> items_GameObject = new List<GameObject>();
		for(int i = 0; i < items.Length; i++){
			//Debug.Log(items[i]);
			Vector3 new_item_pos = itemPositions[i];
			GameObject item_i = Instantiate<GameObject>(original, new_item_pos, transform.rotation);
			Transform parent = item_i.GetComponent<Transform>();
			for (int j = 0; i < parent.childCount; i++)
         	{
             	Transform child = parent.GetChild(j);
				Debug.Log(child.tag);
             	if(child.tag == "label")
					child.GetComponent<TextMesh>().text = items[i];//spawnPoints.Add(child.gameObject);
					Debug.Log(items[i]);
         	}

			//item_i.GetComponent<TextMesh>().text = items[i];
			items_GameObject.Add(item_i);
		}
		return items_GameObject;
	}

	// This method will go through each item to determine if correctly positioned
	// Each region has transform.position and corners
	// Go through each item, get position, and determine if within a region. If so, check if it is the correct region
	// Uses method from drag_drop.cs corners(GameObject) to get list of corners which are Vector3
	double score_user(){
		double score = 0;
		drag_drop dd = new drag_drop();
		for(int i = 0; i <= items_GameObject.Count; i++){
			GameObject item_go = items_GameObject[i];
			List<Vector3> item_go_corners = dd.corners(item_go);
			//test if item is within region
			//returned corners in this order: topright, topleft, bottomright, bottomleft			
			for(int j = 0; j <= regions_GameObject.Count; j++){
				GameObject region_go = regions_GameObject[j];
				List<Vector3> region_go_corners = dd.corners(region_go);
				if(item_go_corners[0].x <= region_go_corners[0].x
				&& item_go_corners[0].z <= region_go_corners[0].z
				&& item_go_corners[0].x >= region_go_corners[0].x
				&& item_go_corners[0].z >= region_go_corners[0].z){
					//then item is within region
					//test if item is correctly within region
					int correct_region_j = items_belongs_to_wich_regions[i];
					if(correct_region_j == j){
						//correct answer
						score++;
					}else{
						//incorrect answer
					}

				}
				
			}
		}
		Debug.Log(items_GameObject.Count);
		Debug.Log(regions_GameObject.Count);
		return score;
		
	}


	void reshuffle(Vector3[] texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++ )
        {
            Vector3 tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
    }

}
