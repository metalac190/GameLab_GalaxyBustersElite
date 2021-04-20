using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnergyBurst : WeaponBase
{
	private List<GameObject> projectilePool = new List<GameObject>();
	private float cdTime = 0f;
	private float chargeTimer = 0f;
	private bool fireReady;
	private Vector3 newScale;

	[Header("Primary Fire Settings")]
	[SerializeField] float projectileSpeed = 40f;
	[SerializeField] float fireRate = 2f;

	[Header("Hold Fire Settings")]
	[SerializeField] float chargeUpTime = 0.5f;
	[SerializeField] float damageMultiplier = 2f;
	[SerializeField] float speedMultiplier = 1.5f;
	[SerializeField] private bool weaponCharged;
	[SerializeField] private float minScale = 1f;
	[SerializeField] private float maxScale = 1.75f;
	[SerializeField] private Color chargeStartColor;
	[SerializeField] private Color chargeEndColor;
	private Gradient chargeColorGradient = new Gradient();
	[SerializeField] private int overloadChargeSpeedMultiplier = 4;

	[Header("Effects")]
	[SerializeField] UnityEvent OnWeaponCharged;
	[SerializeField] UnityEvent OnChargedFire;
	private GameObject chargingShot;
	private MeshRenderer shotRenderer;
	private EnergyWave shotProjectile;

	private void OnEnable()
	{
		overloaded = false;
		weaponCharged = false;

		GradientColorKey[] colorKeys = new GradientColorKey[2];
		GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
		colorKeys[0] = new GradientColorKey(chargeStartColor, 0);
		colorKeys[1] = new GradientColorKey(chargeEndColor, 1);
		alphaKeys[0] = new GradientAlphaKey(chargeStartColor.a, 0);
		alphaKeys[1] = new GradientAlphaKey(chargeEndColor.a, 1);
		chargeColorGradient.SetKeys(colorKeys, alphaKeys);
	}

	void Update()
	{
		fireReady = (GameManager.gm.currentState == GameState.Gameplay && !GameManager.gm.Paused);
		chargeMeter = GameManager.player.controller.GetOverloadCharge();

		if (Input.GetButtonDown("Primary Fire") && fireReady)
		{
			// Set fire rate based on a cooldown
			if (Time.time - cdTime > 1 / fireRate)
			{
				cdTime = Time.time;

				foreach (Transform point in spawnPoints)
				{
					chargingShot = PoolUtility.InstantiateFromPool(projectilePool, point.position, point.rotation, projectile);
					shotRenderer = chargingShot.GetComponent<MeshRenderer>();
					shotProjectile = chargingShot.GetComponent<EnergyWave>();
					newScale = new Vector3(minScale, minScale, minScale);
					chargingShot.transform.localScale = newScale;
					chargingShot.GetComponent<EnergyWave>().projectileVFX.transform.localScale = newScale;
				}
			}
		}

		if (Input.GetButton("Primary Fire") && fireReady)
		{
			chargeTimer += Time.deltaTime * (overloaded ? overloadChargeSpeedMultiplier : 1);
		}

		if(chargeTimer >= chargeUpTime && !weaponCharged)
		{
			weaponCharged = true;
			OnWeaponCharged.Invoke();
		}
		else if (chargeTimer <= chargeUpTime)
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
		}

	}

	private void FixedUpdate()
	{
		if (Input.GetButton("Primary Fire") && fireReady && chargingShot != null)
		{
			// Sets color based on charge time
			shotRenderer.material.SetColor("_UnlitColor", chargeColorGradient.Evaluate(chargeTimer / chargeUpTime));
			// Sets scale based on charge time
			float shotScale = Mathf.Lerp(minScale, maxScale, chargeTimer / chargeUpTime);
			newScale = new Vector3(shotScale, shotScale, shotScale);
			chargingShot.transform.localScale = newScale;
			chargingShot.GetComponent<EnergyWave>().projectileVFX.transform.localScale = newScale;
		}
	}

	private void LateUpdate()
	{
		if (Input.GetButton("Primary Fire") && fireReady && chargingShot != null)
		{
			chargingShot.transform.rotation = spawnPoints[0].transform.rotation;
			chargingShot.transform.position = spawnPoints[0].transform.position;
		}
	}

	void FireEnergy()
	{
		if (chargingShot != null)
		{
			float chargeAmount = chargeTimer / chargeUpTime;

			// Set damage and speed based on charge time
			float finalDamage = Mathf.Lerp(damage, damage * damageMultiplier, chargeAmount);
			float finalSpeed = Mathf.Lerp(projectileSpeed, projectileSpeed * speedMultiplier, chargeAmount);
			float finalSize = Mathf.Lerp(minScale, maxScale, chargeTimer / chargeUpTime);

			// Release projectile
			shotProjectile.ActivateProjectile(chargeAmount, finalDamage, finalSpeed, finalSize);

			chargingShot = null;
			OnStandardFire.Invoke();
		}
	}

	public bool IsWeaponCharged()
	{
		return weaponCharged;
	}

	public override void DeactivateOverload()
	{
		overloaded = false;
		StopCoroutine("ActivateOverload");
		//StopCoroutine("EnergyOverload");
		CancelInvoke();
	}

}
