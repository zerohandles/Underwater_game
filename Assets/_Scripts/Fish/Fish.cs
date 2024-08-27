using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Fish : MonoBehaviour
{
    public NavMeshAgent Agent { get; private set; }
    public PlayerController Player { get; private set; }
    public Vector3 Target { get; private set; }
    public bool IsTargetReached { get; private set; }

    [Header("AI Navigation")]
    [SerializeField] Collider _fishCollider;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] LayerMask _NavMeshLayer;
    [SerializeField] string _navMeshName = "NavMesh";
    [SerializeField] float _navMeshOffset = 10;
    [SerializeField] float _newTargetRadius;
    [SerializeField] float _waterHeight = 95;
    NavMeshSurface _surface;
    NavMeshData _data;
    float _stuckDelay = 3;
    float _stuckTimer;
    bool _isStuck;
    Vector3 _lastPosition;
    Vector3 _currentPosition;

    [Header("Stats")]
    [SerializeField] float _wanderSpeed;
    [SerializeField] float _fleeSpeed;
    [SerializeField] float _chaseSpeed;


    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        _surface = GameObject.Find(_navMeshName).GetComponent<NavMeshSurface>();
        _data = _surface.navMeshData;
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        _lastPosition = transform.position;
        Target = transform.position;

        // Set starting nav mesh offset so fish can spawn at any depth and remain on the nav mesh
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, _NavMeshLayer);
        Agent.baseOffset = transform.position.y - hit.point.y;
        Agent.enabled = true;
    }

    private void OnDisable()
    {
        Agent.enabled = false;
    }

    void Update()
    {
        _currentPosition = transform.position;
        _stuckTimer += Time.deltaTime;
        if (_stuckTimer > 1 )
        {
            _stuckTimer = 0;

            if (!_isStuck && Vector3.Distance(_lastPosition, _currentPosition) < 1f)
            {
                // Debug.Log(name + " Is Stuck");
                StartCoroutine(StuckTimer(_currentPosition));
            }

            _lastPosition = _currentPosition;
        }

        // Debug.Log($"Distance to Target: {Vector3.Distance(transform.position, Target)}");

        // Check if destination has been reached, ignoring depth.
        IsTargetReached = Vector3.Distance(transform.position, new Vector3(Target.x, transform.position.y, Target.z)) < 5f;

        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, _groundLayer);
        if (Mathf.Abs(Target.y - transform.position.y) > 1)
            ChangeDepth(hit);
        else
            MaintainDepth(hit);  
    }

    // Compare the starting position with current position after a delay to check if fish is not moving
    private IEnumerator StuckTimer(Vector3 stuckPos)
    {
        Debug.Log(name + " is starting stuck routine " + Time.time);
        if (_isStuck)
            yield return null;

        _isStuck = true;
        yield return new WaitForSeconds(_stuckDelay);
        if (Vector3.Distance(transform.position, stuckPos) < 1)
            SetRandomTarget();
        _isStuck = false;
    }

    /// <summary>
    /// Move the NavMeshAgent's BassOffset towards the target's y position
    /// </summary>
    /// <param name="hit"></param>
    public void ChangeDepth(RaycastHit hit)
    {
        float direction = Mathf.Sign(Target.y - transform.position.y);
        float distanceToSeaFloor = Target.y - hit.point.y;
        float minDepth = _fishCollider.bounds.size.y + hit.point.y + _navMeshOffset;

        // Prevent fish from swimming through the ground
        Agent.baseOffset = distanceToSeaFloor > minDepth ? Agent.baseOffset += direction * 0.1f : minDepth;
    }

    /// <summary>
    /// Keeps the NavMeshAgent at a consistant height on unlevel terrian.
    /// </summary>
    /// <param name="hit"></param>
    public void MaintainDepth(RaycastHit hit)
    {
        float desiredDepth = Target.y + _navMeshOffset;
        float minDepth = (_fishCollider.bounds.size.y * 1.5f) + hit.point.y + _navMeshOffset;

        // Prevent fish from bobbing up and down on unlevel terrain.
        Agent.baseOffset = desiredDepth > minDepth ? desiredDepth : minDepth;
    }

    public void SetRandomTarget()
    {
/*        Vector3 randomDirection = Random.insideUnitSphere * _newTargetRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 20, 1))
        {
            Vector3 finalPosition = new Vector3(hit.position.x, Random.Range(Target.y - 10, Target.y + 10), hit.position.z);
            finalPosition.y = Mathf.Clamp(finalPosition.y, hit.position.y, _waterHeight);
            Target = finalPosition;
            // Debug.Log(gameObject.name + " new random target: " + Target);
            Agent.destination = new Vector3(Target.x, transform.position.y, Target.z);
        }
        else
        {
            // Debug.Log("Failed to find new target.");
            SetRandomTarget();
        }*/
        var bounds = _data.sourceBounds;
        var x = Random.Range(bounds.min.x, bounds.max.x);
        var y = Target.y;
        var z = Random.Range(bounds.min.z, bounds.max.z);

        Vector3 newDestination = new Vector3(x, Random.Range(y - 10, y + 10), z);
        newDestination.y = Mathf.Clamp(newDestination.y, 5, _waterHeight);
        Target = newDestination;
        Agent.destination = new Vector3(Target.x, -10, Target.z);
        Debug.Log("New destination: " + Target);
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

    public void SetNewTarget(Vector3 newTarget)
    {
        Target = newTarget;
        Agent.destination = newTarget;
    }
}
