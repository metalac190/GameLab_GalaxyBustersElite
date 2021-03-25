using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthPickup : PickupBase
{
    //Refer to Ben Friedman for QA/Bugfixing on Loot System scripts

    //health amount should be standardized by a prefab, but could add functionality for varaible health values through the loot system?
    [SerializeField] private int _healValue = 1;

    protected override void ApplyEffect(PlayerController player)
    {
        player.HealPlayer(_healValue);

        //Invoke Event, Destroy Object
        base.ApplyEffect(player);
    }

    public void SetHealValue(int value)
    {
        _healValue = value;
    }
}
