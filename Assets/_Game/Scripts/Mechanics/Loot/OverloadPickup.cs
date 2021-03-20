using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverloadPickup : PickupBase
{
    //Refer to Ben Friedman for QA/Bugfixing on Loot System

    //health amount should be standardized by a prefab, but could add functionality for varaible health values through the loot system?
    [SerializeField] private float _overchargeValue = 1;

    protected override void ApplyEffect(PlayerController player)
    {
        player.IncreaseOverload(_overchargeValue);

        //Invoke Event, Destroy Object
        base.ApplyEffect(player);
    }

    public void SetOverloadValue(float value)
    {
        _overchargeValue = value;
    }
}
