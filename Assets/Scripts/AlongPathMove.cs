using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlongPathMove : MonoBehaviour
{
    public float speed = 2f; // 移动速度
    public Path path; // 路径对象

    private List<Vector2> pathPoints = new List<Vector2>();
    private float currentDistance;
    private int currentIndex = 0;
    private Vector2 moveDirection;
    private Vector2 targetPosition;

    private bool isMoving = false;

    void Start()
    {
        InitPath();
    }

    void Update()
    {
        if (isMoving&&(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D)))
        {
            Once();
        }else{

        }
    }
    private void Once(){
        float distanceToMove = speed * Time.deltaTime;
        currentDistance += distanceToMove;
        int dir = Input.GetKey(KeyCode.A)?-1:1;
        int pointCount = pathPoints.Count;
        for(int i = currentIndex;;i += dir,i = (i + pointCount) % pointCount)
        {
            float segmentLength = (pathPoints[(i + dir + pointCount) % pointCount] - pathPoints[i]).magnitude;
            if (currentDistance <= segmentLength)
            {
                currentIndex = i;
                moveDirection = dir*((pathPoints[(i + dir + pointCount) % pointCount] - pathPoints[i])*dir).normalized;
                targetPosition = pathPoints[i] + moveDirection * currentDistance;
                transform.position = Vector3.Lerp(transform.position, targetPosition, 0.5f);
                break;
            }
            currentDistance -= segmentLength;
        }
        
    }

    void InitPath()
    {
        pathPoints = path.GetPathPoints();

        if (pathPoints.Count > 1)
        {
            currentIndex = 0;
            currentDistance = 0;
            moveDirection = (pathPoints[1] - pathPoints[0]).normalized;
            targetPosition = pathPoints[0];
            transform.position = targetPosition;
            isMoving = true;
        }
        else
        {
            Debug.LogError("Path has less than 2 points, cannot move.");
            isMoving = false;
        }
    }

    void ResetMovement()
    {
        currentDistance = 0;
        currentIndex = 0;
        moveDirection = (pathPoints[1] - pathPoints[0]).normalized;
        targetPosition = pathPoints[0];
        transform.position = targetPosition;
    }
}
