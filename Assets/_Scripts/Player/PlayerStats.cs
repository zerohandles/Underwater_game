using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] float _health = 5;
    [SerializeField] float _damage = 1;
    [SerializeField] float _money = 0;
    [SerializeField] int _shells = 0;

    public float Health
    {
        get { return _health; }
        private set { _health = value; }
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;

        if (Health <= 0)
            Death();
    }

    public void Death()
    {
        Debug.Log("Player died. Game over.");
    }
}
