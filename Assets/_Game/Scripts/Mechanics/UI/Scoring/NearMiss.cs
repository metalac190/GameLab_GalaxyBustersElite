using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NearMiss : MonoBehaviour
{
	[SerializeField] float shakeAmtOnNearMiss = 0.15f;

	private void Awake()
	{
		GetComponent<Collider>().isTrigger = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Projectile") || other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
		{
			ScoreSystem.NearMiss();
			Debug.Log("Near Miss");

			CameraShaker.instance.Shake(shakeAmtOnNearMiss);
		}
	}
}
