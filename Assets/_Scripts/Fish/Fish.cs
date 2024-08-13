using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class Fish : MonoBehaviour
{
    public NavMeshAgent Agent {get; private set;}
    public PlayerController Player { get; private set;}
    public Vector3 Target {get; private set;}
    public bool IsTargetReached { get; private set; }

    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Collider _fishCollider;
    [SerializeField] float _targetRadius;
    [SerializeField] float _waterHeight = 95;

    float _stuckTimer;
    Vector3 _lastPosition;

    void OnEnable()
    {
        Agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();

        // Set starting nav mesh offset so fish can spawn at any depth and remain on the nav mesh
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, _groundLayer);
        Agent.baseOffset = transform.position.y - hit.point.y;
        Agent.enabled = true;
    }

    void Update()
    {
/*        _stuckTimer += Time.deltaTime;
        if (Vector3.Distance(_lastPosition, transform.position) > 1)
            _stuckTimer = 0;
        if (_stuckTimer > 3)
            SetRandomTarget();*/

        // Check if destination has been reached, ignoring depth.
        IsTargetReached = Vector3.Distance(transform.position, new Vector3(Target.x, transform.position.y, Target.z)) < 5f;

        Agent.destination = new Vector3(Target.x, transform.position.y, Target.z);

        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, _groundLayer);
        if (Mathf.Abs(Target.y - transform.position.y) > 1)
            ChangeDepth(hit);
        else
            MaintainDepth(hit);  
        
        _lastPosition = transform.position;
    }

    /// <summary>
    /// Move the NavMeshAgent's BassOffset towards the target's y position
    /// </summary>
    /// <param name="hit"></param>
    public void ChangeDepth(RaycastHit hit)
    {
        float direction = Mathf.Sign(Target.y - transform.position.y);
        float distanceToSeaFloor = Target.y - hit.point.y;
        float minDepth = _fishCollider.bounds.size.y;

        // Prevent fish from swimming through the ground
        Agent.baseOffset = distanceToSeaFloor > minDepth ? Agent.baseOffset += direction * 0.05f : minDepth;
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

    public void SetRandomTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _targetRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 20, 1))
        {
            Vector3 finalPosition = new Vector3(hit.position.x, Random.Range(Target.y - 10, Target.y +10), hit.position.z);
            finalPosition.y = Mathf.Clamp(finalPosition.y, hit.position.y, _waterHeight);
            Target = finalPosition;
            Debug.Log(gameObject.name + " new random target: " + Target);
        }
        else
        {
            Debug.Log("Failed to find new target.");
            SetRandomTarget();
        }
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
