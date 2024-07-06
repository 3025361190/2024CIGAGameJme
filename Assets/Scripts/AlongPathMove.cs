using System.Collections.Generic;
using UnityEngine;

//移动类型，Once是只移动到终点就停止，Loop是进行走到终点再重新回到起点，Yoyo是循环往返
public enum MoveType
{
    Once,
    Loop,
    Yoyo
}
public class AlongPathMove : MonoBehaviour
{
    public float speed = 2f; // 移动速度
    public Path path; // 路径对象
    public MoveType moveType = MoveType.Once; // 移动类型

    private List<Vector2> pathPoints = new List<Vector2>();
    private float totalLength;
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
        if (isMoving)
        {
            
            float distanceToMove = speed * Time.deltaTime;
            currentDistance += distanceToMove;

            if (currentDistance < totalLength)
            {
                for (int i = currentIndex; i < pathPoints.Count - 1; i++)
                {
                    float segmentLength = (pathPoints[i + 1] - pathPoints[i]).magnitude;

                    if (currentDistance <= segmentLength)
                    {
                        currentIndex = i;
                        moveDirection = (pathPoints[i + 1] - pathPoints[i]).normalized;
                        targetPosition = pathPoints[i] + moveDirection * currentDistance;
                        transform.position = targetPosition;
                        break;
                    }

                    currentDistance -= segmentLength;
                }
            }
            else
            {
                switch (moveType)
                {
                    case MoveType.Once:
                        isMoving = false;
                        break;
                    case MoveType.Loop:
                        ResetMovement();
                        break;
                    case MoveType.Yoyo:
                        pathPoints.Reverse();
                        ResetMovement();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void InitPath()
    {
        pathPoints = path.GetPathPoints();

        if (pathPoints.Count > 1)
        {
            totalLength = 0;
            for (int i = 1; i < pathPoints.Count; i++)
            {
                totalLength += (pathPoints[i] - pathPoints[i - 1]).magnitude;
            }
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
