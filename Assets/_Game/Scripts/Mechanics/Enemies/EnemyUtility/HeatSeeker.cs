using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HeatSeeker : MonoBehaviour
{
    [SerializeField] public bool isFollowing { get; private set; } = false;
    [SerializeField] public float RotationSpeed { get; private set; } = 5f;

    private Rigidbody rb = null;

    private bool _timeOut = false;
    private float _currTime = 0;
    private float _dieTime = 2f;

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

        if (_timeOut)
        {
            _currTime+= Time.deltaTime;
            if (_currTime > _dieTime)
                gameObject.SetActive(false);
        }
    }

    public void StartFollowing()
    {
        isFollowing = true;
    }

    public void StopFollowing()
    {
        isFollowing = false;
        _timeOut = true;
    }

    public void SetRotationSpeed(float value)
    {
        RotationSpeed = value;
    }
}
