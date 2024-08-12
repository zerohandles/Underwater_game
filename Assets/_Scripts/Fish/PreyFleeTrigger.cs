using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyFleeTrigger : MonoBehaviour
{
    PreyStateManager _prey;

    void Start()
    {
        _prey = GetComponentInParent<PreyStateManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _prey.IsFleeing = true;
            _prey.IsWandering = false;
        }
    }
}
