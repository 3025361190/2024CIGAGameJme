/*
文件名：Enemy.cs
编辑人：Fortunate瑞
文件描述：敌人脚本,挂载在敌人预制体上,用于处理敌人的一些触发行为
组件依赖：Rigidbody2D, CircleCollider2D
*/

using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

// 区分unity中不同库中的Vector2
using Vector2 = UnityEngine.Vector2;

public class Enemy : MonoBehaviour
{
    
    public ColorType enemyColor;                // 敌人颜色
    public int amount;                          // 敌人伤害值
    public float ChainEffectRadius = 2.0f;      // 连锁效果半径
    public GameObject baozha;
    private GameObject currentEnemy;

    // Start is called before the first frame update
    private void Start() {
        // 设置敌人颜色
        enemyColor = (ColorType)Random.Range(0, 5);
        // 根据颜色设置资源颜色
        Transform childTransform = transform.Find("body");
        switch (enemyColor)
        {
            case ColorType.Red:
                childTransform.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case ColorType.Yellow:
                childTransform.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case ColorType.Blue:
                childTransform.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case ColorType.White:
                childTransform.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case ColorType.Purple:
                childTransform.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0.5f);
                break;
        }
        amount = 1;
    }

    // 碰撞检测,当敌人碰撞到Infinity时触发,造成伤害
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Infinity"))
        {
            collision.gameObject.GetComponent<InfinityHealth>().TakeDamage(1);
        }
    }

    // 敌人死亡和注销
    public void Die()
    {
        Vector3 currentPosition = transform.position; // 使用 transform.position 获取当前对象的位置
        GameObject newPrefabInstance = Instantiate(baozha, currentPosition, Quaternion.identity);

        // 获取 EnemySpawner 的引用并调用 RemoveEnemy 方法
        EnemySpawner enemySpawner = GameObject.Find("EnemySpawnerObject")?.GetComponent<EnemySpawner>();
        if (enemySpawner != null)
        {
            enemySpawner.RemoveEnemy(gameObject); // 将当前敌人移除
        }

        Destroy(gameObject); // 销毁当前敌人对象
    }

    // 敌人被子弹击中时,处理击中事件,由子弹调用
    public void HandleHit(ColorType bulletColor)
    {
        Debug.Log("Enemy hit by bullet");
        if (bulletColor == enemyColor)
        {
            TriggerChainEffect(enemyColor);
        }
        else
        {
            // 敌人被击中,但是颜色不匹配,不触发连锁效果,敌人消失
            Die();
        }
    }

    // 实现连锁击杀效果,对附近的全部敌人调用TriggerChainEffect方法,传入该敌人自己的颜色实现连锁效果
    void TriggerChainEffect(ColorType enemyColor)
    {
        // // 获取附近范围内所有碰撞体
        // Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, ChainEffectRadius);
        // foreach (var collider in colliders)
        // {
        //     if(collider != null)
        //     {
        //         if (collider.CompareTag("Enemy"))
        //         {
        //             // 后期可增加发射闪电特效等
        //             // ...
        //             // 调用敌人的TriggerChainEffect方法,传入当前敌人的颜色,实现连锁效果
        //             collider.GetComponent<Enemy>().TriggerChainEffect(enemyColor);
        //         }
        //     }
        // }
        // // 敌人死亡
        Die();
    }
}
