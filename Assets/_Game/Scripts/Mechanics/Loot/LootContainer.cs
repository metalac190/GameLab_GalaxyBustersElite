using UnityEngine;
using System.Collections;
using System;

public class LootContainer : BreakableBase
{
    //Refer to Ben Friedman for QA/Bugfixing on LootContainer

    //TODO create a reference array for each PickUp prefab

    [Header("Loot Drop Chance")]
    [Tooltip("Chance out of 100 to drop Weapon A")]
    [SerializeField] private float WeaponAChance = 0f;

    [Tooltip("Chance out of 100 to drop Weapon B")]
    [SerializeField] private float WeaponBChance = 0f;

    [Tooltip("Chance out of 100 to drop Weapon C")]
    [SerializeField] private float WeaponCChance = 0f;

    [Tooltip("Chance out of 100 to drop any Points")]
    [SerializeField] private float PointsChance = 100f;

    [Header("Coin Drop Range")]
    [Tooltip("Smallest Number of Points, when dropped")]
    [SerializeField] private int MinPointsDrop = 0;

    [Tooltip("Largest Number of Points, when dropped\nIf 0 or less than Smallest, always drops Smallest instead")]
    [SerializeField] private int MaxPointsDrop = 100;

    /// <summary> Implements LootDropping functionality in addition to Break()
    ///
    /// </summary>
    public override void Break()
    {
        //TODO implement loot drop
        RollDropChance();

        base.Break();
    }

    /// <summary> Compares all drop chances, and picks loot (if any) to drop
    /// 
    /// </summary>
    private void RollDropChance()
    {
        float[] allDrops = { WeaponAChance, WeaponBChance, WeaponCChance, PointsChance };
        float totalDrop = WeaponAChance + WeaponBChance + WeaponCChance + PointsChance;

        //if Designer inputs 50% A + 0% else, total would be 50, instead boost total to 100
        //if Designer inputs 100% across the board, chances are compared against 400, and NOT drop all 4 items
        if (totalDrop < 100)
            totalDrop = 100;

        float thisRoll = UnityEngine.Random.Range(0f, totalDrop);

        //create
        for (int i=0; i < allDrops.Length; i++)
        {
            float cumulativeRollChance = 0f;
            
            //all previous drops, including self
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
    ///     Loot is organized in a reference array following WeaponA, B, C, Points
    ///     Implementation for droping Weapon versus Points needs to be handled
    /// </para>
    /// 
    /// </summary>
    /// <param name="lootReference"> Reference integer in LootReferenceArray for a specific loot Pickup</param>
    private void DropLoot(int lootReference)
    {
        //TODO instantitate(referenceArray[lootReference]);
        Debug.Log("Dropping Loot Number " + lootReference);
    }
}
