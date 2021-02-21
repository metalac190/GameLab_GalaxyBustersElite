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

    //do all drops give Points? Do weapons give an ammount? 
    //Can we re-use PickupBase as Points-Default?
    //private GameManger manager = GameManger.STATICMANAGER;

    private void Awake()
    {
        triggerVolume = GetComponent<Collider>();
        //PickedUp.AddListener(manager);    //?
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO implement PlayerBase check
        //LayerMask -> Pickup only collides with Player
        Debug.Log("OnTriggerEnter PickupBase");

        ApplyEffect();
    }

    /// <summary> Virtual/Abstract function
    /// <para> All children need to override to implement </para>
    /// 
    /// </summary>
    protected virtual void ApplyEffect()
    {
        PickedUp.Invoke();
        Destroy(this.gameObject);
    }
}
