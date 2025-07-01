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

    // TODO: 瑞，移到bossHealth.cs中
    // public void HandleHit(ColorType bulletColor)
    // {
    //     // Debug.Log("Enemy hit by bullet");
    //     if (bulletColor == enemyColor && !isChaining)
    //     {
    //         TriggerChainEffect(enemyColor);
    //     }
    //     else
    //     {
    //         // 敌人被击中,但是颜色不匹配,或者已经触发过连锁。则不触发连锁效果,敌人立即死亡
    //         Die();
    //     }
    // }

    // public void Die()
    // {
    //     Vector3 currentPosition = transform.position; // 使用 transform.position 获取当前对象的位置
    //     GameObject newPrefabInstance = Instantiate(bossBaozha, currentPosition, Quaternion.identity);

    //     // 获取 BossSpawner 的引用并调用 RemoveBoss 方法
    //     GameObject spawnerObject = GameObject.Find("BossSpawnerObject");
    //     if (spawnerObject != null)
    //     {
    //         BossSpawner[] bossSpawners = spawnerObject.GetComponents<BossSpawner>();
    //         foreach (var bossSpawner in bossSpawners)
    //         {
    //             // 暴力遍历所有BossSpawner，并调用RemoveBoss方法，在Remove中判断是否包含当前boss
    //             bossSpawner.RemoveBoss(gameObject);
    //         }
    //     }

    //     Destroy(gameObject); // 销毁当前敌人对象
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}