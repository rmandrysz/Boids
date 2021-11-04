using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField]
    private AgentParameters settings;

    // Movement
    [SerializeField]
    private Rigidbody rb;
    private Vector3 movementDirection;
    private Vector3 velocity;
    private Vector3 acceleration;

    // Debugging
    [SerializeField] private bool showDebugLogs = false;
    
    private void Start() {
        movementDirection = transform.forward;
        velocity = movementDirection * (settings.maxSpeed + settings.minSpeed) / 2;
    }

    private void FixedUpdate()
    {
        AvoidCollisions();
        UpdatePosition();
        RotateInMoveDirection();
    }

    private void UpdatePosition() {
        velocity += acceleration * Time.fixedDeltaTime;

        float speed = Mathf.Clamp(velocity.magnitude, settings.minSpeed, settings.maxSpeed);
        movementDirection = velocity.normalized;
        velocity = movementDirection * speed;

        rb.MovePosition(rb.position + velocity);
        acceleration = Vector3.zero;
    }

    private float IsDetectingCollisions() {
        RaycastHit hit;
        float sphereCastRadius = settings.sphereCastRadius;
        float rangeOfSight = settings.rangeOfSight;
        if (Physics.SphereCast(transform.position, sphereCastRadius, movementDirection, out hit, rangeOfSight, settings.obstacleLayer)) {
            Debug.DrawRay(transform.position, movementDirection * rangeOfSight, Color.red);
            if (showDebugLogs) {
                print("Found an object - distance: " + hit.distance);
            }
            return hit.distance;
        }
        Debug.DrawRay(transform.position, movementDirection * rangeOfSight, Color.white);
        return -1f;
    }

    private void AvoidCollisions() {
        float distance = IsDetectingCollisions();
        if (distance > -1f) {
            Vector3 targetDirection = FindNewDirection();
            Vector3 modifiedDirection = MoveToTarget(targetDirection) * settings.collisionAvoidanceWeight;
            if (showDebugLogs) {
                Debug.Log(string.Format("Collision modifiedDirection: {0}", modifiedDirection));
            }
            acceleration += modifiedDirection;        
        }
    }

    private Vector3 FindNewDirection() {
        Vector3[] directions = PointCalculator.directions;
        Vector3 bestDirection = transform.forward;
        float distanceToObstacle = 0f;
        RaycastHit hit;

        float sphereCastRadius = settings.sphereCastRadius;
        float rangeOfSight = settings.rangeOfSight;

        for (int i = 0; i < directions.Length; ++i) {
            Vector3 dir = transform.TransformDirection(directions[i]);
            if (Physics.SphereCast (transform.position, sphereCastRadius, dir, out hit, rangeOfSight, settings.obstacleLayer)) {
                Debug.DrawRay(transform.position, dir * rangeOfSight, Color.red);
                if (hit.distance > distanceToObstacle) {
                    bestDirection = dir;
                    distanceToObstacle = hit.distance;
                }
            }
            else {
                Debug.DrawRay(transform.position, dir * rangeOfSight, Color.green);
                return dir;
            }
        }

        return bestDirection;
    }

    private Vector3 MoveToTarget (Vector3 target) {
        Vector3 influence = target.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude(influence, settings.maxSteeringForce);
    }

    private void RotateInMoveDirection() {
        if (movementDirection != Vector3.zero) {
            rb.MoveRotation(Quaternion.LookRotation(movementDirection));
        }
    }

    private void RandomizeDirection() {
        float x = Random.Range(-10f, 10f);
        float y = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);

        movementDirection.Set(x, y ,z);
        movementDirection.Normalize();
    }    
}
