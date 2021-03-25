using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BossMissile : EntityBase
{
    private Rigidbody rb = null;

    [SerializeField] private float _lifetime = 5f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _turnSpeed = 0.25f;

    private GameObject playerRef = null;
    private float _time = 0f;
    private int _damage = 1;
    
    private void OnEnable()
    {
        _time = _lifetime;
        rb.velocity = transform.forward * _speed;

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //setactive timer, instead of Destroy(this, delay), usable for Pooling
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    //set values when Instantiated, able to adjust on the fly
    public void SetTarget(Vector3 point)
    {
        //convert to Coroutine (or add to Update loop), to turn over time, instead of snapping
        transform.LookAt(point);
    }

    public void SetTarget(GameObject player)
    {
        Debug.Log("Missile Found Player");
        playerRef = player;

        HeatSeeker seeker = GetComponent<HeatSeeker>();
        seeker?.StartFollowing();
        seeker?.SetRotationSpeed(_turnSpeed);
    }

    public void SetDamage(int value)
    {
        _damage = value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        player?.DamagePlayer(_damage);

        //self destruct
        TakeDamage(999);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player?.DamagePlayer(_damage);

            //self destruct
            TakeDamage(999);
        }
    }
}
