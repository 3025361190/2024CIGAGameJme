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
    public float knockbackDistance = 1f;        // 退后的距离
    public float knockbackTime = 0.5f;          // 退后持续时间
    public float bufferTime = 1f;               // 缓冲时间
    public float moveBackTime = 1f;             // 移动回目标的时间

    private Vector2 currentDirection;           // 当前移动方向
    private float timeSinceLastChange;          // 上次改变随机方向的时间
     private bool isKnockedBack = false;        // 是否正在被击退
    

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
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log("Collision detected");
        if (collision.gameObject.CompareTag("Infinity"))
        {
            if (collision.gameObject.CompareTag("Infinity") && !isKnockedBack)
            {
                ContactPoint2D contact = collision.GetContact(0);
                Vector2 knockbackDirection = (transform.position - (Vector3)contact.point).normalized;

                // 启动退后协程
                StartCoroutine(KnockbackRoutine(knockbackDirection));
            }
        }
    }

    private IEnumerator KnockbackRoutine(Vector2 direction)
    {
        isKnockedBack = true;
        Vector2 startPosition = transform.position;
        Vector2 knockbackPosition = startPosition + direction * knockbackDistance;

        // 平滑退后
        float elapsedTime = 0f;
        while (elapsedTime < knockbackTime)
        {
            transform.position = Vector2.Lerp(startPosition, knockbackPosition, elapsedTime / knockbackTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = knockbackPosition;

        // 缓冲时间
        yield return new WaitForSeconds(bufferTime);

        // 平滑移动回原位置
        elapsedTime = 0f;
        while (elapsedTime < moveBackTime)
        {
            transform.position = Vector2.Lerp(knockbackPosition, startPosition, elapsedTime / moveBackTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;

        isKnockedBack = false;
    }

}
