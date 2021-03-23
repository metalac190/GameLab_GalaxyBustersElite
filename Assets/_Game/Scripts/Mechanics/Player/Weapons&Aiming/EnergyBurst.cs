using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnergyBurst : WeaponBase
{
	private List<GameObject> projectilePool = new List<GameObject>();
	private List<GameObject> chargedProjectilePool = new List<GameObject>();
	private float cdTime = 0f;
	private float chargeTimer = 0f;
	private bool fireReady;

	[Header("Primary Fire Settings")]
	[SerializeField] GameObject projectile;
	[SerializeField] float projectileSpeed = 40f;
	[SerializeField] float fireRate = 2f;

	[Header("Hold Fire Settings")]
	[SerializeField] GameObject chargedProjectile;
	[SerializeField] float chargeUpTime = 0.5f;
	[SerializeField] float damageMultiplier = 2f;
	[SerializeField] float speedMultiplier = 1.5f;
	[SerializeField] private bool weaponCharged;

	[Header("Effects")]
	[SerializeField] UnityEvent OnWeaponCharged;
	[SerializeField] UnityEvent OnChargedFire;

	private void OnEnable()
	{
		overloaded = false;
		weaponCharged = false;

		// Set instantiated projectile's speed and damage
		projectile.GetComponent<Projectile>().SetVelocity(projectileSpeed);
		projectile.GetComponent<Projectile>().SetDamage(damage);

		// Set charged projectile's speed and damage
		chargedProjectile.GetComponent<Projectile>().SetVelocity(projectileSpeed * speedMultiplier);
		chargedProjectile.GetComponent<Projectile>().SetDamage(damage * damageMultiplier);
	}

	void Update()
	{
		fireReady = (GameManager.gm.currentState == GameState.Gameplay && !GameManager.gm.Paused);
		chargeMeter = GameManager.player.controller.GetOverloadCharge();

		if (Input.GetButton("Primary Fire") && !overloaded && fireReady)
		{
			chargeTimer += Time.deltaTime;
		}

		if(chargeTimer >= chargeUpTime && !overloaded && !weaponCharged)
		{
			weaponCharged = true;
			OnWeaponCharged.Invoke();
		}
		else if (chargeTimer <= chargeUpTime && !overloaded)
		{
			weaponCharged = false;
		}

		if (Input.GetButtonUp("Primary Fire") && fireReady)
		{
			FireEnergy();
			chargeTimer = 0;
		}

		if (Input.GetButton("Overload Fire") && chargeMeter >= meterRequired && !overloaded && fireReady)
		{
			GameManager.player.controller.SetOverload(chargeMeter - meterRequired);

			// Start the overload countdown
			StartCoroutine("ActivateOverload");
			StartCoroutine("EnergyOverload");

		}

	}

	void FireEnergy()
	{
		// Set fire rate based on a cooldown
		if (Time.time - cdTime > 1 / fireRate)
		{
			cdTime = Time.time;

			if (weaponCharged)
			{
				// Instantiate projectile at each spawn point
				foreach (Transform point in spawnPoints)
				{
					//Object Pooling instead of Instantiate
					GameObject bulletObj = PoolUtility.InstantiateFromPool(chargedProjectilePool, point.position, point.rotation, chargedProjectile);
				}

				OnStandardFire.Invoke();
			}
			else
			{
				// Instantiate projectile at each spawn point
				foreach (Transform point in spawnPoints)
				{
					//Object Pooling instead of Instantiate
					GameObject bulletObj = PoolUtility.InstantiateFromPool(projectilePool, point.position, point.rotation, projectile);
				}

				OnStandardFire.Invoke();
			}
		}
	}

	public bool IsWeaponCharged()
	{
		return weaponCharged;
	}

	IEnumerator EnergyOverload()
	{
		weaponCharged = true;
		yield return new WaitForSeconds(overloadTime);
		weaponCharged = false;
	}

	public override void DeactivateOverload()
	{
		overloaded = false;
		StopCoroutine("ActivateOverload");
		StopCoroutine("EnergyOverload");
		CancelInvoke();
	}

}
