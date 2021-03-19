using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCrosshairs : MonoBehaviour
{
	[Header("Crosshairs in world")]
	public Transform playerNear;
	public Transform playerFar;
	[Header("Crosshairs on canvas")]
	public RectTransform canvasNear;
	public RectTransform canvasFar;

    void Update()
    {
		Vector3 posNear = Camera.main.WorldToScreenPoint(playerNear.position);
		Vector3 posFar = Camera.main.WorldToScreenPoint(playerFar.position);

		canvasNear.position = posNear;
		canvasFar.position = posFar;
	}
}
