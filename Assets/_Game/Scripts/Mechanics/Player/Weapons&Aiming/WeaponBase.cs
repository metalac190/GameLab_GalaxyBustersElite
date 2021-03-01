using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{

	// TODO: Customize editor so only applicable settings will show with each projectile type

	[Header("Weapon Settings")]
	public Projectiles projectileType;
	[SerializeField] GameObject projectile;
	public Transform[] spawnPoints;

	[Header("Click Fire Settings")]
	[SerializeField] float clickCooldown = 0.5f;
	private float cdTime = 0f;
	[SerializeField] float damage = 3f;
	[SerializeField] [Range(1,10)] int projectileSpeed = 5;
	[SerializeField] [Range(0, 180)] float projectileCone = 6f;

	[Header("Hold Fire Settings")]
	[SerializeField] float fireRate = 3f;
	[SerializeField] bool tickDamage = false;
	[SerializeField] float damageMultiplier = 1f;

	[Header("Overload Settings")]
	[SerializeField] float fireRateMultiplier = 2f;
	[SerializeField] float meterRequired = 50f;
	[SerializeField] float overloadTime = 2.5f;
	private bool overloaded = false;
	float chargeMeter = 0f;


	void Update()
	{

		// TODO: Add slight bonus for clicking rapidly over holding fire
		if (Input.GetButton("Primary Fire") && !overloaded)
		{
			switch (projectileType)
			{
				case Projectiles.bullet:
					FireBullet(projectile);
					break;

				case Projectiles.energy:
					FireEnergy(projectile);
					break;

				case Projectiles.laser:
					FireLaser(projectile);
					break;
			}

		}

		if (Input.GetButton("Overload Fire"))
		{
			switch (projectileType)
			{
				case Projectiles.bullet:
					overloaded = true;
					FireBullet(projectile);
					overloaded = false;
					break;

				case Projectiles.energy:
					FireEnergy(projectile);
					break;

				case Projectiles.laser:
					FireLaser(projectile);
					break;
			}

		}

	}


	void FireBullet(GameObject bullet)
	{
		if (Time.time - cdTime > 1 / (overloaded ? fireRate * fireRateMultiplier : fireRate))
		{
			cdTime = Time.time;
			foreach (Transform point in spawnPoints)
			{
				Instantiate(bullet, point.position, point.rotation);
			}
		}
	}

	// TODO: Add hold click to charge for energy wave
	void FireEnergy(GameObject wave)
	{
		//foreach (Transform point in spawnPoints)
		//{
		//	Instantiate(wave, point.position, point.rotation);
		//}
	}

	// TODO: Add raycasts for laser firing
	void FireLaser(GameObject laser)
	{
		//foreach (Transform point in spawnPoints)
		//{
		//	Instantiate(laser, point.position, point.rotation);
		//}
	}

	public enum Projectiles
	{
		bullet,
		energy,
		laser
	};
}
