using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BreakableBase : EntityBase
{
    //Refer to Ben Friedman for QA/Bugfixing on BreakableBase

    //TODO inherit from iShootable, or other Hit-Detection scripts
    //confer with EnemyProgrammer (Brett) to determine how Health is being handled

    private Collider triggerVolume = null;
    private Rigidbody rb = null;
    
    private void Awake()
    {
        triggerVolume = GetComponent<Collider>();
        triggerVolume.isTrigger = true;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
}
