using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AgentParameters : ScriptableObject
{   
    [Header("Movement Params")]
    public float minSpeed = 2f;
    public float maxSpeed = 6f;

    [Header("Direction Influence")]
    public float maxSteeringForce = 3f;

    [Header("Collision Avoidance")]
    public float collisionAvoidanceWeight = 10f;
    public float rangeOfSight = 20f;
    public float sphereCastRadius = 1f;
    public LayerMask obstacleLayer;

    [Header("Rules")]
    public bool enableFlockmateAvoidance = false;
    public float flockmateAvoidanceWeight = 5f;

    public bool enableVelocityMatching = true;
    public float velocityMatchingWeight = 3f;
    
    public bool enableFlockCentering = true;
    public float flockCenteringWeight = 5f;
}
