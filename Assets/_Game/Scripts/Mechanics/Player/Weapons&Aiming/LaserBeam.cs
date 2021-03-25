using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserBeam : WeaponBase
{
	private float cdTime = 0f;
	private bool fireReady, targetFound;
	private LineRenderer line;
	private Transform target;
	private float tickDamage;

	[Header("Hold Fire Settings")]
	[SerializeField] float aimAssistRadius = 2f;
	[SerializeField] float tickRate = 0.2f;
	[SerializeField] float damageMultiplier = 1.3f;

	[Header("Effects")]
	[SerializeField] UnityEvent OnLaserStop;

	[SerializeField] bool laserActive;
	[SerializeField] LayerMask targetLayers;

	private void OnEnable()
	{
		overloaded = false;
		laserActive = false;
		tickDamage = damage;
		line = GetComponent<LineRenderer>();
		line.positionCount = 1;
	}

	void Update()
	{
		fireReady = (GameManager.gm.currentState == GameState.Gameplay && !GameManager.gm.Paused);
		chargeMeter = GameManager.player.controller.GetOverloadCharge();
		line.SetPosition(0, spawnPoints[0].position);

		if (Input.GetButton("Primary Fire") && fireReady)
		{
			TrackTarget();
			FireLaser();
		}

		if(Input.GetButtonUp("Primary Fire") && fireReady && laserActive)
		{
			laserActive = false;
			line.positionCount = 1;
			OnLaserStop.Invoke();
		}

		if (Input.GetButton("Overload Fire") && chargeMeter >= meterRequired && !overloaded && fireReady)
		{
			GameManager.player.controller.SetOverload(chargeMeter - meterRequired);

			// Start the overload countdown
			StartCoroutine("ActivateOverload");
			StartCoroutine("LaserOverload");

		}

	}

	void FireLaser()
	{
		// Set fire rate based on time between ticks
		if (Time.time - cdTime >= tickRate)
		{
			cdTime = Time.time;

			if (targetFound)
			{
				line.positionCount = 2;
				line.SetPosition(1, target.position);
				tickDamage *= damageMultiplier;
				target.GetComponent<EnemyBase>().TakeDamage(tickDamage);
				Debug.Log(tickDamage);
				OnStandardFire.Invoke();
			}
			
		}
	}

	void TrackTarget()
	{
		RaycastHit hit = new RaycastHit();
		targetFound = Physics.SphereCast(spawnPoints[0].position, aimAssistRadius, spawnPoints[0].forward, out hit, 50f, targetLayers);

		if (targetFound)
		{
			laserActive = true;
			target = hit.transform;
		}
		else
		{
			laserActive = false;
			target = null;
			line.positionCount = 1;
			tickDamage = damage;
		}
	}

	public bool GetIsLaserActive() { return laserActive; }

	// TODO: Add laser overload behavior
	IEnumerator LaserOverload()
	{
		yield return new WaitForSeconds(overloadTime);
	}

	public override void DeactivateOverload()
	{
		overloaded = false;
		StopCoroutine("ActivateOverload");
		StopCoroutine("LaserOverload");
		CancelInvoke();
	}

}
