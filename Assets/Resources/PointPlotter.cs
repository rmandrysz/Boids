using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPlotter : MonoBehaviour
{
    [Range(10, 10000)]
    public int numberOfPoints;

    public float turnFraction = 0.000000f;

    public float pow = 1f;

    public GameObject pointPrefab;
    private Transform[] points;

    public bool incrementFraction = true;
    public bool incrementPow = true;


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
        if (incrementPow) {
            IncrementPow();
        }
    }

    private void PlotPoints() {
        Vector3 position = new Vector3(0, 0, 0);
        for (int i = 0; i < numberOfPoints; ++i) {
            float distance = Mathf.Pow(i / (numberOfPoints + 1f), pow);
            float angle = 2 * Mathf.PI * turnFraction * i;

            float x = 5 * distance * Mathf.Cos(angle);
            float y = 5 * distance * Mathf.Sin(angle);

            // Debug.Log(string.Format("distance = {0}, angle = {0}, x = {0}, y = {0}", distance, angle, x, y));

            position.Set(x, y, 0);
            points[i].position = position;
        }
    }

    private void IncrementTurnFraction() {
        turnFraction += 0.000005f;
    }

    private void IncrementPow() {
        pow += 0.01f;
    }
}
