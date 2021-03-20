﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] float playerHealth = 10f;
	[SerializeField] float overloadCharge = 0f;
	[SerializeField] GameObject currentWeapon;
	public GameObject[] weapons;
    bool isDodging = false;
    bool isInvincible = false;

	[Header("Effects")]
	[Range(0, 5)]
	[SerializeField] float cameraShakeOnHit = 1;
	[SerializeField] UnityEvent OnHit;
	[SerializeField] UnityEvent OnDeath;
	[SerializeField] UnityEvent OnPickedUpWeapon;
	[SerializeField] float playerHealthLowThreshold = 1;
	float lastFramePlayerHealth;
	[SerializeField] UnityEvent OnHealthStartedBeingLow;
	[SerializeField] UnityEvent OnHealthStoppedBeingLow;


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

        InvokingHealthStartedOrStoppedBeingLowEvents();
    }

    private void InvokingHealthStartedOrStoppedBeingLowEvents()
    {
        bool currentlyLowHealth = playerHealth < playerHealthLowThreshold;
        bool lastFrameHealthWasLow = lastFramePlayerHealth < playerHealthLowThreshold;

        if (!lastFrameHealthWasLow && currentlyLowHealth)
            OnHealthStartedBeingLow.Invoke();
        else if (lastFrameHealthWasLow && !currentlyLowHealth)
            OnHealthStoppedBeingLow.Invoke();

        lastFramePlayerHealth = playerHealth;
    }

    public void DamagePlayer(float amount)
	{
        if (isDodging || isInvincible) //Not sure if dodging protects form environmental damage; if not, change this
        {
            return;
        }
		playerHealth -= amount;

		CameraShaker.instance.Shake(cameraShakeOnHit);

		OnHit.Invoke();
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

		OnPickedUpWeapon.Invoke();
	}

    public void ToggleDodging(bool dodge)
    {
        isDodging = dodge;
    }

    public void Toggleinvincibility(bool inv)
    {
        isInvincible = inv;
    }
}
