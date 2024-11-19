using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSpline : MonoBehaviour
{
    public Transform[] controlPoints;
    public int splineDegree = 2; // Puede ser 2, 3, o cualquier grado deseado
    public int numberOfPointsOnCurve = 100;

    private Vector3[] curvePoints;

    void Start()
    {
        ComputeBSpline();
    }

    void ComputeBSpline()
    {
        curvePoints = new Vector3[numberOfPointsOnCurve];
        float increment = 1f / (numberOfPointsOnCurve - 1);

        for (int i = 0; i < numberOfPointsOnCurve; i++)
        {
            float t = i * increment;
            curvePoints[i] = CalculateBSplinePoint(t);
        }
    }

    Vector3 CalculateBSplinePoint(float t)
    {
        int n = controlPoints.Length - 1;

        Vector3 point = Vector3.zero;

        for (int i = 0; i <= n; i++)
        {
            float blend = CalculateBSplineBlend(i, splineDegree, t);
            point += controlPoints[i].position * blend;
        }

        return point;
    }

    float CalculateBSplineBlend(int i, int k, float t)
    {
        if (k == 1)
        {
            if (t >= i / (float)(controlPoints.Length - 1) && t < (i + 1) / (float)(controlPoints.Length - 1))
                return 1f;
            else
                return 0f;
        }

        float term1 = (t - i / (float)(controlPoints.Length - 1)) / ((i + k - 1) / (float)(controlPoints.Length - 1) - i / (float)(controlPoints.Length - 1)) * CalculateBSplineBlend(i, k - 1, t);
        float term2 = ((i + k) / (float)(controlPoints.Length - 1) - t) / ((i + k) / (float)(controlPoints.Length - 1) - (i + 1) / (float)(controlPoints.Length - 1)) * CalculateBSplineBlend(i + 1, k - 1, t);

        return term1 + term2;
    }

    void OnDrawGizmos()
    {
        if (curvePoints == null)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < curvePoints.Length - 1; i++)
        {
            Gizmos.DrawLine(curvePoints[i], curvePoints[i + 1]);
        }

        Gizmos.color = Color.blue;

        foreach (var point in controlPoints)
        {
            Gizmos.DrawSphere(point.position, 0.1f);
        }
    }
}
