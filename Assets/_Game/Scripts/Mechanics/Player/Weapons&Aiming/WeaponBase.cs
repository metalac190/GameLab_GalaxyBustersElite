using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Base class for creating player weapons and modifying how they behave
public class WeaponBase : MonoBehaviour
{

	[Header("Weapon Settings")]
	public string weaponID;
	[SerializeField] protected GameObject projectile;
	public Transform[] spawnPoints;

	[Header("Primary Fire Settings")]
	[SerializeField] protected float damage = 3;

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
