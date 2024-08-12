using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorChaseTrigger : MonoBehaviour
{
    PredatorStateMachine _predator;

    void Start()
    {
        _predator = GetComponentInParent<PredatorStateMachine>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
            _predator.IsChasing = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
            _predator.IsChasing = false;
    }
}
