using UnityEngine;
using System.Collections;
using System;

public class LootContainer : BreakableBase
{
    //Refer to Ben Friedman for QA/Bugfixing on LootContainer

    //TODO create a reference array for each PickUp prefab

    [Header("Weapon Drop Settings")]
    [Tooltip("Reference to Each Weapon Prefab")]
    [SerializeField] private GameObject[] WEAPONREF_ARRAY = new GameObject[3];    //Prefab Weapon asset for each weapon type

    [Tooltip("Chance out of 100 to drop Weapon A")]
    [SerializeField] private float WeaponAChance = 0f;

    [Tooltip("Chance out of 100 to drop Weapon B")]
    [SerializeField] private float WeaponBChance = 0f;

    [Tooltip("Chance out of 100 to drop Weapon C")]
    [SerializeField] private float WeaponCChance = 0f;


    [Header("Health Drop Settings")]
    [Tooltip("HealthPickup Prefab Reference")]
    [SerializeField] private GameObject HEALTHREF = null;   //Prefab Pickup asset with VFX, SFX, etc

    [Tooltip("Chance out of 100 to drop a Health Pickup")]
    [SerializeField] private float healthChance = 100f;


    [Header("Coin Drop Settings")]
    [Tooltip("PointsPickup Prefab Reference")]
    [SerializeField] private GameObject POINTSREF = null;   //Prefab Pickup asset with VFX, SFX, etc

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
        float[] allDrops = { WeaponAChance, WeaponBChance, WeaponCChance, healthChance, pointsChance };

        //if Designer inputs 100% across the board, chances are compared against 400, and NOT drop all 4 items
        float totalDrop = WeaponAChance + WeaponBChance + WeaponCChance + healthChance + pointsChance;

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
            GameObject droppedWeapon = Instantiate(WEAPONREF_ARRAY[lootReference], transform.position, Quaternion.identity); //Prefab asset with VFX, SFX, etc
            Debug.Log("Dropped Weapon " + lootReference);

        }
        else if(lootReference == 3)
        {
            //ref 3 == Health
            GameObject droppedHealth = Instantiate(HEALTHREF, transform.position, Quaternion.identity);
            Debug.Log("Dropped Health");

        }
        else
        {
            //else, Coin Pickup
            GameObject droppedPoints = Instantiate(POINTSREF, transform.position, Quaternion.identity); //Prefab asset with VFX, SFX, etc
            PointsPickup pointsPickup = droppedPoints.GetComponent<PointsPickup>();
            pointsPickup.SetPointAmount(pointsAwarded);
            pointsPickup.GivePoints();

            Debug.Log("Dropping " + pointsAwarded +" Points");
        }
    }
}
