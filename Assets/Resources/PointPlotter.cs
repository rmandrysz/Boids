using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPlotter : MonoBehaviour
{
    [Range(10, 10000)]
    public int numberOfPoints;

    public float turnFraction = 0.000000f;

    public GameObject pointPrefab;
    private Transform[] points;

    public bool incrementFraction = true;


    private void Awake() {
        points = new Transform[numberOfPoints];

        for (int i = 0; i < numberOfPoints; ++i) {
            points[i] = GameObject.Instantiate(pointPrefab, Vector3.zero, Quaternion.identity).transform;
        }
    }

    void FixedUpdate()
    {
        PlotPoints();
        if (incrementFraction) {
            IncrementTurnFraction();
        }
    }

    private void PlotPoints() {
        Vector3 position = new Vector3(0, 0, 0);
        for (int i = 0; i < numberOfPoints; ++i) {
            float distance = i / (numberOfPoints - 0.001f);
            float angle = 2 * Mathf.PI * turnFraction * i;

            float x = 5 * distance * Mathf.Cos(angle);
            float y = 5 * distance * Mathf.Sin(angle);

            position.Set(x, y, 0);
            points[i].position = position;
        }
    }

    private void IncrementTurnFraction() {
        turnFraction += 0.000001f;
    }
}
