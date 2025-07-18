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
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    
    
    private float flashDuration;                    // 闪烁持续时间


    public Slider healthSlider;              // 玩家血量滑动条，代码在场景中查找绑定
    public Image healthSliderFill; // 玩家血量滑动条填充图片，代码在场景中查找绑定
    public TextMeshProUGUI healthText;              // 玩家血量文本，代码在场景中查找绑定
    public GameObject spriteRenderer;          // 玩家图片，用于闪烁效果，代码中绑定
    private Color originalColor;                    // 玩家图片原始颜色

    private float flashTimer = 0f;                  // 闪烁计时器
    public bool isFlashing = false;               // 是否闪烁

    private float flashInterval; // 闪烁间隔时间
    private float flashIntervalTimer = 0f; // 闪烁计时器
    private bool isRedColor = false;    // 当前是否为红色
    private float bloodRaturnValue;
    private float bloodRaturnTimer = 0f; // 血量回复计时器
    private float bloodRaturnInterval; // 血量回复间隔时间


    private void Awake()
    {
        // 从global_config.json中读取闪烁持续时间
        //var globalConfig = JsonLoader.LoadJsonAsJObject("StaticData/global_config");
        var globalConfig = GameManager.Instance.data.globalData;
        if (globalConfig != null)
        {   
            try
            {
                flashDuration = globalConfig["flashDuration"].ToObject<float>();
                flashInterval = globalConfig["flashInterval"].ToObject<float>();
                bloodRaturnValue = globalConfig["bloodRaturnValue"].ToObject<float>();
                bloodRaturnInterval = 60.0f / bloodRaturnValue;
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
        if(spriteRenderer == null)
        {
            spriteRenderer = GameObject.Find("playerimg");
            if(spriteRenderer == null)
            {
                Debug.LogError("未找到playerimg物体！");
            }
        }

        // 用名字获取子组件引用
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.GetComponent<SpriteRenderer>().color;
        }else
        {
            Debug.LogError("未找到playerimg物体的SpriteRenderer组件！");
        }

        // // 查找并绑定血量文本
        // if (healthText == null)
        // {
        //     GameObject healthObj = GameObject.Find("health");
        //     if (healthObj != null)
        //     {
        //         healthText = healthObj.GetComponent<TextMeshProUGUI>();
        //     }
        //     else
        //     {
        //         Debug.LogError("未找到health物体！");
        //     }
        //     if (healthText == null)
        //     {
        //         Debug.LogError("未找到血量文本组件！");
        //     }
        // }

        // 查找并绑定血量滑动条
        if (healthSlider == null)
        {
            GameObject sliderObj = GameObject.Find("healthSlider");
            if (sliderObj != null)
            {
                healthSlider = sliderObj.GetComponent<Slider>();
            }
            else
            {
                Debug.LogError("未找到HealthSlider物体！");
            }
            if (healthSlider == null)
            {
                Debug.LogError("未找到血量滑动条组件！");
            }
        }

        // 查找并绑定血量滑动条填充图片
        if (healthSliderFill == null)
        {
            GameObject fillObj = GameObject.Find("healthSliderFill");
            if (fillObj != null)
            {
                healthSliderFill = fillObj.GetComponent<Image>();
            }
            else
            {
                Debug.LogError("未找到HealthSliderFill物体！");
            }
            if (healthSliderFill == null)
            {
                Debug.LogError("未找到血量滑动条填充图片组件！");
            }
        }

        // 初始化显示
        UpdateHealthDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        // 回复血量
        if (GameManager.Instance.currentHealth < GameManager.Instance.initialHealth)
        {
            bloodRaturnTimer += Time.deltaTime;
            if (bloodRaturnTimer >= bloodRaturnInterval)
            {
                GameManager.Instance.currentHealth += 1; // 每间隔回复1点血量
                if (GameManager.Instance.currentHealth > GameManager.Instance.initialHealth)
                {
                    GameManager.Instance.currentHealth = GameManager.Instance.initialHealth; // 确保不超过初始值
                }
                UpdateHealthDisplay();
                bloodRaturnTimer = 0f; // 重置计时器
                // Debug.Log("回复血量：" + GameManager.Instance.currentHealth);
            }
        }
        // 处理受伤闪烁效果
        if (isFlashing)
        {
            // Debug.Log("Flashing: " + flashTimer);
            flashTimer += Time.deltaTime;
            flashIntervalTimer += Time.deltaTime;

            // 每过一个间隔时间就切换一次颜色
            if (flashIntervalTimer >= flashInterval)
            {
                isRedColor = !isRedColor; // 切换颜色状态
                spriteRenderer.GetComponent<SpriteRenderer>().color = isRedColor? new Color(originalColor.r, 0.58f, 0.77f, 0.5f)  // 透明度50%
    : originalColor;  // 恢复原始颜色（包括原始透明度）
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
        // Debug.Log("TriggerFlash called: " + flashDuration);
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


        if (healthSlider != null)
        {
            healthSlider.value = GameManager.Instance.currentHealth * 1.0f / GameManager.Instance.initialHealth;
            // 血条颜色随生命值变化而线性变化
            float healthPercentage = GameManager.Instance.currentHealth * 1.0f / GameManager.Instance.initialHealth;
           // healthSliderFill.color = Color.Lerp(Color.red, Color.green, healthPercentage);
        }
        else
        {
            Debug.LogError("未找到血量滑动条组件！");
        }
    }
}
