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
    public GameObject enemyPrefab_red;          // 敌人预制体, 在unity编辑器中拖动赋值
    public GameObject enemyPrefab_yellow;          // 敌人预制体, 在unity编辑器中拖动赋值
    public GameObject enemyPrefab_blue;          // 敌人预制体, 在unity编辑器中拖动赋值
    public GameObject enemyPrefab_purple;          // 敌人预制体, 在unity编辑器中拖动赋值
    public GameObject enemyPrefab_white;          // 敌人预制体, 在unity编辑器中拖动赋值
    public float spawnInterval;             // 生成敌人的间隔时间
    // 敌人移动参数,生成enemy后，赋值给enemy的EnemyMovement组件中的成员
    public float moveSpeed;                 // 移动速度,由GameManager在每关开始时赋值
    public int damage;                      // 敌人伤害值,由GameManager在每关开始时赋值
    public float randomRange;               // 随机移动的幅度
    public float changeDirectionInterval;   // 改变随机方向的时间间隔
    public float knockbackBaseForce;        // 击退力基础值
    public float knockbackTime;             // 击退持续时间
    public float bufferTime;                // 缓冲时间
    public float attackCooldownTime;        // 伤害冷却时间
    public float chainEffectRadius;         // 连锁效果半径
    public float chainExplosionDelay;       // 连锁爆炸延迟
    public float chainKnockbackForceMultiplier;           // 连锁击退的力系数
    public int maxEnemyCount;                // 最大敌人数量

    private float timer = 0.0f;             // 计时器
    // public float radius = 8.0f;              // 圆的半径
    // 预定义的生成点
    private readonly Vector2[] spawnPoints = new Vector2[]
    {
        new(-10, -5),    // 左下
        new(10, -5),     // 右下
        new(-10, 5),     // 左上
        new(10, 5),      // 右上
        new(-10, 0),    // 左中
        new(10, 0),     // 右中
        new(0, -6),    // 下中
        new(0, 6)      // 上中
    };       

    private List<GameObject> enemyList = new();    // 用于注册enemy实例
    private int enemyCount = 0;                   // 敌人数量计数器


    // 初始化
    void Start()
    {
        // 从enemy_config.json中获取spawnInterval
        var enemyConfig = GameManager.Instance.data.enemyData;
        if (enemyConfig != null)
        {   
            // 获取enemy_config.json中的数据
            try
            {
                // 乘以系数
                spawnInterval = enemyConfig["spawnInterval"].ToObject<float>()*spawnInterval;
                // 乘以系数
                moveSpeed = enemyConfig["moveSpeed"].ToObject<float>()*moveSpeed;
                // 乘以系数
                damage = enemyConfig["damage"].ToObject<int>()*damage;
                randomRange = enemyConfig["randomRange"].ToObject<float>();
                changeDirectionInterval = enemyConfig["changeDirectionInterval"].ToObject<float>();
                knockbackBaseForce = enemyConfig["knockbackBaseForce"].ToObject<float>();
                knockbackTime = enemyConfig["knockbackTime"].ToObject<float>();
                bufferTime = enemyConfig["bufferTime"].ToObject<float>();
                attackCooldownTime = enemyConfig["attackCooldownTime"].ToObject<float>();
                chainEffectRadius = enemyConfig["chainEffectRadius"].ToObject<float>();
                chainExplosionDelay = enemyConfig["chainExplosionDelay"].ToObject<float>();
                chainKnockbackForceMultiplier = enemyConfig["chainKnockbackForceMultiplier"].ToObject<float>();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"读取敌人配置数据失败: {e.Message}");
            }
            // Debug.Log($"敌人配置加载完成");
        }
        else
        {
            Debug.LogError("加载敌人生成配置失败！");
        }
        Debug.Log($"将会生成 {maxEnemyCount}个小怪");
    }


    // Update is called once per frame
    void Update()
    {
        // 间隔时间生成敌人
        timer += Time.deltaTime;
        if (timer >= spawnInterval && enemyCount < maxEnemyCount)
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
        // 设置敌人颜色
        var enemyColor = (ColorType)Random.Range(0, 5);
        GameObject enemyPrefab = null;
        switch (enemyColor)
        {
            case ColorType.Red:
                enemyPrefab = enemyPrefab_red;
                break;
            case ColorType.Yellow:
                enemyPrefab = enemyPrefab_yellow;
                break;
            case ColorType.Blue:
                enemyPrefab = enemyPrefab_blue;
                break;
            case ColorType.Purple:
                enemyPrefab = enemyPrefab_purple;
                break;
            case ColorType.White:
                enemyPrefab = enemyPrefab_white;
                break;
            default:
                Debug.LogError($"未找到{enemyColor}小怪预制体");
                return;
        }
        // 实例化敌人并注册到enemyList
        GameObject instantiate = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        if(instantiate == null)
        {
            Debug.Log("未找到小怪instantiate");
        }
        // 获取EnemyMovement组件
        if (!instantiate.TryGetComponent<EnemyMovement>(out EnemyMovement enemyMovement))
        {
            Debug.Log("未找到小怪EnemyMovement组件");
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
        // TODO: enemy和boss的参数改为每个level单独配置
        // 目前的思路是保留原来的config文件，需要给level单的配置的变量改为系数，变成1.0
        // 实际应用的时候乘以系数，这样buff仍然能通过修改全局的config来控制，缺点是只能通过乘法，在buff的加法中进行报错

        // 获取Enemy组件
        if (!instantiate.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log("未找到Enemy组件");
            return;
        }
        // 赋值给Enemy组件中的成员
        enemy.enemyColor = enemyColor;
        enemy.damage = damage;
        enemy.attackCooldownTime = attackCooldownTime;
        enemy.ChainEffectRadius = chainEffectRadius;
        enemy.chainExplosionDelay = chainExplosionDelay;

        enemyList.Add(instantiate);
        enemyCount++;
    }

    // 注销enemy实例
    public void RemoveEnemy(GameObject enemy)
    {
        // 如果enemyList中包含enemy，则移除
        if(enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);
            if(enemyList.Count == 0 && enemyCount >= maxEnemyCount)
            {
                // 该类敌人已全部死亡
                GameManager.Instance.AKindOfEnemyAllDead();
                // Debug.Log($"{enemyPrefab.name}已全部死亡");
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

    // 在Scene视图中绘制生成点
    void OnDrawGizmos()
    {
        
        // 绘制所有生成点
        foreach (Vector2 point in spawnPoints)
        {
            Gizmos.color = new Color(0f, 0.5f, 0f, 0.5f); // 半透明的绿色
            // 绘制一个实心球体表示生成点
            Gizmos.DrawSphere(point, 0.3f);
            // 在Scene视图中显示坐标值
            #if UNITY_EDITOR
            UnityEditor.Handles.Label(point, $"({point.x}, {point.y})");
            #endif
        }
    }
}
