using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LaserDamage : MonoBehaviour
{
    private int _damage = 1;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void SetDamage(int value)
    {
        _damage = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        player?.DamagePlayer(_damage);
    }
}
