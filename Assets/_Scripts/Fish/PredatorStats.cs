using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorStats : MonoBehaviour, IDamageable
{
    [SerializeField] float _health = 1f;

    public float Health 
    { 
        get { return _health; } 
        private set { _health -= value; }
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;

        if (Health <= 0)
            Death();
    }

    public void Death()
    {
        // Add loot drops
        Destroy(gameObject);
    }
}
