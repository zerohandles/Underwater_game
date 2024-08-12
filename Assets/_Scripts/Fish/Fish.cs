using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Fish : MonoBehaviour
{
    public NavMeshAgent Agent {get; private set;}
    public PlayerController Player { get; private set;}
    public Vector3 Target {get; private set;}

    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Collider _fishCollider;


    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        Agent.destination = new Vector3(Target.x, transform.position.y, Target.z);

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, _groundLayer);
        if (Mathf.Abs(Target.y - transform.position.y) > 1)
            SetDepth(hit);
        else
            MaintainDepth(hit);
    }

    /// <summary>
    /// Move the NavMeshAgent's BassOffset towards the target's y position
    /// </summary>
    /// <param name="hit"></param>
    public void SetDepth(RaycastHit hit)
    {
        float direction = Mathf.Sign(Target.y - transform.position.y);
        float distanceToSeaFloor = Target.y - hit.point.y;
        float minDepth = _fishCollider.bounds.size.y;

        // Prevent fish from swimming through the ground
        Agent.baseOffset = distanceToSeaFloor > minDepth ? Agent.baseOffset += (direction * 0.05f) : minDepth;
    }

    /// <summary>
    /// Keeps the NavMeshAgent at a consistant height on unlevel terrian.
    /// </summary>
    /// <param name="hit"></param>
    public void MaintainDepth(RaycastHit hit)
    {
        float desiredDepth = Target.y - hit.point.y;
        float minDepth = _fishCollider.bounds.size.y;

        // Prevent fish from bobbing up and down on unlevel terrain.
        Agent.baseOffset = desiredDepth > minDepth ? desiredDepth : minDepth;
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

    public void SetNewTarget(Vector3 newTarget) => Target = newTarget;
}
