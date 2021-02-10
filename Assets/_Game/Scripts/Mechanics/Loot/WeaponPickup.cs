using UnityEngine;
using System.Collections;

public class WeaponPickup : PickupBase
{
    //Refer to Ben Friedman for QA/Bugfixing on WeaponPickup

    //TODO reference PlayerBase/PlayerWeapon scripts?
    //  Can I get the reference from the OnTrigger, ApplyEffect chain?

    //TODO private weaponBase myWeaponType = foo;

    /// <summary> Applies new WeaponType to PlayerBase
    /// 
    /// </summary>
    protected override void ApplyEffect()
    {
        //TODO implement WeaponSwap functionality
        //PlayerWeapon = myWeaponType;

        //UnityEvent.Invoke() + Destroy()
        base.ApplyEffect();
    }
}
