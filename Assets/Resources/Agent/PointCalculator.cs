using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointCalculator
{
    public static Vector3[] viewDirections;
    public static float awareness = 360f;

    static PointCalculator() {
        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float thetaIncrement = 2 * Mathf.PI * goldenRatio;

        for (int i = 0; i < awareness; ++i) {
            float t = i / (awareness + 0.5f);
            float phi = Mathf.Acos(1 - 2 * t);
            float theta = thetaIncrement * i;

            float x = Mathf.Cos(theta) * Mathf.Sin(phi);
            float y = Mathf.Sin(theta) * Mathf.Sin(phi);
            float z = Mathf.Cos(phi);

            // Debug.Log(string.Format("distance = {0}, angle = {0}, x = {0}, y = {0}", distance, angle, x, y));

            viewDirections[i] = new Vector3(x, y, z);
        }
    }
}
