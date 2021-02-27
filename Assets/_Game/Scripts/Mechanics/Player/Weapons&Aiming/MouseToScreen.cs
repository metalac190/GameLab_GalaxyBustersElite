using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToScreen : MonoBehaviour
{
	private Vector3 mousePos;
	private Vector3 targetVector;
	float moveSpeed = 0.02f;
	private Vector3 mouseRef = Vector3.zero;
	[SerializeField] float distance;

	void Start()
	{
		Cursor.visible = false;
	}

	void Update()
	{
		mousePos = Input.mousePosition;
		mousePos.z = distance + 10;
		targetVector = Camera.main.ScreenToWorldPoint(mousePos);
		transform.position = Vector3.SmoothDamp(transform.position, targetVector, ref mouseRef, moveSpeed);
	}


}
