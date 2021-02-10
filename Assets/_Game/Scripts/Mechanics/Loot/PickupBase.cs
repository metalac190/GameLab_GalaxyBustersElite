using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PickupBase : MonoBehaviour
{
    //Refer to Ben Friedman for QA/Bugfixing on PickupBase

    public UnityEvent PickedUp;

    //ensure that paired Collider is set to Trigger Volume
    private Collider triggerVolume = null;

    private void Awake()
    {
        triggerVolume = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO implement PlayerBase check
        //if other.getcomponent<PlayerBase> != null
        //  //ApplyEffect()
    }

    /// <summary> Virtual/Abstract function
    /// <para> All children need to override to implement </para>
    /// 
    /// </summary>
    /// /// <param name="playerReference"> Reference from OnTriggerEnter to player's Collision/PlayerBase
    protected virtual void ApplyEffect()
    {
        PickedUp.Invoke();
        Destroy(this.gameObject);
    }
}
