using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class UtilityEvent : MonoBehaviour
{
    public UnityEvent Triggered;
    private Collider _myCollider = null;

    private void Awake()
    {
        _myCollider = GetComponent<Collider>();
        _myCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //validate, other is Player, probably a better method somewhere
        if (other.CompareTag("Player"))
            Triggered.Invoke();
    }
}
