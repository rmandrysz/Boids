using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    public float speed = 10.0f;
    [SerializeField] private Rigidbody rb;
    private Vector3 movementDirection;
    private Vector3 velocity;
    
    private void Start() {
        RandomizeDirection();
    }
    private void FixedUpdate()
    {
        Move();
        RotateInMoveDirection();
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
