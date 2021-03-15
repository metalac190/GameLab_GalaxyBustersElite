using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    [Header("EntityBase")]
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [SerializeField] float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
