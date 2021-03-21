using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Base class for creating player weapons and modifying how they behave
public class WeaponBase : MonoBehaviour
{

	[Header("Weapon Settings")]
	public string weaponID;
	public Projectiles projectileType;
	[SerializeField] protected GameObject projectile;
	public Transform[] spawnPoints;

	[Header("Fire Settings")]
	[SerializeField] protected int damage = 3;
	[SerializeField] protected float projectileSpeed = 40f;
	[SerializeField] [Range(0, 45)] protected float projectileCone = 6f;

	[Header("Overload Settings")]
	[SerializeField] protected float meterRequired = 50f;
	[SerializeField] protected float overloadTime = 2.5f;
	protected bool overloaded = false;
	protected float chargeMeter = 0f;

	[Header("Effects")]
	[SerializeField] protected UnityEvent OnStandardFire;
	[SerializeField] protected UnityEvent OnOverloadActivated;

	IEnumerator ActivateOverload()
	{
		overloaded = true;
		GameManager.player.controller.TogglePlayerOverloaded(true);
		OnOverloadActivated.Invoke();

		yield return new WaitForSeconds(overloadTime);

		overloaded = false;
		GameManager.player.controller.TogglePlayerOverloaded(false);
	}

	public virtual void DeactivateOverload()
	{
		overloaded = false;
		StopCoroutine("ActivateOverload");
		CancelInvoke();
	}

}
