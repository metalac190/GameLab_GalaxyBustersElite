using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Collider))]
public class PickupBase : MonoBehaviour
{
    //Refer to Ben Friedman for QA/Bugfixing on Loot System scripts

    //hookups for FX teams, and other systems
    public UnityEvent PickedUp;

    private Collider triggerVolume = null;

    public event Action onDestroy;

    //do all drops give Points? Do weapons give an ammount? 
    //Can we re-use PickupBase as Points-Default?

    private void Awake()
    {
        triggerVolume = GetComponent<Collider>();
        triggerVolume.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //LayerMask -> Pickup only collides with Player
        PlayerController playerReference = other.GetComponent<PlayerController>();
        if (playerReference != null)
            ApplyEffect(playerReference);
    }

	private void OnDestroy()
	{
        onDestroy?.Invoke();
	}

	private void OnEnable()
	{
        ScoreHUD.Instance.PickupEnabled(this);
	}

	/// <summary> Baseline ApplyEffect function
	/// <para> Implements Event.Invoke + Destroy (or disable) this object.</para>
	/// <para> All children need to override to implement </para>
	/// </summary>
	protected virtual void ApplyEffect(PlayerController player)
    {
        PickedUp.Invoke();
        Destroy(this.gameObject);
    }
}
