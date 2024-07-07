/*
文件名：InfinityHealth.cs
编辑人: 小途
文件描述：控制莫比乌斯环的血量状态
组件依赖：Rigidbody2D, CircleCollider2D
需要在unity编辑器中拖动赋值的属性：“quanquan:莫比乌斯环不健康时的红圈的GameObject”
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityHealth : MonoBehaviour
{

    public GameObject defeat;//失败场景
    public GameObject quanquan;                 // 圈圈
    public float damageThreshold = 15f;         // 伤害阈值
    public int maxHealth = 30;                  // 最大血量
    public int currentHealth;                   // 当前血量
    public int healAmount = 1;                  // 每次恢复的血量
    public float healInterval = 1f;             // 每次恢复的时间间隔
    public float damageCooldown = 5f;           // 受到伤害后等待多长时间开始恢复

    private float lastDamageTime;               // 记录最后一次受到伤害的时间
    private bool isHealing;                     // 是否正在恢复血量
    // public GameObject defeat;//失败窗口

    void Start()
    {
        currentHealth = maxHealth; // 初始化血量为最大值
        lastDamageTime = -damageCooldown; // 确保开始时没有等待恢复时间
        isHealing = false;
    }

    void Update()
    {
        // 检查是否到了恢复时间
        if (Time.time - lastDamageTime >= damageCooldown && !isHealing)
        {
            StartCoroutine(HealOverTime());
        }
        // 设置ui的状态
        if(currentHealth<=damageThreshold)
        {
            quanquan.SetActive(true);
        }
        else
        {
            quanquan.SetActive(false);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Gameover();
        }

        lastDamageTime = Time.time; // 更新最后一次受到伤害的时间
        StopCoroutine(HealOverTime()); // 如果正在恢复，停止恢复
        isHealing = false;
        // Debug.Log("Took damage, current health: " + currentHealth);
    }

    private IEnumerator HealOverTime()
    {
        isHealing = true;

        while (currentHealth < maxHealth)
        {
            currentHealth += healAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            // Debug.Log("Healing, current health: " + currentHealth);

            yield return new WaitForSeconds(healInterval);

            // 如果在恢复期间受到了伤害，停止恢复
            if (Time.time - lastDamageTime < damageCooldown)
            {
                isHealing = false;
                yield break;
            }
        }

        isHealing = false;
    }
    private void Gameover()
    {
        defeat.SetActive(true);
    }
}
