/*
文件名：EnemySpawner.cs
编辑人：Fortunate瑞
文件描述：敌人生成器,单独挂载在一个空物体上,用于生成敌人
组件依赖：无
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;          // 敌人预制体, 在unity编辑器中拖动赋值
    public float spawnInterval = 2.0f;      // 生成敌人的间隔时间
    private float timer = 0.0f;             // 计时器

    // Start is called before the first frame update
    void Start()
    {
        
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
        Vector2 spawnPosition = new(Random.Range(-10, 10), Random.Range(-10, 10));
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
    
}
