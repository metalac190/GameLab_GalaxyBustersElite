using UnityEngine;
using System.Collections;

public class PointsPickup : PickupBase
{
    //Refer to Ben Friedman for QA/Bugfixing on PointsPickup

    //TODO reference to Points system

    [Tooltip("Points gained when picked up")]
    [SerializeField] private int pointAmount = 10;

    /// <summary> Sets PointAmount
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
        pointAmount = value;
    }


    /// <summary> Increments Points in PointSystem/PlayerSystem
    /// 
    /// </summary>
    protected override void ApplyEffect()
    {
        //TODO PointsSystem.Add(pointAmount);

        //UnityEvent.Invoke() + Destroy()
        base.ApplyEffect();
    }
}
