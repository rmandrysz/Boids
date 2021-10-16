using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    public float speed = 10.0f;
    [SerializeField] Rigidbody rb;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(this.transform.position += new Vector3(1f, 5f, 3f) * speed * Time.fixedDeltaTime);
    }
}
