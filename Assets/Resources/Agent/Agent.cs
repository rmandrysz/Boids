using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private AgentParameters settings;

    // Movement
    [SerializeField]
    private Rigidbody rb;
    public Vector3 movementDirection;
    private Vector3 velocity;
    private Vector3 acceleration;

    public Vector3 avgFlockDir;
    public Vector3 flockCenter;
    public Vector3 avgAgentAvoidance;
    public int numOfFlockmates;

    // Debugging
    [SerializeField] private bool showDebugLogs = false;

    private void Start()
    {
        movementDirection = transform.forward;
        velocity = movementDirection * (settings.maxSpeed + settings.minSpeed) / 2;
    }

    private void FixedUpdate()
    {
        CalculateAcceleration();
        UpdatePosition();
        RotateInMoveDirection();
        ResetAcceleration();
    }

    private void CalculateAcceleration()
    {
        AvoidCollisions();

        if (numOfFlockmates != 0)
        {
            if (settings.enableFlockmateAvoidance)
            {
                AvoidFlockmates();
            }
            if (settings.enableVelocityMatching)
            {
                MatchVelocity();
            }
            if (settings.enableFlockCentering)
            {
                MoveToFlockCenter();
            }
        }
    }

    private void UpdatePosition()
    {
        velocity += acceleration * Time.fixedDeltaTime;

        float speed = Mathf.Clamp(velocity.magnitude, settings.minSpeed, settings.maxSpeed);
        movementDirection = velocity.normalized;
        velocity = movementDirection * speed;

        rb.MovePosition(rb.position + velocity);
    }

    private void ResetAcceleration()
    {
        acceleration = Vector3.zero;
    }

    private float IsDetectingCollisions()
    {
        RaycastHit hit;
        float sphereCastRadius = settings.sphereCastRadius;
        float rangeOfSight = settings.rangeOfSight;
        if (Physics.SphereCast(transform.position, sphereCastRadius, movementDirection, out hit, rangeOfSight, settings.obstacleLayer))
        {
            Debug.DrawRay(transform.position, movementDirection * rangeOfSight, Color.red);
            if (showDebugLogs)
            {
                print("Found an object - distance: " + hit.distance);
            }
            return hit.distance;
        }
        Debug.DrawRay(transform.position, movementDirection * rangeOfSight, Color.white);
        return -1f;
    }

    private void AvoidCollisions()
    {
        float distance = IsDetectingCollisions();
        if (distance > -1f)
        {
            Vector3 targetDirection = FindNewDirection();
            Vector3 modifiedDirection = MoveToTarget(targetDirection) * settings.collisionAvoidanceWeight;
            if (showDebugLogs)
            {
                Debug.Log(string.Format("Collision modifiedDirection: {0}", modifiedDirection));
            }
            acceleration += modifiedDirection;
        }
    }

    private void AvoidFlockmates()
    {
        Vector3 accelerationUpdate = MoveToTarget(avgAgentAvoidance) * settings.flockmateAvoidanceWeight;
        acceleration += accelerationUpdate;
    }

    private void MatchVelocity()
    {
        Vector3 accelerationUpdate = MoveToTarget(avgFlockDir) * settings.velocityMatchingWeight;
        acceleration += accelerationUpdate;
    }

    private void MoveToFlockCenter()
    {
        Vector3 relativeFlockCenter = flockCenter - transform.position;
        Vector3 accelerationUpdate = MoveToTarget(relativeFlockCenter) * settings.flockCenteringWeight;
        acceleration += accelerationUpdate;
    }

    private Vector3 FindNewDirection()
    {
        Vector3[] directions = PointCalculator.directions;
        Vector3 bestDirection = transform.forward;
        float distanceToObstacle = 0f;
        RaycastHit hit;

        float sphereCastRadius = settings.sphereCastRadius;
        float rangeOfSight = settings.rangeOfSight;

        for (int i = 0; i < directions.Length; ++i)
        {
            Vector3 dir = transform.TransformDirection(directions[i]);
            if (Physics.SphereCast(transform.position, sphereCastRadius, dir, out hit, rangeOfSight, settings.obstacleLayer))
            {
                Debug.DrawRay(transform.position, dir * rangeOfSight, Color.red);
                if (hit.distance > distanceToObstacle)
                {
                    bestDirection = dir;
                    distanceToObstacle = hit.distance;
                }
            }
            else
            {
                Debug.DrawRay(transform.position, dir * rangeOfSight, Color.green);
                return dir;
            }
        }

        return bestDirection;
    }

    private Vector3 MoveToTarget(Vector3 target)
    {
        Vector3 influence = target.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude(influence, settings.maxSteeringForce);
    }

    private void RotateInMoveDirection()
    {
        if (movementDirection != Vector3.zero)
        {
            rb.MoveRotation(Quaternion.LookRotation(movementDirection));
        }
    }

    private void RandomizeDirection()
    {
        float x = Random.Range(-10f, 10f);
        float y = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);

        movementDirection.Set(x, y, z);
        movementDirection.Normalize();
    }

    public void Init(AgentParameters settings)
    {
        this.settings = settings;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void SetData(Vector3 avgFlockVel, Vector3 avgAgentAvoidance, Vector3 flockCenter)
    {
        this.avgFlockDir = avgFlockVel;
        this.avgAgentAvoidance = avgAgentAvoidance;
        this.flockCenter = flockCenter;
    }
}
