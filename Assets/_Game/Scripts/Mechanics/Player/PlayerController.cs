using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float playerHealth = 10f;
	[SerializeField] float overloadCharge = 0f;
	[SerializeField] GameObject currentWeapon;
	public GameObject[] weapons;

    private void Awake() {
		// Set references in game manager
		GameManager.player.obj = gameObject;
		GameManager.player.movement = GetComponent<PlayerMovement>();
		GameManager.player.controller = this;
    }

    private void Start()
	{
		SetWeapon(currentWeapon);
	}

	void Update()
    {
		// Temporary manual weapon switching for testing purposes
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			SetWeapon(weapons[0]);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			SetWeapon(weapons[1]);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			SetWeapon(weapons[2]);
		}

	}

	public void DamagePlayer(float amount)
	{
		playerHealth -= amount;
	}

	public void HealPlayer(float amount)
	{
		playerHealth += amount;
	}

	public void SetWeapon(GameObject newWeapon)
	{
		// Find and activate/deactivate necessary weapons
		foreach (GameObject weapon in weapons)
		{
			if (weapon.GetComponent<WeaponBase>().weaponID == newWeapon.GetComponent<WeaponBase>().weaponID)
			{
				weapon.SetActive(true);
				currentWeapon = newWeapon;
			}
			else
			{
				// Stop overload if currently active
				weapon.GetComponent<WeaponBase>().DeactivateOverload();

				// Deactivate GameObject
				weapon.SetActive(false);
			}
		}
	}
}
