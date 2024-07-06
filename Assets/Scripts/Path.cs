using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Vector2> GetPathPoints()
    {
        List<Vector2> pathPoints = new List<Vector2>();

        foreach (Transform segment in transform)
        {
            LineRenderer lineRenderer = segment.GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                Vector2[] positions = new Vector2[lineRenderer.positionCount];
                for (int i = 0; i < lineRenderer.positionCount; i++)
                {
                    positions[i] = lineRenderer.GetPosition(i);
                }
                pathPoints.AddRange(positions);
            }
        }

        return pathPoints;
    }
}
