/*
文件名：BossHealth.cs
编辑人：Fortunate瑞
文件描述：boss血量类,用于管理boss的血量
绑定：绑定死亡动画的预制体
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int bossHealthPoint;     // boss max 血量,由bossSpawner从gamemanager.levelconfig中读取后赋值
    public Slider healthSlider;     // boss血量滑动条,在代码中查找绑定
    public Image healthSliderFill;  // boss血量滑动条填充图片,在代码中查找绑定

    public GameObject spriteRenderer;          // boss图片，用于闪烁效果，代码中绑定
    private Color originalColor;                // boss图片原始颜色
    private float flashDuration;                    // 闪烁持续时间

    public GameObject deadAnimation;               // 死亡动画的预制体，在unity编辑器中拖动赋值
    

    // 运行时
    private int currentHealth;       // 当前血量
    private float flashTimer = 0f;                  // 闪烁计时器
    public bool isFlashing = false;               // 是否闪烁

    private float flashInterval; // 闪烁间隔时间
    private float flashIntervalTimer = 0f; // 闪烁计时器
    private bool isRedColor = false;    // 当前是否为红色


    void Awake()
    {
        if(spriteRenderer == null)
        {
            spriteRenderer = GameObject.Find("body");
            if(spriteRenderer == null)
            {
                Debug.LogError("未找到body物体！");
            }
        }
        // 用名字获取子组件引用
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.GetComponent<SpriteRenderer>().color;
        }else
        {
            Debug.LogError("未找到body物体的SpriteRenderer组件！");
        }
        // 查找并绑定血量滑动条和填充图片
        Transform sliderTransform = transform.Find("Canvas/healthSlider");
        if (sliderTransform != null)
        {
            healthSlider = sliderTransform.GetComponent<Slider>();
            if (healthSlider == null)
            {
                Debug.LogError("未找到 Slider 组件！");
            }
            Transform fillTransform = sliderTransform.Find("Fill Area/healthSliderFill");
            if (fillTransform != null)
            {
                healthSliderFill = fillTransform.GetComponent<Image>();
                if (healthSliderFill == null)
                {
                    Debug.LogError("未找到 Image 组件！");
                }
            }
            else
            {
                Debug.LogError("未找到 Fill Area/healthSliderFill 子物体！");
            }
            
        }
        else
        {
            Debug.LogError("找不到 Canvas/healthSlider 子物体！");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        var bossConfig = GameManager.Instance.data.bossData;
        if (bossConfig != null)
        {   
            try
            {
                flashDuration = bossConfig["flashDuration"].ToObject<float>();
                flashInterval = bossConfig["flashInterval"].ToObject<float>();
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
        
        // Debug.Log("初始化中调用UpdateHealthDisplay");
        // 初始化显示
        UpdateHealthDisplay();
    }

    public void SetBossHealthPoint(int healthPoint)
    {
        // Debug.Log("SetBossHealthPoint被调用，healthPoint: " + healthPoint);
        bossHealthPoint = healthPoint;
        currentHealth = bossHealthPoint;
        UpdateHealthDisplay();
    }


    private void UpdateHealthDisplay()
    {
        // Debug.Log("UpdateHealthDisplay被调用");
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth * 1.0f / bossHealthPoint;
            // 血条颜色随生命值变化而线性变化
            float healthPercentage = currentHealth * 1.0f / bossHealthPoint;
            healthSliderFill.color = Color.Lerp(Color.red, Color.green, healthPercentage);
            // Debug.Log("healthSlider.value: " + healthSlider.value);
        }
        else
        {
            Debug.LogError("未找到血量滑动条组件！");
        }
    }


    // Update is called once per frame
    void Update()
    {
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

     // boss被子弹击中时,处理击中事件,由子弹调用
    public void HandleHit()
    {
        AudioManager.Instance.PlaySFX(0);
        currentHealth -= 1;
        // Debug.Log("boss被击中！，当前血量：" + currentHealth + "/" + bossHealthPoint);
        // 确保生命值不会小于0
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthDisplay();
        if (spriteRenderer != null)
        {
            isFlashing = true;
            flashTimer = 0f;
            flashIntervalTimer = 0f; // 重置闪烁间隔计时器
        }

        // 检查是否死亡
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 播放死亡动画
        if(deadAnimation != null)
        {
            Vector3 currentPosition = transform.position; // 使用 transform.position 获取当前对象的位置
            GameObject newPrefabInstance = Instantiate(deadAnimation, currentPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("未绑定死亡动画预制体！");
        }

        // 获取 BossSpawner 的引用并调用 RemoveBoss 方法
        GameObject spawnerObject = GameObject.Find("BossSpawnerObject");
        if (spawnerObject != null)
        {
            BossSpawner[] bossSpawners = spawnerObject.GetComponents<BossSpawner>();
            foreach (var bossSpawner in bossSpawners)
            {
                // 暴力遍历所有BossSpawner，并调用RemoveBoss方法，在Remove中判断是否包含当前boss
                bossSpawner.RemoveBoss(gameObject);
            }
        }

        Destroy(gameObject); // 销毁当前敌人对象
    }
}