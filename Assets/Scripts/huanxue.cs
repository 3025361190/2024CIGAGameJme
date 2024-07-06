using UnityEngine;

public class huanxue : MonoBehaviour
{
    public int currentHealth;
    private int maxHealth = 30;
    private float timeSinceLastDamage;
    private float damageThreshold = 15f;
    private float regenTimeThreshold = 5f;
    private float regenRate = 1f; // һ���ӻظ���Ѫ��
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

        // ��������ظ�Ѫ������ֵ����ʼ�ظ�Ѫ��
        if (timeSinceLastDamage >= regenTimeThreshold)
        {
            int regenAmount = Mathf.FloorToInt(timeSinceLastDamage / regenTimeThreshold) * Mathf.FloorToInt(regenRate); // ����Ӧ�ûظ���Ѫ��
            currentHealth = Mathf.Min(currentHealth + regenAmount, maxHealth);
            timeSinceLastDamage = 0f; // ����ʱ��
        }

        // ������ʾ quanquan ���߼�
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

        timeSinceLastDamage = 0f; // �������˺�ʱ��

    }
}
