using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_Player_Overload : PickupBase
{
    PlayerController playercontoller;
    float _overchargeValue = 0;

    private void Start()
    {
        playercontoller = FindObjectOfType<PlayerController>();
    }
    protected override void ApplyEffect(PlayerController player)
    {
        player.IncreaseOverload(_overchargeValue);
        player.SetOverload(_overchargeValue);
        //Invoke Event, Destroy Object
        //base.ApplyEffect(player);
    }

    public void SetOverloadValue(float value)
    {
        _overchargeValue = value;
    }

}
