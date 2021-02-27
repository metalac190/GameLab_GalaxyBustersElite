using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
	[Header("Weapon Settings")]
	public Projectiles projectileType;

	[Header("Click Fire Settings")]
	[SerializeField] float clickCooldown = 0.1f;
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

    // Update is called once per frame
    void Update()
    {
        
    }

	public enum Projectiles
	{
		bullet,
		energy,
		laser
	};
}
