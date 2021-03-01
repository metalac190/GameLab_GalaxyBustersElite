using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Places crosshairs with given sprites in world determined by mouse location
public class DisplayCrosshairs : MonoBehaviour
{
	[SerializeField] Transform start;
	[SerializeField] Transform end;
	[SerializeField] Transform nearCrosshair;
	[SerializeField] Transform farCrosshair;
	[SerializeField] [Range(0, 1)] float nearCrosshairDistance = 1f;
	[SerializeField] [Range(0, 1)] float farCrosshairDistance = 1f;
	[SerializeField] bool debug = true;
	Vector3 line;

	void Update()
	{
		// Calculate line length
		line = end.position - start.position;

		// Set distance of crosshair at percent distance along line
		nearCrosshair.position = start.position + (line * nearCrosshairDistance);
		farCrosshair.position = start.position + (line * farCrosshairDistance);

		// BUG: Sprites don't rotate towards camera properly near corners of screen
		// Rotate sprite to look at camera
		nearCrosshair.LookAt(Camera.main.transform.position, -Vector3.up);
		farCrosshair.LookAt(Camera.main.transform.position, -Vector3.up);

		// Draw debug line to far crosshair
		if (debug)
		{
			Debug.DrawRay(start.position, line * farCrosshairDistance, Color.green);
		}

	}
}
