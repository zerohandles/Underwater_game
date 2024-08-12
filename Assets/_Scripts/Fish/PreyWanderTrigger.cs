using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyWanderTrigger : MonoBehaviour
{
    PreyStateManager _prey;

    void Start()
    {
        _prey = GetComponentInParent<PreyStateManager>();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _prey.IsFleeing = false;
            _prey.IsWandering = true;
        }
    }
}
