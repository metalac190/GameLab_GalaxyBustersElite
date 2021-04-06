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
	[SerializeField] float projectileSpeed = 40f;
	[SerializeField] float fireRate = 2f;

	[Header("Hold Fire Settings")]
	[SerializeField] GameObject chargedProjectile;
	[SerializeField] float chargeUpTime = 0.5f;
	[SerializeField] float damageMultiplier = 2f;
	[SerializeField] float speedMultiplier = 1.5f;
	[SerializeField] private bool weaponCharged;
	[SerializeField] private float minScale = 1f;
	[SerializeField] private float maxScale = 1.75f;
	[SerializeField] private Color chargeStartColor;
	[SerializeField] private Color chargeEndColor;
	private Gradient chargeColorGradient = new Gradient();

	[Header("Effects")]
	[SerializeField] UnityEvent OnWeaponCharged;
	[SerializeField] UnityEvent OnChargedFire;

	private GameObject chargingShot;
	private MeshRenderer shotRenderer;
	private Projectile shotProjectile;

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

		GradientColorKey[] colorKeys = new GradientColorKey[2];
		GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
		colorKeys[0] = new GradientColorKey(chargeStartColor, 0);
		colorKeys[1] = new GradientColorKey(chargeEndColor, 1);
		alphaKeys[0] = new GradientAlphaKey(chargeStartColor.a, 0);
		alphaKeys[1] = new GradientAlphaKey(chargeEndColor.a, 1);
		chargeColorGradient.SetKeys(colorKeys, alphaKeys);

		// TODO while charging spawn projectile but w 0 speed and following player
		// released when released
	}

	void Update()
	{
		fireReady = (GameManager.gm.currentState == GameState.Gameplay && !GameManager.gm.Paused);
		chargeMeter = GameManager.player.controller.GetOverloadCharge();

		if (Input.GetButtonDown("Primary Fire") && !overloaded && fireReady)
		{
			foreach (Transform point in spawnPoints)
			{
				chargingShot = PoolUtility.InstantiateFromPool(projectilePool, point.position, point.rotation, projectile);
				shotRenderer = chargingShot.GetComponent<MeshRenderer>();
				shotProjectile = chargingShot.GetComponent<Projectile>();

				shotProjectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
				chargingShot.transform.localScale = new Vector3(minScale, minScale, minScale);
				shotProjectile.enabled = false;
			}
		}

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

	private void FixedUpdate()
	{
		if (Input.GetButton("Primary Fire") && !overloaded && fireReady)
		{
			// Sets color based on charge time
			shotRenderer.material.SetColor("_UnlitColor", chargeColorGradient.Evaluate(chargeTimer / chargeUpTime));
			// Sets scale based on charge time
			float shotScale = Mathf.Lerp(minScale, maxScale, chargeTimer / chargeUpTime);
			chargingShot.transform.localScale = new Vector3(shotScale, shotScale, shotScale);
		}
	}

	private void LateUpdate()
	{
		if (Input.GetButton("Primary Fire") && !overloaded && fireReady)
		{
			chargingShot.transform.rotation = spawnPoints[0].transform.rotation;
			chargingShot.transform.position = spawnPoints[0].transform.position;
		}
	}

	void FireEnergy()
	{
		// Set fire rate based on a cooldown
		if (Time.time - cdTime > 1 / fireRate)
		{
			cdTime = Time.time;

			// Sets damage based on charge time
			shotProjectile.SetDamage(Mathf.Lerp(damage, damage * damageMultiplier, chargeTimer / chargeUpTime));
			// release projectile
			shotProjectile.enabled = true;

			OnStandardFire.Invoke();
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
