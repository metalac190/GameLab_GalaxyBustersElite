using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
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
	[SerializeField] float fireRate = 6f;
	[SerializeField] bool chargeWeapon = false;
	[SerializeField] bool tickDamage = false;
	[SerializeField] float damageMultiplier = 1f;

	[Header("Overload Settings")]
	[SerializeField] float fireRateMultiplier = 4f;
	[SerializeField] float meterRequired = 50f;
	[SerializeField] float overloadTime = 2.5f;
	float chargeMeter = 0f;

	void Start()
    {
        
    }


	void Update()
	{

		if (Input.GetButtonDown("Primary Fire"))
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

	}


	void FireBullet(GameObject bullet)
	{
		foreach (Transform point in spawnPoints)
		{
			Instantiate(bullet, point.position, point.rotation);
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
