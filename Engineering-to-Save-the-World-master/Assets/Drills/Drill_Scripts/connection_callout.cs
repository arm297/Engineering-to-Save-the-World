using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Developed by Brighid Meredith 10/1/2017
 Purpose: Control logic of callouts on each connection
 Callouts are rendered when the connection is selected
 Callouts can be hovered over, selected, and unselected
 Callouts consist of quad and 3DText
     */

public class connection_callout : MonoBehaviour {

    //same script is used for multiple types of objects. boolean remembers which type of object is associated. renders behave differently for text then quads.
    private bool is_quad = false;  
    private bool is_text = false;
    private bool is_selected = false;
    public string tag_of_line = "line";
    public Color mouse_over = Color.red;
    public Color selected = Color.green;
    public Color possible = Color.cyan;
    public Color impossible = Color.grey;
    private bool mouse_down = false;
    private bool mouse_inside = false;

    // Use this for initialization
    GameObject parent;
	void Start () {
        parent = FindParentWithTag(gameObject, tag_of_line);
        if (!parent)
        {
            parent = gameObject;
        }

        if (gameObject.GetComponent<Renderer>())
        {
            is_quad = true;
            //print(1);
        }

        if (gameObject.GetComponent<TextMesh>())
        {
            is_text = true;
            is_quad = false;
        }

        if (is_quad)
        {
            gameObject.GetComponent<Renderer>().enabled = false ;
            gameObject.GetComponent<Renderer>().material.color = parent.GetComponent<connection>().impossible;
        }

        if (is_text)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        //on mouse up
        if (Input.GetMouseButtonUp(0))
        {
            if (mouse_down && parent.GetComponent<connection>().selected_connection && mouse_inside && !is_selected)
            {
                is_selected = true;
                
                if (is_quad)
                {
                    gameObject.GetComponent<Renderer>().material.color = parent.GetComponent<connection>().selected;
                }
            }
            else if (mouse_down && parent.GetComponent<connection>().selected_connection && mouse_inside && is_selected)
            {
                is_selected = false;

                if (is_quad)
                {
                    gameObject.GetComponent<Renderer>().material.color = parent.GetComponent<connection>().possible;
                    gameObject.GetComponent<Renderer>().enabled = true;
                    startcolor = parent.GetComponent<connection>().possible;
                }
                if (is_text)
                {
                    gameObject.GetComponent<Renderer>().enabled = true;
                }
            }
            mouse_down = false;
        }

     //   else if (parent.GetComponent<connection>().selected_connection && is_quad && !is_selected && !mouse_down)
      //  {
       //     gameObject.GetComponent<Renderer>().material.color = possible;
        //}

        if (!parent.GetComponent<connection>().selected_connection)
        {
            is_selected = false;
            if (is_quad)
            {
                gameObject.GetComponent<Renderer>().material.color = parent.GetComponent<connection>().impossible;
                gameObject.GetComponent<Renderer>().enabled = false;
            }
            if (is_text)
            {
                gameObject.GetComponent<Renderer>().enabled = false;
            }
        }

        if (parent.GetComponent<connection>().selected_connection)
        {
            //is_selected = false;
            if (is_quad)
            {
                gameObject.GetComponent<Renderer>().enabled = true;
            }
            if (is_text)
            {
                gameObject.GetComponent<Renderer>().enabled = true;
            }
        }
    }

    //Controls highlighting when mouse hovers over block
    private Color startcolor;

    void OnMouseEnter()
    {
        mouse_inside = true;
        startcolor = GetComponent<Renderer>().material.color;
        if (parent.GetComponent<connection>().selected_connection && is_quad && !is_selected)
        {
            gameObject.GetComponent<Renderer>().material.color = parent.GetComponent<connection>().mouse_over;
        }
    }

    void OnMouseExit()
    {
        mouse_inside = false;
        if (is_quad && !is_selected)
        {
            gameObject.GetComponent<Renderer>().material.color = startcolor;
        }
    }

    private void OnMouseDown()
    {
        mouse_down = true;
    }

    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }

}
