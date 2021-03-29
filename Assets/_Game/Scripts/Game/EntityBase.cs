using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class EntityBase : MonoBehaviour
{
    [Header("Effects")]
    public UnityEvent Damaged;
    public UnityEvent Died;

    [Header("Settings")]
    [SerializeField] protected float maxHealth = 1;
    protected float _currentHealth = 0;

    protected virtual void Start()
    {
        _currentHealth = maxHealth;
    }
    
    /// <summary> Applies Damage to this entity
    /// 
    /// </summary>
    /// <param name="damage"> Value passed from Projectile/Source. Amount of Damage.</param>
    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        
        if (_currentHealth <= 0)
        {
            Died.Invoke();

            //SetActive False by default. Override to implement other behavior
            gameObject.SetActive(false);
        }
        else
        {
            Damaged.Invoke();
        }
    }
}
