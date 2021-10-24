using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float speed = 10.0f;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float rangeOfSight = 10f;
    private Vector3 movementDirection;
    private Vector3 velocity;

    [SerializeField] private float sphereCastRadius = 1f;
    
    private void Start() {
        movementDirection = transform.forward;
    }

    private void FixedUpdate()
    {
        AvoidCollisions();
        Move();
        RotateInMoveDirection();
    }

    private bool IsDetectingCollisions() {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, sphereCastRadius, movementDirection, out hit, rangeOfSight)) {
            Debug.DrawRay(transform.position, movementDirection * rangeOfSight, Color.red);
            // print("Found an object - distance: " + hit.distance);
            return true;
        }
        Debug.DrawRay(transform.position, movementDirection * rangeOfSight, Color.white);
        return false;
    }

    private void AvoidCollisions() {
        if (IsDetectingCollisions()) {
            Vector3 newDirection = FindNewDirection();
            movementDirection = newDirection;
        }
    }

    private Vector3 FindNewDirection() {
        Vector3[] directions = PointCalculator.directions;
        Vector3 bestDirection = transform.forward;
        float distanceToObstacle = 0f;
        RaycastHit hit;

        for (int i = 0; i < directions.Length; ++i) {
            Vector3 dir = transform.TransformDirection(directions[i]);
            if (Physics.SphereCast (transform.position, sphereCastRadius, dir, out hit, rangeOfSight)) {
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

    private void Move() {
        velocity = movementDirection * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position += velocity);
    }

    private void RotateInMoveDirection() {
        if (movementDirection != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(movementDirection);
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
