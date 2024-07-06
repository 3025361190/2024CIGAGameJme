using UnityEngine;

public class huanxue : MonoBehaviour
{
    public int currentHealth;
    private int maxHealth = 30;
    private float timeSinceLastDamage;
    private float damageThreshold = 15f;
    private float regenTimeThreshold = 5f;
    private float regenRate = 1f; // 一秒钟回复的血量
    public GameObject quanquan;
    public GameObject defeat;
    void Start()
    {
        currentHealth = maxHealth;
        timeSinceLastDamage = 0f;
    }

    void Update()
    {
        timeSinceLastDamage += Time.deltaTime;

        // 如果超过回复血量的阈值，开始回复血量
        if (timeSinceLastDamage >= regenTimeThreshold)
        {
            int regenAmount = Mathf.FloorToInt(timeSinceLastDamage / regenTimeThreshold) * Mathf.FloorToInt(regenRate); // 计算应该回复的血量
            currentHealth = Mathf.Min(currentHealth + regenAmount, maxHealth);
            timeSinceLastDamage = 0f; // 重置时间
        }

        // 处理显示 quanquan 的逻辑
        if (currentHealth <= damageThreshold)
        {
            quanquan.SetActive(true);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
           defeat.SetActive(true);
        }

        timeSinceLastDamage = 0f; // 重置无伤害时间

    }
}
