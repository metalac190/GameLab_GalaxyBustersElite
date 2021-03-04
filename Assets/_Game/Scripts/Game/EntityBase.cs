using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class EntityBase : MonoBehaviour
{
    public UnityEvent Damaged;
    public UnityEvent Died;

    [Header("Settings")]
    [SerializeField] protected int maxHealth = 1;
    protected int _currentHealth = 0;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = maxHealth;
    }
    
    /// <summary> Applies Damage to this entity
    /// 
    /// </summary>
    /// <param name="damage"> Value passed from Projectile/Source. Amount of Damage.</param>
    public virtual void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Died.Invoke();
            //disable or destroy as needed?
        }
        else
        {
            Damaged.Invoke();
            //set up FX + AnimationController from Inspector, using Event
        }
    }
}
