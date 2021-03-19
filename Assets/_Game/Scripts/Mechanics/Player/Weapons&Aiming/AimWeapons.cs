using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Aims the attached weapons towards a specific point with set speed
public class AimWeapons : MonoBehaviour
{
	GameObject mouse;
	Transform target;
	[SerializeField] Transform[] weapons;
	[SerializeField] float angleClamp = 45f;
	[SerializeField] float speed = 8f;
	[SerializeField] bool debugRays = false;
	[SerializeField] float debugRayLength = 25f;

	Quaternion targetRotation;
	Quaternion clampedRotation;
	float xAngle, yAngle;

	private void OnEnable()
	{
		mouse = GameObject.Find("Mouse");
	}

	void Update()
	{
		// Get vector of weapon center to mouse
		target = mouse.transform;
		Vector3 targetDir = target.position - transform.position;
		float step = speed * Time.deltaTime;

		foreach (Transform weapon in weapons)
		{
			// Rotate weapons to be parallel with weapon center
			Vector3 newDir = Vector3.RotateTowards(weapon.forward, targetDir, step, 0.0F);
			targetRotation = Quaternion.LookRotation(newDir);
			weapon.rotation = targetRotation;

			// Debug
			if (debugRays)
			{
				Debug.DrawRay(weapon.position, newDir * debugRayLength, Color.red);
			}
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



	}
}
