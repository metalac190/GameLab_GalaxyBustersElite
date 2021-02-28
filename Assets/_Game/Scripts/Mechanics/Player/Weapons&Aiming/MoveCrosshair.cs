using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		transform.LookAt(Camera.main.transform.position, -Vector3.up);

		// Debug
		if (debug)
		{
			Debug.DrawRay(start.position, line * percentageToEnd, Color.green);
		}

	}
}
