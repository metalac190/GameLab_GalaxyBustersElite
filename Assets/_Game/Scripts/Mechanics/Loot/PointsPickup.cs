using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PointsPickup : PickupBase
{
    //Refer to Ben Friedman for QA/Bugfixing on PointsPickup

    /* Current Flow:
     * Box is Hit
     * Spawns PointsPickup
     * Box Breaks
     * PointsPickup calls GameManager, increases Points total
     * Points generates VFX, SFX
     * 
     * Alternative:
     * Box is Hit
     * Box calls GameManger, generates VFX, SFX
     * Box Breaks
     */

    /* Pros to Current:
     * PointsDrop is consistent with WeaponsDrop
     * 
     * Cons to Current:
     * Unecessary objects in scene with short lifespan
     */

    /* Pros to Alternative:
     * Calls Manager -in addition- to WeaponsDrop functionality
     * Can drop Points + Weapons 
     * 
     * Cons to Alternative:
     * Might be redundant? 
     * Ask designers if Weapons give Points? 
     */

    //override with UtilityScript IntEvent PointsIncrease.Invoke(int), etc
    public UnityEvent PointsIncrease;
    //doesn't collide, doesn't need an event, calls manager directly??

    //TODO reference to Points system
    //GameManager manager = GameManager.STATIC //singleton implementation

    [Tooltip("Points gained when picked up")]
    [SerializeField] private int pointAmount = 10;

    /// <summary> Initializes PointAmount
    /// <para> 
    ///     Only use when instantiating a prefab that contians this script.
    ///     Do NOT update dynamically
    ///     
    ///     See if this value can be contained in some sort of struct, or Constructor method
    /// </para>
    /// 
    /// </summary>
    /// <param name="value"> New pointAmount value </param>
    public void SetPointAmount(int value)
    {
        //should be affected during Construction
        //research: instantiate(new object, with component<Points>(pointsAmount)) style construction?
        pointAmount = value;
    }


    /// <summary> Increments Points in PointSystem/PlayerSystem
    /// 
    /// </summary>
    public void GivePoints()
    {
        //TODO: 
        //manager.AddPoints(pointAmount);
        //PointsIncrease.Invoke(pointAmount); //manager is listener?
        Debug.Log("Player has been awarded " + pointAmount + " Points!");

        //Calls VFX, SFX
        //Destroy self      //is FX child entities (must time out destroy) or non-child entities (destroy after calling)?
        Destroy(this.gameObject);
    }
}
