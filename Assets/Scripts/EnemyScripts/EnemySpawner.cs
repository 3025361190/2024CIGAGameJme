/*
文件名：EnemySpawner.cs
编辑人：Fortunate瑞
文件描述：敌人生成器,单独挂载在一个空物体EnemySpawnerObject上,用于生成敌人
组件依赖：无
需要在unity编辑器中拖动赋值的属性："enemyPrefab:敌人预制体"
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;          // 敌人预制体, 在unity编辑器中拖动赋值
    public float spawnInterval;             // 生成敌人的间隔时间
    // 敌人移动参数,生成enemy后，赋值给enemy的EnemyMovement组件中的成员
    public float moveSpeed;                 // 移动速度
    public int damage;                      // 敌人伤害值
    public float randomRange;               // 随机移动的幅度
    public float changeDirectionInterval;   // 改变随机方向的时间间隔
    public float knockbackDistance;         // 退后的距离
    public float knockbackTime;             // 退后持续时间
    public float bufferTime;                // 缓冲时间
    public float attackCooldownTime;        // 伤害冷却时间
    public float chainExplosionRange;       // 连锁爆炸范围
    public float chainExplosionDelay;       // 连锁爆炸延迟

    private float timer = 0.0f;             // 计时器
    // public float radius = 8.0f;              // 圆的半径
    // 预定义的生成点
    public Vector2[] spawnPoints = new Vector2[]
    {
        new(-5, -10),
        new(5, 10),
        new(-5, 10),
        new(5, -10),
        new(0, -5),
        new(0, 5),
        new(-10, 0),
        new(10, 0),
        new(0, 10),
        new(0, -10)  
    };       

    private List<GameObject> enemyList = new();    // 用于注册enemy实例


    // 初始化
    void Start()
    {
        // 从enemy_config.json中获取spawnInterval
        var enemyConfig = JsonLoader.LoadJsonAsJObject("StaticData/enemy_config");
        if (enemyConfig != null)
        {   
            // 获取enemy_config.json中的数据
            try
            {
                spawnInterval = enemyConfig["spawnInterval"].ToObject<float>();
                moveSpeed = enemyConfig["moveSpeed"].ToObject<float>();
                damage = enemyConfig["damage"].ToObject<int>();
                randomRange = enemyConfig["randomRange"].ToObject<float>();
                changeDirectionInterval = enemyConfig["changeDirectionInterval"].ToObject<float>();
                knockbackDistance = enemyConfig["knockbackDistance"].ToObject<float>();
                knockbackTime = enemyConfig["knockbackTime"].ToObject<float>();
                bufferTime = enemyConfig["bufferTime"].ToObject<float>();
                attackCooldownTime = enemyConfig["attackCooldownTime"].ToObject<float>();
                chainExplosionRange = enemyConfig["chainEffectRadius"].ToObject<float>();
                chainExplosionDelay = enemyConfig["chainExplosionDelay"].ToObject<float>();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"读取敌人配置数据失败: {e.Message}");
            }
            Debug.Log($"敌人配置加载完成");
        }
        else
        {
            Debug.LogError("加载敌人生成配置失败！");
        }
    }


    // Update is called once per frame
    void Update()
    {
        // 间隔时间生成敌人
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0.0f;
        }
    }

    // 生成敌人
    void SpawnEnemy()
    {
        // Vector2 spawnPosition = GetRandomPositionOutsideCircle(radius);
        // 随机选择一个生成点
        Vector2 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)];
        // 实例化敌人并注册到enemyList
        GameObject instantiate = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        if(instantiate == null)
        {
            Debug.Log("未找到instantiate");
        }
        // 获取EnemyMovement组件
        EnemyMovement enemyMovement = instantiate.GetComponent<EnemyMovement>();
        if(enemyMovement == null)
        {
            Debug.Log("未找到EnemyMovement组件");
        }
        // 赋值给EnemyMovement组件中的成员
        enemyMovement.moveSpeed = moveSpeed;
        enemyMovement.randomRange = randomRange;
        enemyMovement.changeDirectionInterval = changeDirectionInterval;
        enemyMovement.knockbackDistance = knockbackDistance;
        enemyMovement.knockbackTime = knockbackTime;
        enemyMovement.bufferTime = bufferTime;

        // 获取Enemy组件
        Enemy enemy = instantiate.GetComponent<Enemy>();
        if(enemy == null)
        {
            Debug.Log("未找到Enemy组件");
        }
        // 赋值给Enemy组件中的成员
        enemy.damage = damage;
        enemy.cooldownTime = attackCooldownTime;
        enemy.ChainEffectRadius = chainExplosionRange;
        enemy.chainExplosionDelay = chainExplosionDelay;

        enemyList.Add(instantiate);
    }

    // 注销enemy实例
    public void RemoveEnemy(GameObject enemy)
    {
        enemyList.Remove(enemy);
    }

    // // 生成一个随机位置,在一个半径为r的圆外
    // Vector2 GetRandomPositionOutsideCircle(float r)
    // {
    //     // 生成一个随机角度（0 到 360 度）
    //     float theta = Random.Range(0, Mathf.PI * 2);

    //     // 生成一个半径 r 之外的随机距离
    //     float distance = Random.Range(r, r+1); // 或者更大范围

    //     // 将极坐标转换为笛卡尔坐标
    //     float x = distance * Mathf.Cos(theta);
    //     float y = distance * Mathf.Sin(theta);

    //     return new Vector2(x, y);
    // }


    // TODO: 瑞，维护enemyList的时候同时维护一个计数器，当计数器达到一定数量时，不再生成敌人。然后当list被清空时，达成关卡结束条件
    
}
