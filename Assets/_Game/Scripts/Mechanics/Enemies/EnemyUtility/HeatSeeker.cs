using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HeatSeeker : MonoBehaviour
{
    //behavior:
    /* Missile + Rammer + Spearhead have this component
        * 
        * Missile sets a rb.velocity = transform.forward 
        * Missile updates it's look direction every Update to follow a GameObject reference
        * Missile updates it rb.velocity to the new Forward
        *   Missile also has "non-reference" option to just fly straight? Should missile always track player?
        *   
        * Rammer uses vector3.moveTowards(playerReference)
        * Rammer sets tranform.lookAt(playerReference)
        * 
        * Update Rammer to use rb?
     */

    [SerializeField] public bool isFollowing { get; private set; } = false;
    [SerializeField] public float RotationSpeed { get; private set; } = 5f;

    private Rigidbody rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update()
    {
        if (isFollowing)
        {
            //target direction is the line from this point to the player's position
            Vector3 dir = GameManager.player.obj.transform.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);

            //goal rotation limited by turn speed limit
            Quaternion newRot = Quaternion.Slerp(transform.rotation, rot, RotationSpeed * Time.deltaTime);

            //changing the physical orientation, but not the physics rotation
            transform.rotation = newRot;
            
            //changing the angle of movement to match the new forward angle, at speed
            rb.velocity = transform.forward * rb.velocity.magnitude;
        }
    }

    public void StartFollowing()
    {
        Debug.Log("Called Stop");
        isFollowing = true;
    }

    public void StopFollowing()
    {
        isFollowing = false;
    }

    public void SetRotationSpeed(float value)
    {
        RotationSpeed = value;
    }
}
