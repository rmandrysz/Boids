using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    [SerializeField] private Agent prefab;
    public float spawnRadius = 30f;
    public int numOfAgents = 10;
    private Agent[] agents;

    private void Awake() {
        agents = new Agent[numOfAgents];
        Spawn();
    }

    private void Spawn() {
        for (int i = 0; i < numOfAgents; ++i) {
            Vector3 position = transform.position + (Random.insideUnitSphere * spawnRadius);
            Agent agent = Instantiate(prefab); 
            agent.transform.position = position;
            agent.transform.forward = Random.insideUnitSphere;

            agents[i] = agent;
        }
    }
}
