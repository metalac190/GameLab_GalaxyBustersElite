using UnityEngine;
using System.Collections;
using System;

public class LootContainer : MonoBehaviour
{
    //Refer to Ben Friedman for QA/Bugfixing on Loot System scripts

    //TODO create a reference array for each PickUp prefab

    [Header("Weapon Drop Settings")]
    [Tooltip("Reference to Each WeaponPickup Prefab")]
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
    [Tooltip("Amount of Health to Heal Player")]
    [SerializeField] private int _healAmount = 1;

    [Header("Overload Drop Settings")]
    [Tooltip("OverloadPickpup Prefab Reference")]
    [SerializeField] private GameObject OVERLOADREF = null;   //Prefab Pickup asset with VFX, SFX, etc

    [Tooltip("Chance out of 100 to drop an Overload Pickup")]
    [SerializeField] private float overloadChance = 100f;
    [Tooltip("Amount of Overload to give Player")]
    [SerializeField] private int _overloadAmount = 10;

    [Header("Coin Drop Settings")]
    [Tooltip("PointsPickup Prefab Reference")]
    [SerializeField] private GameObject POINTSREF = null;   //Prefab Pickup asset with VFX, SFX, etc

    [Tooltip("Chance out of 100 to drop any Points")]
    [SerializeField] private float pointsChance = 100f;
    [Tooltip("Number of Points awarded")]
    [SerializeField] private int pointsAwarded = 0;

    /// <summary> Compares all drop chances, and picks loot (if any) to drop
    /// 
    /// </summary>
    public void RollDropChance()
    {
        float[] allDrops = { WeaponAChance, WeaponBChance, WeaponCChance, healthChance, overloadChance, pointsChance };

        //if Designer inputs 100% across the board, chances are compared against 600, and NOT drop all 6 items
        float totalDrop = WeaponAChance + WeaponBChance + WeaponCChance + healthChance + overloadChance + pointsChance;

        //if Designer inputs 50% A + 0% else, total would be 50, instead boost total to 100
        if (totalDrop < 100)
            totalDrop = 100;

        float thisRoll = UnityEngine.Random.Range(0f, totalDrop);

        for (int i=0; i < allDrops.Length; i++)
        {
            //all previous drops, including self added to cumulative    
            //i.e. A = 10, B = 20;  A on anything less than 10, B on anything less than 30, B drops from 10-30 range
            float cumulativeRollChance = 0f;
            
            for (int j = i; j >= 0; j--)
                cumulativeRollChance += allDrops[j];

            if (thisRoll < cumulativeRollChance)
            {
                DropLoot(i);
                return;
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
        if (lootReference < 3)
        {
            //drops loot, no extras needed
            GameObject droppedWeapon = Instantiate(WEAPONREF_ARRAY[lootReference], transform.position, Quaternion.identity); //Prefab asset with VFX, SFX, etc
            Debug.Log("Dropped Weapon " + lootReference);

        }
        else if(lootReference == 3)
        {
            //ref 3 == Health
            //drops loot, sets heal amount to adjusted number
            GameObject droppedHealth = Instantiate(HEALTHREF, transform.position, Quaternion.identity);
            droppedHealth.GetComponent<HealthPickup>()?.SetHealValue(_healAmount);

            Debug.Log("Dropped Health");

        }
        else if(lootReference == 4)
        {
            //ref 4 == Overcharge
            //drops loot, sets overload amount to adjusted number
            GameObject droppedOverload = Instantiate(OVERLOADREF, transform.position, Quaternion.identity);
            droppedOverload.GetComponent<OverloadPickup>()?.SetOverloadValue(_overloadAmount);

            Debug.Log("Dropped Health");
        }
        else
        {
            //ask designers, should all enemies drop SOME points?
            //else, Coin Pickup
            //drops points, sets point amount, and automatically applies points
            GameObject droppedPoints = Instantiate(POINTSREF, transform.position, Quaternion.identity); //Prefab asset with VFX, SFX, etc
            PointsPickup pointsPickup = droppedPoints.GetComponent<PointsPickup>();

            pointsPickup?.SetPointAmount(pointsAwarded);
            pointsPickup?.GivePoints();

            Debug.Log("Dropping " + pointsAwarded +" Points");
        }
    }
}
