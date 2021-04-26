using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves the GameObject to the corresponding location of the mouse with provided distance from the camera
public class MouseToScreen : MonoBehaviour
{
	private Vector3 mousePos, sensMouse;
	private Vector3 targetVector;
	float moveSpeed = 0.02f;
	private Vector3 mouseRef = Vector3.zero;
	[SerializeField] bool newAiming = true;
	[SerializeField] [Range(0,2)] public float sensitivity = 1f;
	[SerializeField] float distance;
	float midX, midY, xMoved, yMoved, camPlayerDist;

	Transform player;
	Camera cam;

	private void Start()
	{
		cam = Camera.main;
		player = GameManager.player.obj.transform.Find("Weapon Mount").transform;
		sensMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
	}

	void Update()
	{
		mousePos = Input.mousePosition;

		midX = Screen.width / 2;
		midY = Screen.height / 2;

		xMoved = sensitivity * (mousePos.x - midX);
		yMoved = sensitivity * (mousePos.y - midY);

		sensMouse.x = midX + xMoved;
		sensMouse.y = midY + yMoved;
		sensMouse.z = 0;

		//if (mousePos.x > Screen.width - 2 || mousePos.x < 2)
		//	sensMouse.x += Input.GetAxisRaw("Mouse X");
		//else if(sensMouse.x > Screen.width - 2 || sensMouse.x < 2)
		//	sensMouse.x += Input.GetAxisRaw("Mouse X");
		//else
		//	sensMouse.x = midX + xMoved;

		// we dont want X units ahead of the camera but actually where the mouse would be but limited to X units away from the player
		// first we get the distance between the player and the aim
		if (newAiming)
			camPlayerDist = Vector2.Distance(cam.ScreenToWorldPoint(sensMouse), player.position);
		else
			camPlayerDist = Vector2.Distance(cam.ScreenToWorldPoint(mousePos), player.position);
		// use pyththeorem to find distance from cam we should use
		float dist = Mathf.Sqrt((distance * distance) - (camPlayerDist * camPlayerDist));

		//if (Input.GetKeyDown(KeyCode.Alpha1))
		//	newAiming = newAiming ? false : true;

		//if (Input.GetKeyDown(KeyCode.Alpha2))
		//	sensitivity = 0.5f;

		//if (Input.GetKeyDown(KeyCode.Alpha3))
		//	sensitivity = 1.0f;

		//if (Input.GetKeyDown(KeyCode.Alpha4))
		//	sensitivity = 1.5f;

		//if (sensitivity >= 1)
		//	Cursor.lockState = CursorLockMode.Confined;
		//else
		//	Cursor.lockState = CursorLockMode.None;

		mousePos.z = dist + 10;
		sensMouse.z = dist + 10;

		if (newAiming)
			targetVector = Camera.main.ScreenToWorldPoint(sensMouse);
		else
			targetVector = Camera.main.ScreenToWorldPoint(mousePos);

		transform.position = targetVector;
	}


}
