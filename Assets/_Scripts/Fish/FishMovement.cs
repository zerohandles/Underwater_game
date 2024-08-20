using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed of the fish
    public float rotationSpeed = 3f; // Speed at which the fish rotates
    public float raycastDistance = 2f; // Distance for raycast to detect obstacles
    public LayerMask terrainLayer; // Layer that represents the terrain
    public float minY = 0f; // Minimum y position (ground level)
    public float maxY = 10f; // Maximum y position (water surface level)
    public float distanceFromFloor = 2f; // Desired distance from the ocean floor
    public int rayCount = 5; // Number of raycasts in the forward spread
    public float spreadAngle = 45f; // Spread angle for the raycasts in front

    private Vector3 targetDirection; // Direction the fish is moving

    void Start()
    {
        // Initialize the target direction to a random direction
        targetDirection = GetRandomDirection();
        RotateFish();
    }

    void Update()
    {
        MoveFish();
        CheckForObstacles();
    }

    void MoveFish()
    {
        // Move the fish forward along its local x-axis
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Constrain the y position within the specified bounds and maintain distance from the floor
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, raycastDistance + distanceFromFloor, terrainLayer))
        {
            // Maintain a set distance from the ocean floor
            float desiredY = hit.point.y + distanceFromFloor;
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Clamp(desiredY, minY, maxY),
                transform.position.z
            );
        }
        else
        {
            // Just clamp y position if no hit was detected
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Clamp(transform.position.y, minY, maxY),
                transform.position.z
            );
        }

        // Ensure the fish can swim upwards if it is too low
        if (transform.position.y <= minY + distanceFromFloor)
        {
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Clamp(transform.position.y + 1f, minY, maxY),
                transform.position.z
            );
        }
    }

    void CheckForObstacles()
    {
        RaycastHit hit;
        bool obstacleDetected = false;
        Vector3 avoidanceDirection = Vector3.zero;

        // Raycast directly in front of the fish
        if (Physics.Raycast(transform.position, transform.right, out hit, raycastDistance, terrainLayer))
        {
            obstacleDetected = true;
            avoidanceDirection += hit.normal; // Accumulate the normal of the hit surface
        }

        // Additional raycasts in a spread around the front of the fish
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate the angle for each ray in the spread
            float angle = (i - (rayCount - 1) / 2f) * (spreadAngle / (rayCount - 1));
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.right;

            if (Physics.Raycast(transform.position, rayDirection, out hit, raycastDistance, terrainLayer))
            {
                obstacleDetected = true;
                avoidanceDirection += hit.normal; // Accumulate the normal of the hit surface
            }
        }

        if (obstacleDetected)
        {
            AvoidObstacle(avoidanceDirection);
        }
    }

    void AvoidObstacle(Vector3 avoidanceDirection)
    {
        // Calculate a new direction away from the obstacle
        targetDirection = Vector3.Reflect(transform.right, avoidanceDirection.normalized);
        RotateFish();
    }

    void RotateFish()
    {
        // Smoothly rotate the fish to face the new direction
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    Vector3 GetRandomDirection()
    {
        // Generate a random direction within the 3D space, including y-axis movement
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
