using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointCalculator
{
    public static Vector3[] directions;
    public static int numberOfPoints = 1000;

    static PointCalculator() {
        directions = new Vector3[PointCalculator.numberOfPoints];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float thetaIncrement = 2 * Mathf.PI * goldenRatio;

        for (int i = 0; i < numberOfPoints; ++i) {
            float t = i / (numberOfPoints + 0.5f);
            float phi = Mathf.Acos(1 - 2 * t);
            float theta = thetaIncrement * i;

            float x = Mathf.Cos(theta) * Mathf.Sin(phi);
            float y = Mathf.Sin(theta) * Mathf.Sin(phi);
            float z = Mathf.Cos(phi);

            // Debug.Log(string.Format("distance = {0}, angle = {0}, x = {0}, y = {0}", distance, angle, x, y));

            directions[i] = new Vector3(x, y, z);
        }
    }
}
