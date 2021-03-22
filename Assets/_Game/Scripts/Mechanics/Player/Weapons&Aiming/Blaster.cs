using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Blaster : WeaponBase
{
	private List<GameObject> projectilePool = new List<GameObject>();
	private float cdTime = 0f;
	private bool fireReady;

	[Header("Primary Fire Settings")]
	[SerializeField] float projectileSpeed = 200f;
	[SerializeField] [Range(0, 45)] float projectileCone = 1f;
	[SerializeField] float clickCooldown = 0.5f;

	[Header("Hold Fire Settings")]
	[SerializeField] float fireRate = 3f;

	[Header("Overload Settings")]
	[SerializeField] float fireRateMultiplier = 2f;

	private void OnEnable()
	{
		overloaded = false;

		// Set instantiated projectile's speed and damage
		projectile.GetComponent<Projectile>().SetVelocity(projectileSpeed);
		projectile.GetComponent<Projectile>().SetDamage(damage);
	}

	void Update()
	{
		fireReady = (!overloaded && GameManager.gm.currentState == GameState.Gameplay && !GameManager.gm.Paused);
		chargeMeter = GameManager.player.controller.GetOverloadCharge();

		// TODO: Add slight bonus for clicking rapidly over holding fire
		if (Input.GetButton("Primary Fire") && fireReady)
		{
			FireBullet();
		}

		if (Input.GetButton("Overload Fire") && chargeMeter >= meterRequired && fireReady)
		{
			GameManager.player.controller.SetOverload(chargeMeter - meterRequired);

			// Start the overload countdown
			StartCoroutine("ActivateOverload");
			StartCoroutine("BulletOverload");

		}

	}

	void FireBullet()
	{
		// Set fire rate based on a cooldown/overload multiplier
		if (Time.time - cdTime > 1 / (overloaded ? fireRate * fireRateMultiplier : fireRate))
		{
			cdTime = Time.time;

			// Instantiate projectile at each spawn point
			foreach (Transform point in spawnPoints)
			{
				// Create random rotation within cone
				Quaternion randAng = Quaternion.Euler(Random.Range(projectileCone * -1, projectileCone), Random.Range(projectileCone * -1, projectileCone), 0);

				//Object Pooling instead of Instantiate
				GameObject bulletObj = PoolUtility.InstantiateFromPool(projectilePool, point.position, point.rotation * randAng, projectile);
			}

			OnStandardFire.Invoke();
		}
	}

	IEnumerator BulletOverload()
	{
		InvokeRepeating("FireBullet", 0f, 0.05f);
		yield return new WaitForSeconds(overloadTime);
		CancelInvoke();
	}

	public override void DeactivateOverload()
	{
		overloaded = false;
		StopCoroutine("ActivateOverload");
		StopCoroutine("BulletOverload");
		CancelInvoke();
	}
}
