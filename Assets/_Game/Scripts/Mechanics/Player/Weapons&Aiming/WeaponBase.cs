using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Base class for creating player weapons and modifying how they behave
public class WeaponBase : MonoBehaviour
{

	// TODO: Customize editor so only applicable settings will show with each projectile type

	[Header("Weapon Settings")]
	public string weaponID;
	public Projectiles projectileType;
	[SerializeField] GameObject projectile;
    private List<GameObject> projectilePool = new List<GameObject>();
	public Transform[] spawnPoints;

	[Header("Fire Settings")]
	[SerializeField] float clickCooldown = 0.5f;
	private float cdTime = 0f;
	[SerializeField] int damage = 3;
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

	[Header("Effects")]
	[SerializeField] UnityEvent OnStandardFire;
	[SerializeField] UnityEvent OnOverloadActivated;

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

		if (Input.GetButton("Overload Fire") && chargeMeter >= meterRequired && !overloaded && GameManager.gm.currentState == GameState.Gameplay && !GameManager.gm.Paused)
		{
			GameManager.player.controller.SetOverload(chargeMeter - meterRequired);

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

                //Object Pooling instead of Instantiate
                GameObject bulletObj = PoolUtility.InstantiateFromPool(projectilePool, point.position, point.rotation * randAng, projectile);

                // Set instantiated projectile's speed and damage
                bulletObj.GetComponent<Projectile>().SetVelocity(projectileSpeed);
                bulletObj.GetComponent<Projectile>().SetDamage(damage);
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

	// TODO: Add hold click to charge for energy wave
	void FireEnergy()
	{
		//foreach (Transform point in spawnPoints)
		//{
		//	Instantiate(projectile, point.position, point.rotation);
		//}

		OnStandardFire.Invoke();
	}

	// TODO: Add raycasts for laser firing
	void FireLaser()
	{
		//foreach (Transform point in spawnPoints)
		//{
		//	Instantiate(projectile, point.position, point.rotation);
		//}

		OnStandardFire.Invoke();
	}

	IEnumerator ActivateOverload()
	{
		overloaded = true;
		GameManager.player.controller.TogglePlayerOverloaded(true);
		OnOverloadActivated.Invoke();

		yield return new WaitForSeconds(overloadTime);

		overloaded = false;
		GameManager.player.controller.TogglePlayerOverloaded(false);
	}

	public void DeactivateOverload()
	{
		overloaded = false;
		StopCoroutine("ActivateOverload");
		StopCoroutine("BulletOverload");
		CancelInvoke();
	}

}
