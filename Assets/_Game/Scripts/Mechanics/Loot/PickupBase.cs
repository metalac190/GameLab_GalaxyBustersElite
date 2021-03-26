using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PickupBase : MonoBehaviour
{
    //Refer to Ben Friedman for QA/Bugfixing on Loot System scripts

    //hookups for FX teams, and other systems
    public UnityEvent PickedUp;

    [SerializeField] private Vector3 _spinAngles = Vector3.one;
    [SerializeField] private float _spinSpeed = 1f;

    private Collider triggerVolume = null;

    //do all drops give Points? Do weapons give an ammount? 
    //Can we re-use PickupBase as Points-Default?

    private void Awake()
    {
        triggerVolume = GetComponent<Collider>();
        triggerVolume.isTrigger = true;
    }

    private void FixedUpdate()
    {
    	Vector3 spin = _spinAngles.normalized * _spinSpeed;
        transform.Rotate(spin);
    }

    private void OnTriggerEnter(Collider other)
    {
        //LayerMask -> Pickup only collides with Player
        PlayerController playerReference = other.GetComponent<PlayerController>();
        if (playerReference != null)
            ApplyEffect(playerReference);
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
