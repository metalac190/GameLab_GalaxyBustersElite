using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LaserDamage : MonoBehaviour
{
    private int _damage = 1;

    public void SetDamage(int value)
    {
        _damage = value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        player?.DamagePlayer(_damage);

        if (player != null)
            gameObject.SetActive(false);
    }
}
