using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Developed by Brighid Meredith 10/1/2017
 Purpose: Control drag and drop of quads onto regions
 Regions (quads) respond differently depending on type
 Regions change color on hover, can be dragged, and snap into place if over certain kinds of quads
 IsDragable indicates type of quad, either region (false) or block (true)
 block (int) names the block for connections to reference
     */

public class drag_drop : MonoBehaviour {

    public int block;

    //controls logic
    private bool mouse_inside = false;

    //Controls highlighting when mouse hovers over block
    private Color startcolor;
    public Color mouse_over;
    private Vector3 screenPoint;
    private Vector3 offset;
    public bool IsDragable = true;
    private bool dragged = false;
    private Vector3 text_screenPoint;
    private Vector3 text_offset;
    // Use this for initialization
    private GameObject[] possiblyInsideList;
    private int index_of_snapped_into_pos = -1; // if inside a region, then set to index matching to possiblyInsideLiist
    private GameObject[] possiblyConnectionsList;
    private List<GameObject> affectedConnectionsList = new List<GameObject>();
    public bool IsReliability = false;
    public double reliability = 0.01;  // reliability of block used in reliability calculations
    public double cost = 1.0;  // cost to use the block if dragged into system
    public bool ContainsBlock = false; // only set to true if a block is placed inside. Each block can only contain one reliability
    //for cost
    private double cost_threshold_low = 1.1;
    private double cost_threshold_mid = 2.1;
    private Color cheap_color = new Color(0, 1, 0, 1);
    private Color mid_color = new Color(1, (float)0.92, (float)0.016, 1);
    private Color high_color = new Color(1, 0, 0, 1);

    void OnMouseEnter()
    {
        startcolor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = mouse_over;
        mouse_inside = true;
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = startcolor;
        mouse_inside = false;
    }

