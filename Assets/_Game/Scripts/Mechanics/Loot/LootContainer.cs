using UnityEngine;
using System.Collections;
using System;

public class LootContainer : BreakableBase
{
    //Refer to Ben Friedman for QA/Bugfixing on LootContainer

    //TODO create a reference array for each PickUp prefab

    [Header("Weapon Drop Settings")]
    [Tooltip("WeaponPickup Prefab Reference")]
    [SerializeField] private GameObject weaponPickupReference = null;   //Prefab Pickup asset with VFX, SFX, etc

    [Tooltip("Reference to Each Weapon Prefab")]
    [SerializeField] private GameObject[] weaponPrefabArray = new GameObject[3];    //Prefab Weapon asset for each weapon type

    [Tooltip("Chance out of 100 to drop Weapon A")]
    [SerializeField] private float WeaponAChance = 0f;

    [Tooltip("Chance out of 100 to drop Weapon B")]
    [SerializeField] private float WeaponBChance = 0f;

    [Tooltip("Chance out of 100 to drop Weapon C")]
    [SerializeField] private float WeaponCChance = 0f;


    [Header("Coin Drop Settings")]
    [Tooltip("PointsPickup Prefab Reference")]
    [SerializeField] private GameObject pointsPikcupReference = null;   //Prefab Pickup asset with VFX, SFX, etc

    [Tooltip("Number of Points awarded")]
    [SerializeField] private int pointsAwarded = 0;

    [Tooltip("Chance out of 100 to drop any Points")]
    [SerializeField] private float pointsChance = 100f;

    
    /// <summary> Implements LootDropping functionality in addition to Break()
    ///
    /// </summary>
    public override void Break()
    {
        RollDropChance();

        base.Break();   //OnBreak.Invoke() + Destroy(this)
    }

    /// <summary> Compares all drop chances, and picks loot (if any) to drop
    /// 
    /// </summary>
    private void RollDropChance()
    {
        float[] allDrops = { WeaponAChance, WeaponBChance, WeaponCChance, pointsChance };

        //if Designer inputs 100% across the board, chances are compared against 400, and NOT drop all 4 items
        float totalDrop = WeaponAChance + WeaponBChance + WeaponCChance + pointsChance;

        //if Designer inputs 50% A + 0% else, total would be 50, instead boost total to 100
        if (totalDrop < 100)
            totalDrop = 100;

        float thisRoll = UnityEngine.Random.Range(0f, totalDrop);

        for (int i=0; i < allDrops.Length; i++)
        {
            float cumulativeRollChance = 0f;
            
            //all previous drops, including self added to cumulative    //i.e. A = 10, B = 20; A on anything less than 10, B on anything less than 30, B drops from 10-30 range
            for (int j = i; j >= 0; j--)
                cumulativeRollChance += allDrops[j];

            if (thisRoll < cumulativeRollChance)
            {
                DropLoot(i);
                break;
            }
        }
    }

    /// <summary> Spawns a Pickup Prefab according to RollDropChance()
    /// <para>
    ///     Loot is organized in a reference array following WeaponA, B, C, Points.
    ///     Implementation for droping Weapon versus Points is handled differently
    /// </para>
    /// 
    /// </summary>
    /// <param name="lootReference"> Reference integer in LootReferenceArray for a specific loot Pickup</param>
    private void DropLoot(int lootReference)
    {
        //TODO instantitate(referenceArray[lootReference]);

        if (lootReference < 3)
        {
            GameObject weaponDrop = Instantiate(weaponPickupReference); //Prefab asset with VFX, SFX, etc
            WeaponPickup droppedWeapon = weaponDrop.GetComponent<WeaponPickup>();
            droppedWeapon.SetWeaponReference(weaponPrefabArray[lootReference]);

            Debug.Log("Dropping Weapon" + lootReference);
        }
        else
        {
            GameObject pointsDrop = Instantiate(pointsPikcupReference); //Prefab asset with VFX, SFX, etc
            PointsPickup droppedPoints = pointsDrop.GetComponent<PointsPickup>();
            droppedPoints.SetPointAmount(pointsAwarded);
            droppedPoints.GivePoints();

            Debug.Log("Dropping " + pointsAwarded +" Points");
        }
    }
}
