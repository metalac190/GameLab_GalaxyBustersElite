using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnergyBurst : WeaponBase
{
	private List<GameObject> projectilePool = new List<GameObject>();
	private float cdTime = 0f;

	[Header("Hold Fire Settings")]
	[SerializeField] float chargeTime = 0.5f;
	[SerializeField] float damageMultiplier = 1f;

	[Header("Effects")]
	[SerializeField] UnityEvent OnWeaponCharged;

	private void Awake()
	{
		overloaded = false;
	}

	void Update()
	{
		chargeMeter = GameManager.player.controller.GetOverloadCharge();

		// TODO: Add slight bonus for clicking rapidly over holding fire
		if (Input.GetButton("Primary Fire") && !overloaded && GameManager.gm.currentState == GameState.Gameplay && !GameManager.gm.Paused)
		{
			FireEnergy();
		}

		if (Input.GetButton("Overload Fire") && chargeMeter >= meterRequired && !overloaded && GameManager.gm.currentState == GameState.Gameplay && !GameManager.gm.Paused)
		{
			GameManager.player.controller.SetOverload(chargeMeter - meterRequired);

			// Start the overload countdown
			StartCoroutine("ActivateOverload");
			StartCoroutine("EnergyOverload");

		}

	}

	void FireEnergy()
	{
		// Set fire rate based on a cooldown/overload multiplier
		if (Time.time - cdTime > 1 / (overloaded ? 2f : 1f))
		{
			cdTime = Time.time;

			// Instantiate projectile at each spawn point
			foreach (Transform point in spawnPoints)
			{
				// Create random rotation within cone
				Quaternion randAng = Quaternion.Euler(Random.Range(projectileCone * -1, projectileCone), Random.Range(projectileCone * -1, projectileCone), 0);

				//Object Pooling instead of Instantiate
				//GameObject bulletObj = Instantiate(projectile, point.position, point.rotation);
				GameObject bulletObj = PoolUtility.InstantiateFromPool(projectilePool, point.position, point.rotation * randAng, projectile);

				// Set instantiated projectile's speed and damage
				bulletObj.GetComponent<Projectile>().SetVelocity(projectileSpeed);
				bulletObj.GetComponent<Projectile>().SetDamage(damage);
			}

			OnStandardFire.Invoke();
		}
	}

	IEnumerator EnergyOverload()
	{
		InvokeRepeating("FireEnergy", 0f, 0.05f);
		yield return new WaitForSeconds(overloadTime);
		CancelInvoke();
	}

	public override void DeactivateOverload()
	{
		overloaded = false;
		StopCoroutine("ActivateOverload");
		StopCoroutine("EnergyOverload");
		CancelInvoke();
	}

}
