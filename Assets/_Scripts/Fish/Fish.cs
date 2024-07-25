using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Fish : MonoBehaviour
{
    NavMeshAgent _agent;
    [SerializeField] Transform _target;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Collider _fishCollider;


    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _agent.destination = new Vector3(_target.position.x, transform.position.y, _target.position.z);

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, _groundLayer);
        if (Mathf.Abs(_target.position.y - transform.position.y) > 1)
            SetDepth(hit);
        else
            MaintainDepth(hit);
    }

    /// <summary>
    /// Move the NavMeshAgent's BassOffset towards the target's y position
    /// </summary>
    /// <param name="hit"></param>
    private void SetDepth(RaycastHit hit)
    {
        float direction = Mathf.Sign(_target.position.y - transform.position.y);
        float distanceToSeaFloor = _target.transform.position.y - hit.point.y;
        float minDepth = _fishCollider.bounds.size.y;

        // Prevent fish from swimming through the ground
        _agent.baseOffset = distanceToSeaFloor > minDepth ? _agent.baseOffset += (direction * 0.05f) : minDepth;
    }

    /// <summary>
    /// Keeps the NavMeshAgent at a consistant height on unlevel terrian.
    /// </summary>
    /// <param name="hit"></param>
    private void MaintainDepth(RaycastHit hit)
    {
        float desiredDepth = _target.transform.position.y - hit.point.y;
        float minDepth = _fishCollider.bounds.size.y;

        // Prevent fish from bobbing up and down on unlevel terrain.
        _agent.baseOffset = desiredDepth > minDepth ? desiredDepth : minDepth;
    }


    private void OnDrawGizmos()
    {
        // Set the origin to the underside of the fish
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - _fishCollider.bounds.size.y / 2, transform.position.z);

        Gizmos.color = Color.red;
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity))
        {
            // Adjust the endpoint based on the hit point
            Vector3 endpoint = hit.point;
            Gizmos.DrawLine(origin, endpoint);
        }
    }
}
