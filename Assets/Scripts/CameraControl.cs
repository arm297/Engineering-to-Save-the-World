using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	public float speed = 5.0f;

	public float dragSpeed = 2;
	private Vector3 dragOrigin;

	void Update () {
		if(Input.GetKey(KeyCode.RightArrow))
		{
				transform.position = new Vector3(speed * Time.deltaTime + transform.position.x,transform.position.y,transform.position.z);
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
				transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
				transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
		}
		if(Input.GetKey(KeyCode.UpArrow))
		{
				transform.position = new Vector3(transform.position.x,transform.position.y + speed * Time.deltaTime, transform.position.z);
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
		{
				transform.position = new Vector3(transform.position.x,transform.position.y , transform.position.z - speed * Time.deltaTime);
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
		{
				transform.position = new Vector3(transform.position.x,transform.position.y , transform.position.z + speed * Time.deltaTime);
		}

		if (Input.GetMouseButtonDown(0))
		{
				dragOrigin = Input.mousePosition;
				return;
		}

		if (!Input.GetMouseButton(0)) return;

		Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
		Vector3 move = new Vector3(- pos.x * dragSpeed, - pos.y * dragSpeed, 0);

		transform.Translate(move, Space.World);

}
}
