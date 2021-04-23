using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HeatSeeker : MonoBehaviour
{
    private Rigidbody rb = null;

    [Header("Movement Settings")]    
    [SerializeField] private float _rotationSpeed = 5f;

    [Header("Time-Out Settings")]
    [SerializeField] private float _dieTime = 2f;
    private float _currTime = 0;
    private bool _timeOut = false;

    private bool _isFollowing = false;
    public bool isFollowing { get { return _isFollowing; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void OnEnable()
    {
        _currTime = 0;
        _timeOut = false;
    }

    private void Update()
    {
        if (_isFollowing)
        {
            //target direction is the line from this point to the player's position
            Vector3 dir = GameManager.player.obj.transform.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);

            //goal rotation limited by turn speed limit
            Quaternion newRot = Quaternion.Slerp(transform.rotation, rot, _rotationSpeed * Time.deltaTime);

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
        _isFollowing = true;
    }

    public void StopFollowing()
    {
        _isFollowing = false;
        _timeOut = true;
    }

    public void SetRotationSpeed(float value)
    {
        _rotationSpeed = value;
    }
}