    private bool is_set = false;
    void OnMouseDown()
    {
        if (IsDragable)    // Only do if IsDraggable == true
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            dragged = true;
        }
    }

    void OnMouseDrag()
    {
        if (IsDragable)    // Only do if IsDraggable == true
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z); // hardcode the y and z for your use

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;

            foreach(GameObject go in affectedConnectionsList)
            {
                wipe_connections(go);
            }
            affectedConnectionsList.Clear();
            if(is_set){
                activate_label(possiblyInsideList[index_of_snapped_into_pos], true);
                possiblyInsideList[index_of_snapped_into_pos].GetComponent<drag_drop>().ContainsBlock = false;
                possiblyInsideList[index_of_snapped_into_pos].GetComponent<Renderer>().material.color = startcolor;
                index_of_snapped_into_pos = -1; //no longer within region
                is_set = false;
            }

        }
    }


    private void OnMouseOver()
    {
        //print(gameObject.tag);
    }


    GameObject cam;
    void Start () {
        possiblyInsideList = GameObject.FindGameObjectsWithTag("snap_into");
        possiblyConnectionsList = GameObject.FindGameObjectsWithTag("line");
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0];

        //if reliability block, reset text with reliability
        if(IsReliability && IsDragable){
            GameObject l = FindChildrenWithTag(gameObject,"label")[0];
            l.GetComponent<TextMesh>().text = ""+reliability;//"("+reliability+","+cost+")";
            //and change color depending on cost
            if(cost < cost_threshold_low){
                //cheap
                GetComponent<Renderer>().material.color = cheap_color;
            }else if(cost < cost_threshold_mid){
                //mid cost
                GetComponent<Renderer>().material.color = mid_color;
            }else{
                //high cost
                GetComponent<Renderer>().material.color = high_color;
            }
        }
    }



	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0) && IsDragable && dragged)
        {
            dragged = false;
            //mouse released. check to see if in range of snap_to box
            // Debug.Log("Pressed left click.");
            Vector3 curr_pos = transform.position;
            print(possiblyInsideList.Length);
            for(int i = 0; i < possiblyInsideList.Length; i++)
            {
                
                List<Vector3> list_of_corners = corners(possiblyInsideList[i]);
                float top_right_x = list_of_corners[0].x;
                float top_right_y = list_of_corners[0].z;
                float top_left_x = list_of_corners[1].x;
                float bottom_left_y = list_of_corners[3].z;
                if (curr_pos.x < top_right_x && curr_pos.x > top_left_x && curr_pos.z < top_right_y && curr_pos.z > bottom_left_y)
                {
                    //within this object's domain
                    //snap the block into the region
                    print(possiblyInsideList[i].GetComponent<drag_drop>().ContainsBlock);
                    if((IsReliability==true && possiblyInsideList[i].GetComponent<drag_drop>().ContainsBlock == false) || IsReliability==false){
                        //calculate new position
                        if(IsReliability==true){
                            //different dynamic for reliability game: put in dead center of region
                            Vector3 game_coordinates_of_region = possiblyInsideList[i].transform.position;
                            Vector3 game_coordinates_of_block = transform.position;
                            Vector3 screen_coordinates_of_region = cam.GetComponent<Camera>().WorldToScreenPoint(possiblyInsideList[i].transform.position);
                            Vector3 screen_coordinates_of_block = cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
                            //calculate new x
                            float x_slope = (screen_coordinates_of_region.x - screen_coordinates_of_block.x) 
                            / (game_coordinates_of_region.x - game_coordinates_of_block.x);
                            float x_intercept = screen_coordinates_of_region.x - x_slope * game_coordinates_of_region.x;
                            float new_x = (screen_coordinates_of_region.x - x_intercept)/x_slope;
                            //calculate new y
                            float z_slope = (screen_coordinates_of_region.z - screen_coordinates_of_block.z) 
                            / (game_coordinates_of_region.z - game_coordinates_of_block.z);
                            float z_intercept = screen_coordinates_of_region.z - z_slope * game_coordinates_of_region.z;
                            float new_z = (screen_coordinates_of_region.z - z_intercept)/z_slope;
                            
                            transform.position = new Vector3(new_x, game_coordinates_of_block.y, new_z);


                        }else{
                            //general snap into, but not exact
                            Vector3 snap_pos = possiblyInsideList[i].transform.position;
                            snap_pos.y = curr_pos.y;
                            //transform.position = snap_pos;
                            Vector3 goal_screenPos = cam.GetComponent<Camera>().WorldToScreenPoint(possiblyInsideList[i].transform.position);
                            Vector3 current_screenPos = cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
                            float deltaX = (float)0.5;// + transform.position.x;
                            float deltaZ = (float)0.5;// + transform.position.y;
                            transform.position = new Vector3(curr_pos.x + deltaX, curr_pos.y, curr_pos.z + deltaZ );
                            Vector3 temp_screenPos = cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
                            float scalarX = (temp_screenPos.x - current_screenPos.x)/deltaX;
                            float scalarZ = (temp_screenPos.y - current_screenPos.y)/deltaZ;
                            //Debug.Log(scalarZ);
                            float target_deltaX = goal_screenPos.x - current_screenPos.x;
                            float target_deltaZ = goal_screenPos.z - current_screenPos.z;
                            target_deltaX = target_deltaX / scalarX;
                            target_deltaZ = target_deltaZ / scalarZ;
                            transform.position = new Vector3(curr_pos.x + deltaX, curr_pos.y, curr_pos.z + target_deltaZ);
                            //transform.position = snap_pos;
                            //transform.parent = possiblyInshttps://mail.google.com/mail/u/0/#inboxideList[i].transform.parent;
                            //gameObject.transform.rotation = possiblyInsideList[i].transform.rotation;
                        }
                        
                        
                        //print("inside?");
                        possiblyInsideList[i].GetComponent<drag_drop>().ContainsBlock = true;
                        possiblyInsideList[i].GetComponent<drag_drop>().reliability = reliability;
                        possiblyInsideList[i].GetComponent<drag_drop>().cost = cost;
                        possiblyInsideList[i].GetComponent<Renderer>().material.color = possiblyInsideList[i].GetComponent<drag_drop>().mouse_over;
                        

                        //check for possible connections
                        GameObject go = possiblyInsideList[i];
                        int block_id = go.GetComponent<drag_drop>().block;
                        //if the block had a child with tag 'label', then disable label
                        activate_label(go, false);
                        is_set = true;
                        index_of_snapped_into_pos = i;
                        //go through list of lines to determine if one of them starts or ends at this block_id
                        affectedConnectionsList.Clear();// = new List<GameObject>(); //reset list of affected connections
                        for (int j = 0; j < possiblyConnectionsList.Length; j++)
                        {
                            GameObject go_j = possiblyConnectionsList[j];
                            if(go_j.GetComponent<connection>().block_end1 == block_id || go_j.GetComponent<connection>().block_end2 == block_id)
                            {
                                //print("activate line");
                                go_j.GetComponent<connection>().possible_connection = true;
                                go_j.GetComponent<connection>().updated = false;
                                affectedConnectionsList.Add(go_j);
                            }
                        }
                    }

                }

            }
        }
    }

    //set connection to default
    private void wipe_connections(GameObject go)
    {
        go.GetComponent<connection>().possible_connection = false;
        go.GetComponent<connection>().selected_connection = false;
        go.GetComponent<connection>().updated = false;
    }

    //get the coordinates of each corner of gameobject go
    public List<Vector3> corners(GameObject go)
    {
        float width = go.GetComponent<Renderer>().bounds.size.x;
        float height = go.GetComponent<Renderer>().bounds.size.y;
        float depth = go.GetComponent<Renderer>().bounds.size.z;
        Vector3 topRight = go.transform.position, topLeft = go.transform.position, bottomRight = go.transform.position, bottomLeft = go.transform.position;

        topRight.x += width / 2;
        topRight.y += height / 2;
        topRight.z += depth / 2;

        topLeft.x -= width / 2;
        topLeft.y += height / 2;
        topLeft.z += depth / 2;

        bottomRight.x += width / 2;
        bottomRight.y -= height / 2;
        bottomRight.z -= depth / 2;

        bottomLeft.x -= width / 2;
        bottomLeft.y -= height / 2;
        bottomLeft.z -= depth / 2;

        List<Vector3> cor_temp = new List<Vector3>();
        cor_temp.Add(topRight);
        cor_temp.Add(topLeft);
        cor_temp.Add(bottomRight);
        cor_temp.Add(bottomLeft);

        return cor_temp;
    }

    //switch label renderer
    public void activate_label(GameObject parent, bool status){
        List<GameObject> labels = FindChildrenWithTag(parent, "label");
        foreach(GameObject label in labels){
            label.GetComponent<Renderer>().enabled = status;
        }
    }

    //useful method. should be its own class.
    public static List<GameObject> FindChildrenWithTag(GameObject pObject, string tag)
    {
        List<GameObject> children = new List<GameObject>();
        if (pObject)
        {
            Transform p = pObject.transform;


            foreach (Transform child in p.transform)
            {
                if (child.CompareTag(tag))
                {
                    children.Add(child.gameObject);
                }
            }
        }
        return children;

    }

}
