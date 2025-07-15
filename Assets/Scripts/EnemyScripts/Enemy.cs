/*
文件名：Enemy.cs
编辑人：Fortunate瑞
文件描述：敌人脚本,挂载在敌人预制体上,用于处理敌人的一些触发行为
组件依赖：Rigidbody2D, CircleCollider2D
*/

using System.Collections;
using UnityEngine;

// 区分unity中不同库中的Vector2
using Vector2 = UnityEngine.Vector2;

public class Enemy : MonoBehaviour
{
    
    public ColorType enemyColor;                // 敌人颜色
    public int damage;                          // 敌人伤害值
    public float ChainEffectRadius;             // 连锁效果半径
    public float chainExplosionDelay;           // 连锁爆炸延迟
    public float attackCooldownTime;            // 伤害冷却时间，由spawner赋值



    private bool canTakeDamage = true;          // 是否可以造成伤害的标志位
    private bool isChaining = false;           // 是否正在连锁中
    public GameObject baozha;
    // private GameObject currentEnemy;

    // Start is called before the first frame update
    private void Start() {
        // // 设置敌人颜色
        // enemyColor = (ColorType)Random.Range(0, 5);
        // // 根据颜色设置资源颜色
        // Transform childTransform = transform.Find("body");
        // switch (enemyColor)
        // {
        //     case ColorType.Red:
        //         childTransform.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 126f / 255f, 191f / 255f);
        //         break;
        //     case ColorType.Yellow:
        //         childTransform.GetComponent<SpriteRenderer>().color = new Color(234f / 255f, 253f / 255f, 3f / 255f);
        //         break;
        //     case ColorType.Blue:
        //         childTransform.GetComponent<SpriteRenderer>().color = new Color(151f / 255f, 255f / 255f, 239f / 255f);
        //         break;
        //     case ColorType.White:
        //         childTransform.GetComponent<SpriteRenderer>().color = new Color(253f / 255f, 255f / 255f, 255f / 255f);
        //         break;
        //     case ColorType.Purple:
        //         childTransform.GetComponent<SpriteRenderer>().color = new Color(197f / 255f, 156f / 255f, 255f / 255f);
        //         break;
        // }
    }


    // 碰撞检测,当敌人碰撞到Turret时触发,造成伤害
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Turret"))
        {
            // Debug.Log("Enemy hit Turret but not damage");
        }
        if (collision.gameObject.CompareTag("Turret") && canTakeDamage)
        {
            if(collision.gameObject.GetComponent<PlayerHealth>().isFlashing == false)
            {
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
            // Debug.Log("Enemy hit Turret and damage");
            StartCoroutine(CollisionCooldown());  // 开始冷却协程,防止短时间内多次造成伤害
        }
    }

    // 敌人死亡和注销
    // deadType: 0-正常死亡, 1-连锁死亡
    public void Die(int deadType = 0)
    {
        // TODO：瑞，连锁死亡时，需要播放连锁死亡的特效
        Vector3 currentPosition = transform.position; // 使用 transform.position 获取当前对象的位置
        GameObject newPrefabInstance = Instantiate(baozha, currentPosition, Quaternion.identity);

        // 获取 EnemySpawner 的引用并调用 RemoveEnemy 方法
        GameObject spawnerObject = GameObject.Find("EnemySpawnerObject");
        if (spawnerObject != null)
        {
            EnemySpawner[] enemySpawners = spawnerObject.GetComponents<EnemySpawner>();
            foreach (var enemySpawner in enemySpawners)
            {
                // 暴力遍历所有EnemySpawner，并调用RemoveEnemy方法，在Remove中判断是否包含当前敌人
                // 瑞，这里可以优化，因为我在同一个EnemySpawnerObject中添加了多个EnemySpawner.cs脚本，分别生成蔬菜哥和敌人
                enemySpawner.RemoveEnemy(gameObject);
            }
        }

        Destroy(gameObject); // 销毁当前敌人对象
    }

    // 敌人被子弹击中时,处理击中事件,由子弹调用
    public void HandleHit(ColorType bulletColor)
    {
        // Debug.Log("Enemy hit by bullet");
        if (bulletColor == enemyColor && !isChaining)
        {
            TriggerChainEffect(enemyColor);
        }
        else
        {
            // 敌人被击中,但是颜色不匹配,或者已经触发过连锁。则不触发连锁效果,敌人立即死亡
            Die();
        }
    }

    // 实现连锁击杀效果,对附近的全部敌人调用TriggerChainEffect方法,传入该敌人自己的颜色实现连锁效果
    void TriggerChainEffect(ColorType enemyColor, float deltaTime = 0)
    {
        // Debug.Log("触发连锁爆炸");
        isChaining = true;

        // 获取附近范围内所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, ChainEffectRadius);
        // Debug.Log($"找到 {colliders.Length} 个碰撞体");
        foreach (var collider in colliders)
        {
            if (collider != null && collider.CompareTag("Enemy"))
            {
                // Debug.Log("找到敌人");
                Enemy nearbyEnemy = collider.GetComponent<Enemy>();
                if (nearbyEnemy != null && nearbyEnemy.enemyColor == enemyColor && !nearbyEnemy.isChaining)
                {
                    // Debug.Log("找到敌人，且颜色匹配！");
                    // 递归调用，传递当前延迟时间加上延迟间隔
                    nearbyEnemy.TriggerChainEffect(enemyColor, deltaTime + chainExplosionDelay);
                }
            }
        }

        // 启动延迟死亡的协程
        StartCoroutine(DelayedDeath(deltaTime));
    }

    private IEnumerator DelayedDeath(float deltaTime)
    {
        // 等待延迟时间
        yield return new WaitForSeconds(chainExplosionDelay * deltaTime);
        
        // 检查对象是否还存在
        if (gameObject != null)
        {
            Die(1);
        }
    }

    private IEnumerator CollisionCooldown()
    {
        canTakeDamage = false;  // 设置不能造成伤害
        yield return new WaitForSeconds(attackCooldownTime);  // 等待冷却时间
        canTakeDamage = true;  // 冷却完毕，可以再次造成伤害
    }

    // 在Scene视图中绘制连爆范围
    void OnDrawGizmos()
    {
        // Debug.Log("OnDrawGizmos");
        // 在Scene视图中绘制探测范围
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f); // 半透明的红色
        Gizmos.DrawWireSphere(transform.position, ChainEffectRadius);
        
        // 添加一个实心球体使范围更容易看到
        Gizmos.color = new Color(1f, 0f, 0f, 0.1f); // 非常淡的红色
        Gizmos.DrawSphere(transform.position, ChainEffectRadius);
        
        // 在Scene视图中显示半径值
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position, $"Radius: {ChainEffectRadius}");
        #endif
    }
}
