using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PointsPickup : PickupBase
{
    //Refer to Ben Friedman for QA/Bugfixing on Loot System scripts

    [Tooltip("Points gained when picked up")]
    [SerializeField] private int pointAmount = 10;

    /// <summary> Initializes PointAmount
    /// <para> Only use when instantiating a prefab that contians this script. </para>
    /// <para> Do NOT update dynamically </para>
    /// </summary>
    /// <param name="value"> New pointAmount value </param>
    public void SetPointAmount(int value)
    {
        //should be affected during Construction
        //research: instantiate(new object, with component<Points>(pointsAmount)) style construction?
        pointAmount = value;
    }

    /// <summary> Increments Points in PointSystem/PlayerSystem
    /// <para> Works as a replacement to OnTriggerEnter/ApplyEffect </para>
    /// </summary>
    public void GivePoints()
    {
        ScoreSystem.IncreaseScore(pointAmount);
        PickedUp.Invoke();

        //Destroy self      //is FX child entities (must time out destroy) or non-child entities (destroy after calling)?
        Destroy(this.gameObject);
    }
}
