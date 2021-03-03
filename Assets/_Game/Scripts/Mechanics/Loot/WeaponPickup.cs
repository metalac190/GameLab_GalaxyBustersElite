using UnityEngine;
using System.Collections;

public class WeaponPickup : PickupBase
{
    //Refer to Ben Friedman for QA/Bugfixing on WeaponPickup

    //assigned dynamically when used in LootContainer, 
    //but also manually when placed by LevelDesigner in Level
    [Tooltip("Assign Weapon Prefab (asset) to Apply to Player")]
    [SerializeField] private GameObject weaponReference = null;

    //TODO reference PlayerBase/PlayerWeapon scripts?   
    //  //Can I get the reference from the OnTrigger, ApplyEffect chain?

    // private PlayerController or PlayerWeapon player = null;

    /// <summary> Applies new WeaponType to PlayerBase
    /// 
    /// </summary>
    protected override void ApplyEffect()
    {
        //TODO implement WeaponSwap functionality
        //PlayerController/PlayerWeapon ? . SetWeapon(weaponReference);

        base.ApplyEffect(); //UnityEvent.Invoke() + Destroy()
    }

    /// <summary> Updates Weapon Reference
    /// 
    /// </summary>
    /// <param name="reference"> assigned dynamically by LootContainer, or manually by Designer when placed in Level</param>
    public void SetWeaponReference(GameObject reference)
    {
        weaponReference = reference;
    }
}
