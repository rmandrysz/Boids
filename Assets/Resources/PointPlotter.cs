using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPlotter : MonoBehaviour
{
    [Range(10, 10000)]
    public int numberOfPoints;

    public float turnFraction = 0.000000f;

    public float pow = 1f;

    public float radius = 5f;

    public GameObject pointPrefab;
    private Transform[] points;

    public bool incrementFraction = true;
    public bool incrementPow = true;
    public bool plotIn3D = true;

    public bool incrementNumberOfPoints = false;


    private void Awake() {
        points = new Transform[10000];
        Vector3 startPosition = new Vector3(-200, -200, -200);

        for (int i = 0; i < 10000; ++i) {
            points[i] = GameObject.Instantiate(pointPrefab, startPosition, Quaternion.identity).transform;
        }
    }

    void FixedUpdate()
    {
        if (plotIn3D) {
            Plot3DPoints();
        }
        else {
            Plot2DPoints();
        }
        if (incrementFraction) {
            IncrementTurnFraction();
        }
        if (incrementPow) {
            IncrementPow();
        }
        if (incrementNumberOfPoints) {
            IncrementNumberOfPoints();
        }
    }

    private void Plot2DPoints() {
        Vector3 position = new Vector3(0, 0, 0);
        for (int i = 0; i < numberOfPoints; ++i) {
            float distance = Mathf.Pow(i / (numberOfPoints + 0.5f), pow);
            float angle = 2 * Mathf.PI * turnFraction * i;

            float x = 5 * distance * Mathf.Cos(angle);
            float y = 5 * distance * Mathf.Sin(angle);

            // Debug.Log(string.Format("distance = {0}, angle = {0}, x = {0}, y = {0}", distance, angle, x, y));

            position.Set(x, y, 0);
            points[i].position = position;
        }
    }

    private void Plot3DPoints() {
        Vector3 position = new Vector3(0, 0, 0);
        for (int i = 0; i < numberOfPoints; ++i) {
            float t = i / (numberOfPoints + 0.5f);
            float phi = Mathf.Acos(1 - 2 * t);
            float theta = 2 * Mathf.PI * turnFraction * i;

            float x = Mathf.Cos(theta) * Mathf.Sin(phi);
            float y = Mathf.Sin(theta) * Mathf.Sin(phi);
            float z = Mathf.Cos(phi);

            // Debug.Log(string.Format("distance = {0}, angle = {0}, x = {0}, y = {0}", distance, angle, x, y));

            position.Set(x, y, z);
            position.Normalize();
            position *= radius;
            points[i].position = position;
        }
    }

    private void IncrementTurnFraction() {
        turnFraction += 0.000005f;
    }

    private void IncrementPow() {
        pow += 0.01f;
    }

    private void IncrementNumberOfPoints() {
        ++numberOfPoints;
    }
}
