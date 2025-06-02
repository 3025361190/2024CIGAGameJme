/*
文件名：PlayerHealth.cs
编辑人：fortunate瑞
文件描述：PlayerHealth类，用于管理玩家生命值，受伤闪烁效果等
绑定：
1. 在场景中添加health物体
2. 在health物体下添加TextMeshProUGUI组件
3. 给player添加PlayerHealth脚本
*/


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    
    
    private float flashDuration;                    // 闪烁持续时间

    public TextMeshProUGUI healthText;              // 玩家血量文本，代码在场景中查找绑定
    public GameObject spriteRenderer;          // 玩家图片，用于闪烁效果，代码中绑定
    private Color originalColor;                    // 玩家图片原始颜色

    private float flashTimer = 0f;                  // 闪烁计时器
    public bool isFlashing = false;               // 是否闪烁

    private float flashInterval; // 闪烁间隔时间
    private float flashIntervalTimer = 0f; // 闪烁计时器
    private bool isRedColor = false;    // 当前是否为红色


    private void Awake()
    {
        // 从global_config.json中读取闪烁持续时间
        var globalConfig = JsonLoader.LoadJsonAsJObject("StaticData/global_config");
        if (globalConfig != null)
        {   
            try
            {
                flashDuration = globalConfig["flashDuration"].ToObject<float>();
                flashInterval = globalConfig["flashInterval"].ToObject<float>();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"读取闪烁持续时间失败: {e.Message}");
            }
        }
        else
        {
            Debug.LogError("加载全局配置失败！");
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        // 用名字获取子组件引用
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.GetComponent<SpriteRenderer>().color;
        }

        // 查找并绑定血量文本
        if (healthText == null)
        {
            GameObject healthObj = GameObject.Find("health");
            if (healthObj != null)
            {
                healthText = healthObj.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("未找到health物体！");
            }
            if (healthText == null)
            {
                Debug.LogError("未找到血量文本组件！");
            }
        }

        // 初始化显示
        UpdateHealthDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        // 处理受伤闪烁效果
        if (isFlashing)
        {
            Debug.Log("Flashing: " + flashTimer);
            flashTimer += Time.deltaTime;
            flashIntervalTimer += Time.deltaTime;

            // 每过一个间隔时间就切换一次颜色
            if (flashIntervalTimer >= flashInterval)
            {
                isRedColor = !isRedColor; // 切换颜色状态
                spriteRenderer.GetComponent<SpriteRenderer>().color = isRedColor ? Color.red : originalColor;
                flashIntervalTimer = 0f; // 重置计时器
            }
            
            if (flashTimer >= flashDuration)
            {
                spriteRenderer.GetComponent<SpriteRenderer>().color = originalColor;
                isFlashing = false;
                flashTimer = 0f;
                isRedColor = false;
                flashIntervalTimer = 0f; // 重置计时器
            }
        }
    }

    public void TakeDamage(int damage)
    {
        // 从 GameManager 获取当前生命值并减少
        GameManager.Instance.currentHealth -= damage;
        
        // 确保生命值不会小于0
        if (GameManager.Instance.currentHealth < 0)
        {
            GameManager.Instance.currentHealth = 0;
        }

        // 更新血量显示
        UpdateHealthDisplay();

        // 触发受伤闪烁效果
        // TODO：炮台受伤闪白没效果，需修改
        Debug.Log("TriggerFlash called: " + flashDuration);
        if (spriteRenderer != null)
        {
            isFlashing = true;
            flashTimer = 0f;
            flashIntervalTimer = 0f; // 重置闪烁间隔计时器
        }

        // 检查是否死亡
        if (GameManager.Instance.currentHealth <= 0)
        {
            Die();
        }
    }

    // public void TriggerFlash()
    // {
    //     Debug.Log("TriggerFlash called: " + flashDuration);
    //     if (spriteRenderer != null)
    //     {
    //         spriteRenderer.color = Color.red;
    //         isFlashing = true;
    //         flashTimer = 0f;
    //     }
    // }

    private void Die()
    {
        // 是否补充死亡动画
    }

    public int GetCurrentHealth()
    {
        return GameManager.Instance.currentHealth;
    }

    // 更新血量显示
    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + GameManager.Instance.currentHealth.ToString();
        }
    }
}
