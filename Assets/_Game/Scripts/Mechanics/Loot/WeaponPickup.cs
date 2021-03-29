using UnityEngine;
using System.Collections;

public class WeaponPickup : PickupBase
{
    //Refer to Ben Friedman for QA/Bugfixing on Loot System scripts

    //assigned dynamically when used in LootContainer, 
    //but also manually when placed by LevelDesigner in Level
    [Tooltip("Assign Weapon Prefab (asset) to Apply to Player")]
    [SerializeField] private GameObject weaponReference = null;

    /// <summary> Applies new WeaponType to PlayerBase
    /// 
    /// </summary>
    protected override void ApplyEffect(PlayerController player)
    {
        //TODO implement WeaponSwap functionality
        //PlayerController/PlayerWeapon ? . SetWeapon(weaponReference);
        player.SetWeapon(weaponReference);

        base.ApplyEffect(player); //UnityEvent.Invoke() + Destroy()
    }

    /// <summary> Updates Weapon Reference
    /// <para> Used procedurally by LootContainer, or manually by a designer.</para>
    /// </summary>
    /// <param name="reference"> assigned dynamically by LootContainer, or manually by Designer when placed in Level</param>
    public void SetWeaponReference(GameObject reference)
    {
        weaponReference = reference;
    }
}
