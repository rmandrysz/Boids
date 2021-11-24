using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    [SerializeField] private Agent prefab;
    [SerializeField] public AgentParameters settings;
    public float spawnRadius = 30f;
    public int numOfAgents = 10;
    private Agent[] agents;
    private AgentBufferData[] bufferData;
    public ComputeShader computeShader;

    const int threadGroupSize = 1024;

    private void Awake()
    {
        agents = new Agent[numOfAgents];
        bufferData = new AgentBufferData[numOfAgents];
        Spawn();
    }

    private void FixedUpdate()
    {
        UpdateAgentData();
    }

    private void Spawn()
    {
        for (int i = 0; i < numOfAgents; ++i)
        {
            Vector3 position = transform.position + (Random.insideUnitSphere * spawnRadius);
            Agent agent = Instantiate(prefab);
            agent.transform.position = position;
            agent.transform.forward = Random.insideUnitSphere;
            agent.Init(settings);

            agents[i] = agent;
        }
    }

    private void UpdateAgentData()
    {
        for (int i = 0; i < numOfAgents; ++i)
        {
            bufferData[i].pos = agents[i].transform.position;
            bufferData[i].dir = agents[i].movementDirection;
        }

        ComputeBuffer buffer = new ComputeBuffer(numOfAgents, AgentBufferData.Size);
        buffer.SetData(bufferData);

        computeShader.SetBuffer(0, "agents", buffer);
        computeShader.SetInt("numAgents", numOfAgents);
        computeShader.SetFloat("rangeOfSight", settings.rangeOfSight);

        int numOfThreadGroups = Mathf.CeilToInt(numOfAgents / (float)threadGroupSize);
        computeShader.Dispatch(0, numOfThreadGroups, 1, 1);

        buffer.GetData(bufferData);

        for (int i = 0; i < numOfAgents; ++i)
        {
            if (bufferData[i].numFlockmates != 0)
            {
                agents[i].avgFlockDir = bufferData[i].flockmateDirSum / bufferData[i].numFlockmates;
                agents[i].flockCenter = bufferData[i].flockmatePosSum / bufferData[i].numFlockmates;
                agents[i].avgAgentAvoidance = bufferData[i].avgAgentAvoidance / bufferData[i].numFlockmates;
                agents[i].numOfFlockmates = bufferData[i].numFlockmates;
            }
            else
            {
                agents[i].avgFlockDir = Vector3.zero;
                agents[i].flockCenter = Vector3.zero;
                agents[i].avgAgentAvoidance = Vector3.zero;
                agents[i].numOfFlockmates = 0;
            }
        }

        buffer.Release();
    }

    public struct AgentBufferData
    {
        public Vector3 pos;
        public Vector3 dir;

        public Vector3 flockmateDirSum;
        public Vector3 flockmatePosSum;
        public Vector3 avgAgentAvoidance;
        public int numFlockmates;

        public static int Size
        {
            get
            {
                return sizeof(float) * 3 * 5 + sizeof(int);
            }
        }
    }
}
