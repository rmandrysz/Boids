using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundControl : MonoBehaviour
{
    public float playgroundSize = 50f;

    [SerializeField] private List<string> planesKeys;
    [SerializeField] private List<Transform> planesValues;

    private void Start() {
        initSize();
    }
    private void OnCollisionEnter(Collision other) {
        Debug.Log(other.contacts[0].point);
        loopPosition(other);
    }

    private void loopPosition(Collision agent)
    {
        Vector3 contactPoint = agent.contacts[0].point;
        float xDist = Mathf.Abs(contactPoint.x);
        float yDist = Mathf.Abs(contactPoint.y);
        float zDist = Mathf.Abs(contactPoint.z);
        float playgroundBorder = playgroundSize / 2f;
        var agentTransform = agent.transform;
        var agentPosition = agentTransform.position;

        if (xDist > yDist && xDist > zDist) {
            agentTransform.position = new Vector3(-agentPosition.x, agentPosition.y, agentPosition.z);
        }
        else if (yDist > xDist && yDist > zDist) {
            agentTransform.position = new Vector3(agentPosition.x, -agentPosition.y, agentPosition.z);
        }
        else if (zDist > xDist && zDist > yDist) {
            agentTransform.position = new Vector3(agentPosition.x, agentPosition.y, -agentPosition.z);
        }
    }

    private void initSize() {
        Vector3 size = new Vector3(playgroundSize, playgroundSize, playgroundSize);

        foreach (var plane in planesValues) {
            plane.localScale = size;
        }

        planesValues[planesKeys.IndexOf("Front")].transform.position = new Vector3(0, 0, playgroundSize);
        planesValues[planesKeys.IndexOf("Right")].transform.position = new Vector3(playgroundSize, 0, 0);
        planesValues[planesKeys.IndexOf("Left")].transform.position = new Vector3(-playgroundSize, 0, 0);
        planesValues[planesKeys.IndexOf("Back")].transform.position = new Vector3(0, 0, -playgroundSize);
        planesValues[planesKeys.IndexOf("Top")].transform.position = new Vector3(0, playgroundSize, 0);
        planesValues[planesKeys.IndexOf("Bottom")].transform.position = new Vector3(0, -playgroundSize, 0);
    }
}
