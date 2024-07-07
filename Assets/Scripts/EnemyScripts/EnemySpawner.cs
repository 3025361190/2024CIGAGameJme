/*
文件名：EnemySpawner.cs
编辑人：Fortunate瑞
文件描述：敌人生成器,单独挂载在一个空物体EnemySpawnerObject上,用于生成敌人
组件依赖：无
需要在unity编辑器中拖动赋值的属性：“enemyPrefab:敌人预制体”
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;          // 敌人预制体, 在unity编辑器中拖动赋值
    public float spawnInterval = 0.5f;      // 生成敌人的间隔时间
    private float timer = 0.0f;             // 计时器
    // public float radius = 8.0f;              // 圆的半径
    // 预定义的生成点
    public Vector2[] spawnPoints = new Vector2[]
    {
        new(-10, 10),
        new(10, 10),
        new(10, -10),
        new(-10, -10),
        new(0, 10),
        new(0, -10),
        new(-10, 0),
        new(10, 0),
        new(15, 15),
        new(-15, -15)  
    };       

    private List<GameObject> enemyList = new();    // 用于注册enemy实例

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
        // 颜色扎堆
        // ...

        enemyList.Add(instantiate);
    }

    // 注销enemy实例
    public void RemoveEnemy(GameObject enemy)
    {
        enemyList.Remove(enemy);
    }

    // 生成一个随机位置,在一个半径为r的圆外
    Vector2 GetRandomPositionOutsideCircle(float r)
    {
        // 生成一个随机角度（0 到 360 度）
        float theta = Random.Range(0, Mathf.PI * 2);

        // 生成一个半径 r 之外的随机距离
        float distance = Random.Range(r, r+1); // 或者更大范围

        // 将极坐标转换为笛卡尔坐标
        float x = distance * Mathf.Cos(theta);
        float y = distance * Mathf.Sin(theta);

        return new Vector2(x, y);
    }
    
}
