using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCrosshairs : MonoBehaviour
{
	private Transform playerNear;
	private Transform playerFar;
	[Header("Crosshairs on canvas")]
	public RectTransform canvasNear;
	public RectTransform canvasFar;

	public void OnEnable()
	{
		playerNear = GameObject.Find("CrosshairNearTransform").transform;
		playerFar = GameObject.Find("CrosshairFarTransform").transform;
	}

	void Update()
    {
		Vector3 posNear = Camera.main.WorldToScreenPoint(playerNear.position);
		Vector3 posFar = Camera.main.WorldToScreenPoint(playerFar.position);

		canvasNear.position = posNear;
		canvasFar.position = posFar;
	}
}
