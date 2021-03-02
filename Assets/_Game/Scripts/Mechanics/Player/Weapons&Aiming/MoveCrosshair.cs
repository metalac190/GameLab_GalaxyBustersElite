using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Places GameObject at a percent distance on the line between two points.
public class MoveCrosshair : MonoBehaviour
{
	[SerializeField] Transform start;
	[SerializeField] Transform end;
	[SerializeField] [Range(0, 1)] float percentageToEnd = 1f;
	[SerializeField] bool debug = true;
	Vector3 line;

    void Update()
    {
		line = end.position - start.position;
		transform.position = start.position + (line * percentageToEnd);

		// BUG: Sprites don't rotate towards camera properly near corners of screen
		transform.rotation = Camera.main.transform.rotation;

		// Debug
		if (debug)
		{
			Debug.DrawRay(start.position, line * percentageToEnd, Color.green);
		}

	}
}
