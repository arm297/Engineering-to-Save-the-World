using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Developed by Brighid Meredith 10/1/2017
 Purpose: Controls logic of Connections
 When an associated quad is activated, the connection lights up (association controlled by block_end1 and block_end2
 When connection is possible, it is rendered. connection changes color on mouse over. connection can be selected or unselected. if selected, callouts are activated.
     */

public class connection : MonoBehaviour {

    public Color impossible = Color.grey;
    public Color possible = Color.cyan;
    public Color mouse_over = Color.red;
    public Color selected = Color.green;
    public bool selected_connection = false;
    public bool possible_connection = false;
    public string tag_of_line = "line";
    public int block_end1;
    public int block_end2;
    private List<GameObject> children;
    private GameObject parent;
    private bool mouse_down_over = false;
    private bool mouse_inside = false;
    private Color startcolor;
    public bool updated = false;



    // Use this for initialization
    void Start () {
        parent = FindParentWithTag(gameObject, tag_of_line);
        if (!parent)
        {
            parent = gameObject;
        }
        children = FindChildrenWithTag(parent, tag_of_line);

    }


    private void OnMouseDown()
    {
        mouse_down_over = true;
    }


    //Controls highlighting when mouse hovers over block
    //store start_color before changing to default hover
    //check triggers to see if connection is selectable
    void OnMouseEnter()
    {
        mouse_inside = true;
        startcolor = GetComponent<Renderer>().material.color;

        if (!parent.GetComponent<connection>().selected_connection)
        {
            if (parent.GetComponent<connection>().possible_connection)
            {
                foreach (GameObject child in children)
                {
                    child.GetComponent<Renderer>().material.color = parent.GetComponent<connection>().mouse_over;
                }
            }
        }
    }

    //mouse exited object. return to start color.
    void OnMouseExit()
    {
        mouse_inside = false;
        if (!parent.GetComponent<connection>().selected_connection)
        {
            foreach (GameObject child in children)
            {
                child.GetComponent<Renderer>().material.color = startcolor;
            }
        }
    }

    // Update is called once per frame
    void Update () {

        if (!updated)
        {
            //store true in trigger
            updated = true;

            //change color to possible
            if (parent.GetComponent<connection>().possible_connection && !parent.GetComponent<connection>().selected_connection)
            {
                
                foreach (GameObject child in children)
                {
                    child.GetComponent<Renderer>().material.color = parent.GetComponent<connection>().possible;
                    child.GetComponent<Renderer>().enabled = true;
                }
            }

            //change color to default
            if (!parent.GetComponent<connection>().possible_connection && !parent.GetComponent<connection>().selected_connection)
            {
                foreach (GameObject child in children)
                {
                    child.GetComponent<Renderer>().material.color = parent.GetComponent<connection>().impossible;
                    child.GetComponent<Renderer>().enabled = false;
                }
            }

            //change to selected
            if (parent.GetComponent<connection>().selected_connection)
            {
                foreach (GameObject child in children)
                {
                    child.GetComponent<Renderer>().material.color = parent.GetComponent<connection>().selected;
                }
            }
        }

        //on mouse up
        //check triggers and mouse location
        if (Input.GetMouseButtonUp(0))
        {
            if (mouse_down_over && parent.GetComponent<connection>().possible_connection && mouse_inside && !parent.GetComponent<connection>().selected_connection)
            {
                parent.GetComponent<connection>().selected_connection = true;
                parent.GetComponent<connection>().updated = false;
                
            }
            else if (mouse_down_over && parent.GetComponent<connection>().possible_connection && mouse_inside && parent.GetComponent<connection>().selected_connection)
            {
                parent.GetComponent<connection>().selected_connection = false;
                parent.GetComponent<connection>().updated = false;
                startcolor = parent.GetComponent<connection>().possible;
            }
            mouse_down_over = false;
            parent.GetComponent<connection>().mouse_down_over = false;
        }

    }

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
