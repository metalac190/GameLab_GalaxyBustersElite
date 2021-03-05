using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int projDamage { get; private set; } = 0;

    public float bulletSpeed;

    Transform player;
    Vector3 target;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector3(player.position.x, player.position.y, player.position.z);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, bulletSpeed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(int damage)
    {
        projDamage = damage; 
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            col.gameObject.GetComponent<EntityBase>().TakeDamage(projDamage);
            Debug.Log("Player is hit with: " + projDamage);
            /// Eventual hookup to deal damage to player - Would just be TakeDamage with projDamage
        }
    }
}
