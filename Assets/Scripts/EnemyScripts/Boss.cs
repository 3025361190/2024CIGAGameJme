/*
文件名：Boss.cs
编辑人：Fortunate瑞
文件描述：boss类,用于管理boss的逻辑,挂载在boss预制体上
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public int damage;                          // 敌人伤害值
    public float ChainEffectRadius;             // 连锁效果半径
    public float chainExplosionDelay;           // 连锁爆炸延迟
    public float cooldownTime;                  // 伤害冷却时间，由spawner赋值
    // public GameObject bossBaozha;               // 死亡爆炸效果



    private bool canTakeDamage = true;          // 是否可以造成伤害的标志位
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
            // Debug.Log("Boss hit Turret and damage");
            StartCoroutine(CollisionCooldown());  // 开始冷却协程,防止短时间内多次造成伤害 
        }
    }

    private IEnumerator CollisionCooldown()
    {
        canTakeDamage = false;  // 设置不能造成伤害
        yield return new WaitForSeconds(cooldownTime);  // 等待冷却时间
        canTakeDamage = true;  // 冷却完毕，可以再次造成伤害
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}