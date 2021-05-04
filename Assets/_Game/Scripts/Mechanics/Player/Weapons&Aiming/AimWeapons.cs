using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Aims the attached weapons towards a specific point with set speed
public class AimWeapons : MonoBehaviour
{
	GameObject mouse;
	Transform target;
	Camera cam;
	[SerializeField] Transform[] weapons;
	[SerializeField] float speed = 8f;
	[SerializeField] bool debugRays = false;
	[SerializeField] float debugRayLength = 25f;
	[SerializeField] GameObject currentTarget;

	public float aimAssistDistance = 50f;
	public float aimAssistWidth = 2f;
	public LayerMask targetMask;

	Quaternion targetRotation;
	Vector3 mousePos;
	bool targetFound = false;

	private void OnEnable()
	{
		mouse = GameObject.Find("Mouse");
		cam = Camera.main;
	}

	void Update()
	{
		// Get vector of weapon center to mouse
		target = mouse.transform;
		Vector3 targetDir = target.position - transform.position;
		float step = speed * Time.deltaTime;

		if (!targetFound)
		{
			foreach (Transform weapon in weapons)
			{
				// Rotate weapons to be parallel with weapon center
				Vector3 newDir = Vector3.RotateTowards(weapon.forward, targetDir, step, 0.0F);
				targetRotation = Quaternion.LookRotation(newDir, transform.up);
				weapon.rotation = targetRotation;

				// Debug
				if (debugRays)
				{
					Debug.DrawRay(weapon.position, newDir * debugRayLength, Color.red);
					Debug.DrawRay(transform.position, targetDir * 50, Color.green);
				}
			}
			currentTarget = null;
		}

	}

	private void FixedUpdate()
	{
		mousePos = Input.mousePosition;
		Ray ray = cam.ScreenPointToRay(mousePos);
		RaycastHit hit;
		float step = speed * Time.deltaTime;

		if (Physics.SphereCast(ray, aimAssistWidth, out hit, aimAssistDistance, targetMask))
		{
			Vector3 targetDir = hit.transform.position - transform.position;
			Debug.DrawRay(transform.position, targetDir * 50, Color.green);
			targetFound = true;
			currentTarget = hit.transform.gameObject;

			foreach (Transform weapon in weapons)
			{
				// Rotate weapons to be parallel with weapon center
				Vector3 newDir = Vector3.RotateTowards(weapon.forward, targetDir, step, 0.0F);
				targetRotation = Quaternion.LookRotation(newDir, transform.up);
				weapon.rotation = targetRotation;

				// Debug
				if (debugRays)
				{
					Debug.DrawRay(weapon.position, newDir * debugRayLength, Color.red);
					Debug.DrawRay(weapon.position, newDir * debugRayLength, Color.red);
				}
			}
		}
		else { targetFound = false; }
	}

}
