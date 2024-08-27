using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyStats : MonoBehaviour, IDamageable
{
    public void TakeDamage(float amount)
    {
        Death();
    }

    public void Death()
    {
        // Add loot drops
        Destroy(gameObject);
    }

}
