/*
文件名：BosssSpawner.cs
编辑人：Fortunate瑞
文件描述：boss生成器,单独挂载在一个空物体BosssSpawnerObject上,用于生成boss
组件依赖：无
需要在unity编辑器中拖动赋值的属性："bossPrefab:Boss预制体"
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    // boss预制体,在unity编辑器中拖动赋值
    public GameObject bossPrefab;
    public int bossHealthPoint;             // boss max 血量
    public float moveSpeed;                 // 移动速度
    public int damage;                      // 敌人伤害值
    public float randomRange;               // 随机移动的幅度
    public float changeDirectionInterval;   // 改变随机方向的时间间隔
    public float knockbackBaseForce;        // 击退力基础值
    public float knockbackTime;             // 击退持续时间
    public float bufferTime;                // 缓冲时间
    public float attackCooldownTime;        // 伤害冷却时间
    public float chainKnockbackForceMultiplier;           // 连锁击退的力系数
    public int maxBossCount;                // 最大boss数量
    public float spawnTime;                 // 生成boss的时间

    // 预定义的生成点
    Vector2[] spawnPoints = new Vector2[]
    {
        new(-10, -5),   // 左下
        new(10, -5),    // 右下
        new(-10, 5),    // 左上
        new(10, 5),     // 右上
        new(-10, 0),    // 左中
        new(10, 0),     // 右中
        new(0, -6),     // 下中
        new(0, 6)       // 上中
    };


    // 运行时
    private List<GameObject> bossList = new();    // 用于注册boss实例
    private int bossCount = 0;                   // boss数量计数器


    // Start is called before the first frame update
    void Start()
    {
        // 从boss_config.json中获取boss的配置数据
        var bossConfig = GameManager.Instance.data.bossData;
        if (bossConfig != null)
        {   
            // 获取boss_config.json中的数据
            try
            {
                moveSpeed = bossConfig["moveSpeed"].ToObject<float>();
                damage = bossConfig["damage"].ToObject<int>();
                randomRange = bossConfig["randomRange"].ToObject<float>();
                changeDirectionInterval = bossConfig["changeDirectionInterval"].ToObject<float>();
                knockbackBaseForce = bossConfig["knockbackBaseForce"].ToObject<float>();
                knockbackTime = bossConfig["knockbackTime"].ToObject<float>();
                bufferTime = bossConfig["bufferTime"].ToObject<float>();
                attackCooldownTime = bossConfig["attackCooldownTime"].ToObject<float>();
                chainKnockbackForceMultiplier = bossConfig["chainKnockbackForceMultiplier"].ToObject<float>();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"读取boss配置数据失败: {e.Message}");
            }
            // Debug.Log($"boss配置加载完成");
        }
        else
        {
            Debug.LogError("加载boss配置失败！");
        }
        
        var levelConfig = GameManager.Instance.currentLevelConfig;
        if(levelConfig != null)
        {
            spawnTime = levelConfig.bossTime;
            bossHealthPoint = levelConfig.bossHealthPoint;
            maxBossCount = levelConfig.isBoss;
        }
        else
        {
            Debug.LogError("读取当前关卡的boss配置失败！");
        }
        Debug.Log($"将会生成 {maxBossCount}个boss");
    }


    // 生成敌人
    void SpawnBoss()
    {
        // Vector2 spawnPosition = GetRandomPositionOutsideCircle(radius);
        // 随机选择一个生成点
        Vector2 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)];
    
        // 实例化敌人并注册到enemyList
        GameObject instantiate = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        if(instantiate == null)
        {
            Debug.Log("未找到Boss clone实体");
        }
        // 获取EnemyMovement组件
        if (!instantiate.TryGetComponent<EnemyMovement>(out EnemyMovement enemyMovement))
        {
            Debug.Log("未找到Boss EnemyMovement组件");
            return;
        }
        // 赋值给EnemyMovement组件中的成员
        enemyMovement.moveSpeed = moveSpeed;
        enemyMovement.randomRange = randomRange;
        enemyMovement.changeDirectionInterval = changeDirectionInterval;
        enemyMovement.knockbackBaseForce = knockbackBaseForce;
        enemyMovement.knockbackTime = knockbackTime;
        enemyMovement.bufferTime = bufferTime;
        enemyMovement.chainKnockbackForceMultiplier = chainKnockbackForceMultiplier;

        // 获取Boss组件
        if (!instantiate.TryGetComponent<Boss>(out Boss boss))
        {
            Debug.Log("未找到Boss组件");
        }
        // 赋值给Boss组件中的成员
        boss.damage = damage;
        boss.cooldownTime = attackCooldownTime;

        // 获取BossHealth组件
        if (!instantiate.TryGetComponent<BossHealth>(out BossHealth bossHealth))
        {
            Debug.Log("未找到BossHealth组件");
        }
        // Debug.Log("bossSpawner中调用SetBossHealthPoint");
        // 赋值给BossHealth组件中的成员
        bossHealth.SetBossHealthPoint(bossHealthPoint);




        bossList.Add(instantiate);
        bossCount++;
    }

    // 注销enemy实例
    public void RemoveBoss(GameObject boss)
    {
        // 如果bossList中包含boss，则移除
        if(bossList.Contains(boss))
        {
            bossList.Remove(boss);
            if(bossList.Count == 0 && bossCount >= maxBossCount)
            {
                // Boss已全部死亡
                GameManager.Instance.isAllBossDead = true;
                // Debug.Log($"{bossPrefab.name}已全部死亡");
            }
        }
        else
        {
            // 因为我在同一个EnemySpawnerObject中添加了多个EnemySpawner.cs脚本，分别生成蔬菜哥和敌人
            // 调用remove时直接暴力的都调用了，所以需要判断是否包含enemy
            // Debug.Log("未找到匹配的enemy");
            return;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.GetPassedTime() >= spawnTime && bossCount < maxBossCount)
        {
            SpawnBoss();
        }
    }
}
