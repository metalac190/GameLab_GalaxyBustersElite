using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private int projDamage = 0;
    public int ProjDamage { get { return projDamage; } }

    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifetime;

    private GameObject player = null;
    Vector3 target;

    private void Awake()
    {
        Destroy(gameObject, bulletLifetime);
    }

    void Start()
    {
        player = GameManager.player.obj;

        target = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, bulletSpeed * Time.deltaTime);
    }

    public void SetDamage(int damage)
    {
        projDamage = damage; 
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //col.gameObject.GetComponent<EntityBase>().TakeDamage(ProjDamage);
            col.gameObject.GetComponent<PlayerController>().DamagePlayer(ProjDamage);
            Debug.Log("Player is hit with: " + ProjDamage);

            Destroy(gameObject);
        }
    }
}
