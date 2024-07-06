using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[ExecuteInEditMode]
public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public Transform pointD;
 
    // Update is called once per frame
    void Update()
    {
        List<Vector3> path = BezierUtility.BeizerIntepolate4List(pointA.position, pointB.position, pointC.position, pointD.position, 40);
 
        lineRenderer.positionCount = path.Count;
        lineRenderer.SetPositions(path.ToArray());
    }
}