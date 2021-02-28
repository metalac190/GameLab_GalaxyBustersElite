using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCrosshair : MonoBehaviour
{
	[SerializeField] Transform target;
	[SerializeField] float angleClamp = 45f;
	[SerializeField] float speed = 1f;
	[SerializeField] bool debugRays = true;
	[SerializeField] float debugRayLength = 10f;

	Quaternion targetRotation;
	Quaternion clampedRotation;
	float xAngle, yAngle;

	void Update()
	{
		Vector3 targetDir = target.position - transform.position;
		float step = speed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		targetRotation = Quaternion.LookRotation(newDir);

		// Debug
		if (debugRays)
		{
			Debug.DrawRay(transform.position, newDir * debugRayLength, Color.red);
		}

		// TODO: Fix angle clamping to use local rotation
		//xAngle = targetRotation.eulerAngles.x;
		//xAngle = (xAngle > 180) ? xAngle - 360 : xAngle;
		//yAngle = targetRotation.eulerAngles.y;
		//yAngle = (yAngle > 180) ? yAngle - 360 : yAngle;

		//clampedRotation.eulerAngles = new Vector3(
		//  Mathf.Clamp(xAngle, angleClamp * -1, angleClamp),
		//  Mathf.Clamp(yAngle, angleClamp * -1, angleClamp),
		//  0
		//);

		transform.rotation = targetRotation;

	}
}
