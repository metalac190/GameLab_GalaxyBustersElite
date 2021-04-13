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
	private List<GameObject> overloadTargets = new List<GameObject>();
	private float tickDamage;
	private Transform firePoint;
	private List<GameObject> beamPool = new List<GameObject>();
	float trackingDistance = 50f;
	float aimAssistRadius = 2f;
	float numTicks;
	float nextDamage;
	LayerMask targetLayers;
	GameObject mouse;
	GameObject targetingMode;

	[Header("Hold Fire Settings")]
	[SerializeField] float damageCap = 5f;
	[SerializeField] float timeToCharge = 5f;
	[SerializeField] float tickRate = 0.1f;
	//[SerializeField] float damageMultiplier = 1.3f;

	[Header("Effects")]
	[SerializeField] UnityEvent OnLaserStop;
	[SerializeField] bool laserActive;
	[SerializeField] UnityEvent OnOverloadFired;

	private void OnEnable()
	{
		firePoint = spawnPoints[0];
		overloaded = false;
		laserActive = false;
		tickDamage = damage;
		numTicks = 0;
		line = projectile.GetComponent<LineRenderer>();
		line.positionCount = 1;
		SetupOverloadCollider();
		trackingDistance = GetComponent<AimWeapons>().aimAssistDistance;
		aimAssistRadius = GetComponent<AimWeapons>().aimAssistWidth;
		targetLayers = GetComponent<AimWeapons>().targetMask;
		mouse = GameObject.Find("Mouse");
		targetingMode = GameManager.gm.HUD.transform.Find("TargetingMode").gameObject;
	}

	void Update()
	{
		fireReady = (GameManager.gm.currentState == GameState.Gameplay && !GameManager.gm.Paused);
		chargeMeter = GameManager.player.controller.GetOverloadCharge();
		line.SetPosition(0, spawnPoints[0].position);

		if (Input.GetButton("Primary Fire") && !overloaded && fireReady)
		{
			TrackTarget();
			FireLaser();
		}

		if(Input.GetButtonUp("Primary Fire") && !overloaded && fireReady)
		{
			laserActive = false;
			numTicks = 0;
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
		laserActive = true;

		// Set fire rate based on time between ticks
		if (Time.time - cdTime >= tickRate)
		{
			cdTime = Time.time;

			if (targetFound)
			{
				//Old exponential damage increase
				//tickDamage = (tickDamage * damageMultiplier >= damageCap) ? damageCap : tickDamage * damageMultiplier;

				//New linear damage increase
				nextDamage = damage + (damageCap - damage) * Mathf.Pow(numTicks * tickRate, 2);
				tickDamage = nextDamage >= damageCap ? damageCap : nextDamage;
				projectile.GetComponent<Beam>().SetTarget(target.gameObject);
				target.GetComponent<EntityBase>().TakeDamage(tickDamage);
				numTicks++;
				OnStandardFire.Invoke();
			}			
		}

		if (!targetFound)
		{
			projectile.GetComponent<Beam>().SetTarget(mouse);
			OnStandardFire.Invoke();
		}
	}

	void FireLaserOverload()
	{

		if (overloadTargets.Count > 0)
		{
			laserActive = true;

			foreach (GameObject enemy in overloadTargets)
			{
				enemy.GetComponent<EntityBase>().TakeDamage(50);
			}

			overloadTargets.Clear();
		}

		laserActive = false;
		line.positionCount = 1;
	}

	void TrackTarget()
	{
		RaycastHit hit = new RaycastHit();
		targetFound = Physics.SphereCast(firePoint.position, aimAssistRadius, firePoint.forward, out hit, 50f, targetLayers);

		if (targetFound)
		{
			target = hit.transform;
		}
		else
		{
			target = null;
			line.positionCount = 1;
			tickDamage = damage;
			numTicks = 0;
		}
	}

	void DrawLasers(bool drawing)
	{
		if (overloadTargets.Count > 0)
		{
			if (drawing)
			{
				// Create all overload beams
				foreach (GameObject enemy in overloadTargets)
				{
					// Create line renderers with object pooling
					GameObject beamObj = PoolUtility.InstantiateFromPool(beamPool, projectile, projectile.transform.parent);
					beamObj.GetComponent<Beam>().SetTarget(enemy);
				}
			}
			// Deactivate all beams
			else
			{
				foreach (GameObject beam in beamPool)
				{
					beam.GetComponent<LineRenderer>().positionCount = 1;
					beam.SetActive(false);
				}
			}
		}
	}

	public bool GetIsLaserActive() { return laserActive; }

	IEnumerator LaserOverload()
	{
		targetingMode.SetActive(true);
		firePoint.GetComponent<GroupTargetDetector>().pauseTracking = false;
		firePoint.GetComponent<GroupTargetDetector>().SetCollider(true);

		yield return new WaitForSeconds(overloadTime - 0.5f);

		targetingMode.SetActive(false);
		overloadTargets = firePoint.GetComponent<GroupTargetDetector>().targets;
		DrawLasers(true);
		firePoint.GetComponent<GroupTargetDetector>().pauseTracking = true;

		if (overloadTargets.Count > 0) OnOverloadFired?.Invoke();

		yield return new WaitForSeconds(0.5f);

		DrawLasers(false);
		FireLaserOverload();
		firePoint.GetComponent<GroupTargetDetector>().SetCollider(false);
	}

	public override void DeactivateOverload()
	{
		overloaded = false;
		StopCoroutine("ActivateOverload");
		CancelInvoke();
	}

	void SetupOverloadCollider()
	{
		CapsuleCollider collider = firePoint.GetComponent<CapsuleCollider>();
		collider.height = trackingDistance;
		collider.center = new Vector3(0, 0, trackingDistance / 2);
		collider.radius = aimAssistRadius;
	}

}
