using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Places crosshairs with given sprites in world determined by mouse location
public class DisplayCrosshairs : MonoBehaviour
{
	[SerializeField] Transform cam;
	[SerializeField] Transform player;
	[SerializeField] Transform mouse;
	[SerializeField] Transform nearCrosshair;
	[SerializeField] Transform farCrosshair;
	[SerializeField] [Range(0, 2f)] float nearCrosshairDistance = 1f;
	[SerializeField] [Range(0, 2f)] float farCrosshairDistance = 1f;
	[SerializeField] bool debug = true;

	Vector3 line1, line2, avgLine, avgStart, targetLine;

	void Update()
	{
		// Calculate average line between camera to mouse and player to mouse
		avgLine = (cam.position - player.position) * -1;
		avgStart = cam.position + (avgLine * .5f);
		targetLine = mouse.position - avgStart;

		// Set distance of crosshair at percent distance along line
		nearCrosshair.position = avgStart + (targetLine * nearCrosshairDistance);
		farCrosshair.position = avgStart + (targetLine * farCrosshairDistance);

		// Rotate sprite to look at camera
		nearCrosshair.rotation = Camera.main.transform.rotation;
		farCrosshair.rotation = Camera.main.transform.rotation;

		// Draw debug lines for each vector
		if (debug)
		{
			line1 = mouse.position - cam.position;
			line2 = mouse.position - player.position;

			Debug.DrawRay(cam.position, line1 * farCrosshairDistance, Color.green);
			Debug.DrawRay(player.position, line2 * farCrosshairDistance, Color.blue);
			Debug.DrawRay(avgStart, targetLine, Color.red);
		}
	}
}

