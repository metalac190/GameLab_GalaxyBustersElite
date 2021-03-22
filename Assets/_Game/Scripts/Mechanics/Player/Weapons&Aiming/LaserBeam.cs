using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserBeam : WeaponBase
{
	private float cdTime = 0f;
	private bool fireReady;

	[Header("Hold Fire Settings")]
	[SerializeField] float tickRate = 0.2f;
	[SerializeField] float damageMultiplier = 1.3f;

	[Header("Effects")]
	[SerializeField] UnityEvent OnLaserStop;

	private void Awake()
	{
		overloaded = false;
	}

	void Update()
	{
		fireReady = (!overloaded && GameManager.gm.currentState == GameState.Gameplay && !GameManager.gm.Paused);
		chargeMeter = GameManager.player.controller.GetOverloadCharge();

		if (Input.GetButton("Primary Fire") && fireReady)
		{
			FireLaser();
		}

		if (Input.GetButton("Overload Fire") && chargeMeter >= meterRequired && fireReady)
		{
			GameManager.player.controller.SetOverload(chargeMeter - meterRequired);

			// Start the overload countdown
			StartCoroutine("ActivateOverload");
			StartCoroutine("EnergyOverload");

		}

	}

	void FireLaser()
	{
		// Set fire rate based on a cooldown/overload multiplier
		if (Time.time - cdTime > 1 / (overloaded ? 2f : 1f))
		{
			cdTime = Time.time;

			// Instantiate projectile at each spawn point
			foreach (Transform point in spawnPoints)
			{
				
			}

			OnStandardFire.Invoke();
		}
	}

	IEnumerator LaserOverload()
	{
		InvokeRepeating("FireLaser", 0f, 0.05f);
		yield return new WaitForSeconds(overloadTime);
		CancelInvoke();
	}

	public override void DeactivateOverload()
	{
		overloaded = false;
		StopCoroutine("ActivateOverload");
		StopCoroutine("LaserOverload");
		CancelInvoke();
	}

}
