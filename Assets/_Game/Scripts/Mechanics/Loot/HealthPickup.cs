using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthPickup : PickupBase
{
    public UnityEvent healthPickedUp;

    [SerializeField] private int healthGain = 1;

    protected override void ApplyEffect()
    {
        //TODO Gain Health

        //Invoke Event, Destroy Object
        base.ApplyEffect();
    }
}
