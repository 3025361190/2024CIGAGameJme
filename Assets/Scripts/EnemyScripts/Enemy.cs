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

    // Start is called before the first frame update
    private void Start() {
        // 设置敌人颜色
        enemyColor = (ColorType)Random.Range(0, 5);
        amount = 1;
    }

    // 碰撞检测,当敌人碰撞到Infinity时触发,造成伤害
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Infinity"))
        {
            collision.gameObject.GetComponent<InfinityHealth>().TakeDamage(1);
        }
    }

    // 敌人死亡和注销
    public void Die()
    {
        GameObject.Find("EnemySpawnerObject").GetComponent<EnemySpawner>().RemoveEnemy(gameObject);
        Destroy(gameObject);
    }

    // 敌人被子弹击中时,处理击中事件,由子弹调用
    public void HandleHit(ColorType bulletColor)
    {
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
        // 获取附近范围内所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, ChainEffectRadius);
        foreach (var collider in colliders)
        {
            if(collider != null)
            {
                if (collider.CompareTag("Enemy"))
                {
                    // 后期可增加发射闪电特效等
                    // ...
                    // 调用敌人的TriggerChainEffect方法,传入当前敌人的颜色,实现连锁效果
                    collider.GetComponent<Enemy>().TriggerChainEffect(enemyColor);
                }
            }
        }
        // 敌人死亡
        Die();
    }
}
