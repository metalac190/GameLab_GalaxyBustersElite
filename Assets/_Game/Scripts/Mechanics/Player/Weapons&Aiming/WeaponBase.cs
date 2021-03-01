using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for creating player weapons and modifying how they behave
// Refer to Xander Youssef with questions
public class WeaponBase : MonoBehaviour
{

	// TODO: Customize editor so only applicable settings will show with each projectile type

	[Header("Weapon Settings")]
	public Projectiles projectileType;
	[SerializeField] GameObject projectile;
	public Transform[] spawnPoints;

	[Header("Fire Settings")]
	[SerializeField] float clickCooldown = 0.5f;
	private float cdTime = 0f;
	[SerializeField] float damage = 3f;
	[SerializeField] float projectileSpeed = 40f;
	[SerializeField] [Range(0, 45)] float projectileCone = 6f;

	[Header("Hold Fire Settings")]
	[SerializeField] float fireRate = 3f;
	[SerializeField] bool chargeWeapon = false;
	[SerializeField] bool tickDamage = false;
	[SerializeField] float damageMultiplier = 1f;

	[Header("Overload Settings")]
	[SerializeField] float fireRateMultiplier = 2f;
	[SerializeField] float meterRequired = 50f;
	[SerializeField] float overloadTime = 2.5f;
	private bool overloaded = false;
	float chargeMeter = 0f;

	private void Awake()
	{
		overloaded = false;
	}

	void Update()
	{

		// TODO: Add slight bonus for clicking rapidly over holding fire
		if (Input.GetButton("Primary Fire") && !overloaded)
		{
			switch (projectileType)
			{
				case Projectiles.bullet:
					FireBullet();
					break;

				case Projectiles.energy:
					FireEnergy();
					break;

				case Projectiles.laser:
					FireLaser();
					break;
			}

		}

		if (Input.GetButton("Overload Fire") && !overloaded)
		{
			// Start the overload countdown
			StartCoroutine("ActivateOverload");

			switch (projectileType)
			{
				case Projectiles.bullet:
					StartCoroutine("BulletOverload");
					break;

				case Projectiles.energy:
					FireEnergy();
					break;

				case Projectiles.laser:
					FireLaser();
					break;
			}

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

				// Instantiate projectile
				GameObject bulletObj = Instantiate(projectile, point.position, point.rotation * randAng);

				// Set instantiated projectile's speed and damage
				bulletObj.GetComponent<Projectile>().speed = projectileSpeed;
				bulletObj.GetComponent<Projectile>().damage = damage;
			}
		}
	}

	IEnumerator BulletOverload()
	{
		InvokeRepeating("FireBullet", 0f, 0.05f);
		yield return new WaitForSeconds(overloadTime);
		CancelInvoke();
	}

	// TODO: Add hold click to charge for energy wave
	void FireEnergy()
	{
		//foreach (Transform point in spawnPoints)
		//{
		//	Instantiate(projectile, point.position, point.rotation);
		//}
	}

	// TODO: Add raycasts for laser firing
	void FireLaser()
	{
		//foreach (Transform point in spawnPoints)
		//{
		//	Instantiate(projectile, point.position, point.rotation);
		//}
	}

	IEnumerator ActivateOverload()
	{
		overloaded = true;
		yield return new WaitForSeconds(overloadTime);
		overloaded = false;
	}

	public void DeactivateOverload()
	{
		overloaded = false;
		StopCoroutine("ActivateOverload");
		StopCoroutine("BulletOverload");
		CancelInvoke();
	}

}
