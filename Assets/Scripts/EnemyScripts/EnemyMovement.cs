/*
文件名：EnemyMovement.cs
编辑人：Fortunate瑞
文件描述：敌人移动脚本,挂载在敌人预制体上,用于控制敌人的移动
组件依赖：Rigidbody2D, CircleCollider2D
rigidbody2d组件需要设置为 Dynamic
绑定：
通过"Turret"标签查找炮台并触发碰撞检测
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Vector2 targetPosition;              // 目标位置
    // 由enemySpawner赋值以下属性
    public float moveSpeed;                 // 移动速度
    public float randomRange;               // 随机移动的幅度
    public float changeDirectionInterval;   // 改变随机方向的时间间隔
    public float knockbackBaseForce;        // 击退力基础值
    public float knockbackTime;             // 击退持续时间
    public float bufferTime;                // 缓冲时间
    public float chainKnockbackForceMultiplier;           // 连锁击退的力系数
    private Vector2 currentDirection;           // 当前移动方向
    private float timeSinceLastChange;          // 上次改变随机方向的时间
    private bool isKnockedBack = false;        // 是否正在被击退
    private Rigidbody2D rb;                     // Rigidbody2D组件引用

    void Start()
    {
        // 获取Rigidbody2D组件
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("EnemyMovement requires a Rigidbody2D component!");
            return;
        }

        // 冻结旋转
       rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        

        // 初始化方向为朝向目标位置的方向
        currentDirection = (targetPosition - (Vector2)transform.position).normalized;
        timeSinceLastChange = 0f;
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        // 更新目标位置
        targetPosition = GameObject.FindGameObjectWithTag("Turret").transform.position;

        timeSinceLastChange += Time.fixedDeltaTime;

        // 每隔一段时间改变一次随机方向
        if (timeSinceLastChange >= changeDirectionInterval)
        {
            ChangeRandomDirection();
            timeSinceLastChange = 0f;
        }

        // 使用velocity进行移动
        if (!isKnockedBack)
        {
            rb.velocity = currentDirection * moveSpeed;
        }
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

    // 碰撞检测,当敌人碰撞到collision时触发
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // 碰撞到炮台
        if (!isKnockedBack && collision.gameObject.CompareTag("Turret"))
        {
           KnockbackByVelocity(collision);
        }
        // 处理与其他的敌人的碰撞
        else if (isKnockedBack && collision.gameObject.CompareTag("Enemy"))
        {
            // 计算从自己到敌人的方向作为击退方向
            Vector2 knockbackDirection = ((Vector2)collision.transform.position - (Vector2)transform.position).normalized;
            collision.gameObject.GetComponent<EnemyMovement>().Knockback(knockbackDirection, chainKnockbackForceMultiplier);
        }
    }


    // 和速度有关的击退自己
    public void KnockbackByVelocity(Collision2D collision)
    {
        // 计算从炮台到敌人的方向作为击退方向
        Vector2 knockbackDirection = ((Vector2)transform.position - (Vector2)collision.transform.position).normalized;
            
        // 根据碰撞速度计算击退力，增加一个系数使效果更明显，最小值为基础值
        float knockbackForce = Mathf.Max(collision.relativeVelocity.magnitude * knockbackBaseForce, knockbackBaseForce);
            
        rb.velocity = Vector2.zero;  // 先清除当前速度
        // Debug.Log("击退力：" + knockbackForce + " 方向：" + knockbackDirection);
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        // 启动击退状态协程
        StartCoroutine(KnockbackStateRoutine());
    }

    // 固定力击退自己
    public void Knockback(Vector2 knockbackDirection, float multiplier = 1.0f)
    {
        // 施加击退力
        float knockbackForce = knockbackBaseForce * multiplier;
            
        rb.velocity = Vector2.zero;  // 先清除当前速度
        // Debug.Log("击退力：" + knockbackForce + " 方向：" + knockbackDirection);
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        // 启动击退状态协程
        StartCoroutine(KnockbackStateRoutine());
    }

    // 击退状态协程
    private IEnumerator KnockbackStateRoutine()
    {
        isKnockedBack = true;
        // 等待击退时间
        yield return new WaitForSeconds(knockbackTime);
        // 原地缓冲
        rb.velocity = Vector2.zero;
        // 缓冲时间
        yield return new WaitForSeconds(bufferTime);        
        isKnockedBack = false;
    }

}
