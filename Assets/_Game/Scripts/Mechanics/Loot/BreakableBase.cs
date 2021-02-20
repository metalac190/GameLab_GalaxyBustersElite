using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BreakableBase : MonoBehaviour
{
    //Refer to Ben Friedman for QA/Bugfixing on BreakableBase

    //TODO inherit from iShootable, or other Hit-Detection scripts
    //confer with EnemyProgrammer (Brett) to determine how Health is being handled

    public UnityEvent OnBreak;
    public UnityEvent OnHit;

    private Collider triggerVolume = null;
    private Rigidbody rb = null;

    //re-define useing EntityBase.Health
    [SerializeField] private int hits = 1;    //number of hits needed to destroy this object
    private int curHits = 0;


    private void Awake()
    {
        triggerVolume = GetComponent<Collider>();
        triggerVolume.isTrigger = true;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter BreakableBase");
        
        //use Layers system to make sure Breakables only interact with Projectiles
        //might need to use tags for PlayerProjectiles, EnemyProjectiles?
        MarkHit();
        
    }

    /// <summary> Called when hit, refer to Hit-Detection scripts
    /// <para>
    ///     Implement as a Listener to Hit-Detection script, call when object is Hit
    ///     Increments curHits to mark amount of hits taken in lifetime
    ///     Calls Break() when passing Hits threshold
    /// </para>
    /// </summary>
    public void MarkHit()
    {
        OnHit.Invoke();

        curHits++;

        if (curHits >= hits)
        {
            Break();
        }
    }

    /// <summary>default Break calls UnityEvent and Destroy()
    /// <para>
    ///     LootContainer and other children should override Break() to implement their own special function calls
    /// </para>
    /// <seealso cref="LootContainer.cs"/>
    /// </summary>
    public virtual void Break()
    {
        OnBreak.Invoke();
        Destroy(this.gameObject);
    }

}
