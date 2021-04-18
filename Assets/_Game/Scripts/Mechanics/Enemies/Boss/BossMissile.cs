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
    
    private float _time = 0f;
    private int _damage = 1;
    private HeatSeeker _seeker = null;
    private HeatSeeker seeker
    {
        get
        {
            if (_seeker == null)
                _seeker = GetComponent<HeatSeeker>();

            return _seeker;
        }
    }


    protected virtual void Awake()
    {
        // Move projectile forwards with set speed
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * _speed;
    }

    protected virtual void OnEnable()
    {
        _time = 0;
        rb.velocity = transform.forward * _speed;
    }

    private void Update()
    {
        // Destroy projectile after lifetime expires
        _time += Time.deltaTime;
        if (_time > _lifetime)
            gameObject.SetActive(false);
    }

    //set values when Instantiated, able to adjust on the fly
    public void SetTarget(Vector3 point)
    {
        //using RB move forward, moves automatically
        transform.LookAt(point);
    }

    public void SetTarget(GameObject player)
    {
        Debug.Log("Missile Found Player");

        seeker?.StartFollowing();
        seeker?.SetRotationSpeed(_turnSpeed);
    }

    public void SetDamage(int value)
    {
        _damage = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.DamagePlayer(_damage);
            gameObject.SetActive(false);
        }
    }
}
