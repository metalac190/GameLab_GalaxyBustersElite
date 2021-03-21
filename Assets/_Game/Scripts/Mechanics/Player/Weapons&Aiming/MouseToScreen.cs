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

	/*void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
	}*/

	void Update()
	{
		mousePos = Input.mousePosition;
		mousePos.z = distance + 10;
		targetVector = Camera.main.ScreenToWorldPoint(mousePos);
		transform.position = targetVector;
		//transform.position = Vector3.SmoothDamp(transform.position, targetVector, ref mouseRef, moveSpeed);
	}


}
