using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves the GameObject to the corresponding location of the mouse with provided distance from the camera
public class MouseToScreen : MonoBehaviour
{
	private Vector3 mousePos;
	private Vector3 targetVector;
	float moveSpeed = 0.02f;
	private Vector3 mouseRef = Vector3.zero;
	[SerializeField] float distance;

	Transform player;
	Camera cam;

	/*void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
	}*/

	private void Start()
	{
		cam = Camera.main;
		player = GameManager.player.obj.transform.Find("Weapon Mount").transform;
	}

	void Update()
	{
		mousePos = Input.mousePosition;

		// we dont want X units ahead of the camera but actually where the mouse would be but limited to X units away from the player
		// first we get the distance between the player and the aim
		float camPlayerDist = Vector2.Distance((Vector2)cam.ScreenToWorldPoint(mousePos), (Vector2)player.position);
		// use pyththeorem to find distance from cam we should use
		float dist = Mathf.Sqrt((distance * distance) - (camPlayerDist * camPlayerDist));
		

		mousePos.z = dist + 10;
		targetVector = Camera.main.ScreenToWorldPoint(mousePos);
		transform.position = targetVector;
		//transform.position = Vector3.SmoothDamp(transform.position, targetVector, ref mouseRef, moveSpeed);
	}


}
