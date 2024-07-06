/*
文件名：EnemyMovement.cs
编辑人：Fortunate瑞
文件描述：敌人移动脚本,挂载在敌人预制体上,用于控制敌人的移动
组件依赖：Rigidbody2D, CircleCollider2D
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Vector2 targetPosition;              // 目标位置
    public float moveSpeed = 1f;                // 移动速度
    public float randomRange = 1f;              // 随机移动的幅度
    public float changeDirectionInterval = 2f;  // 改变随机方向的时间间隔

    private Vector2 currentDirection;           // 当前移动方向
    private float timeSinceLastChange;          // 上次改变随机方向的时间
    private float knockbackDistance = 1f;       // 退后的距离

    void Start()
    {
        // 初始化方向为朝向目标位置的方向
        currentDirection = (targetPosition - (Vector2)transform.position).normalized;
        timeSinceLastChange = 0f;
    }

    void Update()
    {
        timeSinceLastChange += Time.deltaTime;

        // 每隔一段时间改变一次随机方向
        if (timeSinceLastChange >= changeDirectionInterval)
        {
            ChangeRandomDirection();
            timeSinceLastChange = 0f;
        }

        // 移动对象
        transform.position += (Vector3)(currentDirection * moveSpeed * Time.deltaTime);
    }

    void ChangeRandomDirection()
    {
        // 计算朝向目标位置的方向
        Vector2 directionToTarget = (targetPosition - (Vector2)transform.position).normalized;

        // 生成一个随机方向
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        // 将朝向目标位置的方向和随机方向混合
        currentDirection = (directionToTarget + randomDirection * randomRange).normalized;
    }

    // 碰撞检测,当敌人碰撞到Infinity时触发
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Infinity"))
        {
            // 获取第一个碰撞点
            ContactPoint2D contact = collision.GetContact(0);

            // 计算退后的方向
            Vector2 knockbackDirection = (transform.position - (Vector3)contact.point).normalized;
            // 可以增加反弹的方向啥的
            // ...

            // 检查退后路径是否被阻挡
            RaycastHit2D hit = Physics2D.Raycast(transform.position, knockbackDirection, knockbackDistance);
            if (hit.collider == null)
            {
                // 没有障碍物，安全退后
                transform.position += (Vector3)knockbackDirection * knockbackDistance;
            }
            else
            {
                // 有障碍物，调整退后距离为距离障碍物的距离减去一个小量
                float adjustedDistance = hit.distance - 0.1f;
                transform.position += (Vector3)knockbackDirection * adjustedDistance;
            }
        }
    }

}
