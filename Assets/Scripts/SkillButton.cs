using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SkillButton : MonoBehaviour
{
    public GameObject turret;
    public SceneType currentSceneType;

    // 技能冷却相关
    private bool cdFlag = false;      // 冷却标志
    private float cdTime;      // 冷却时间设置为5秒
    private float cdTimer;            // 冷却计时器
    public GameObject cooldownImage; // 冷却图像
    public GameObject cooldownText; // 冷却文本

    // 清汤持续时间相关
    private float durationTimer;
    private float durationTime; // 持续时间设置为2秒
    private bool isDurationActive = false; // 持续时间标志



    public GameObject background;
    public GameObject effect;
    private Animator beijing1;
    private Animator beijing2;
    public Animator skillbuttion;
  //  public AudioSource audioSource;//音效

    void Awake()
    {
        currentSceneType = SceneType.HongYou;
        // Debug.Log("currentSceneType in manager AWAKE is " + currentSceneType);
        turret =  GameObject.FindGameObjectsWithTag("Turret")[0];
        beijing1 = background.GetComponent<Animator>();
        beijing2 = effect.GetComponent<Animator>();
        cdTimer = 0.0f;              // 初始化计时器
        durationTimer = 0.0f;       // 初始化持续时间计时器
        //var modeConfig = JsonLoader.LoadJsonAsJObject("StaticData/mode_config");
        var modeConfig = GameManager.Instance.data.modeData;
        cdTime = modeConfig["splitModeCD"].ToObject<float>();
        durationTime = modeConfig["splitModeDuration"].ToObject<float>();
        cooldownImage.GetComponent<UnityEngine.UI.Image>().fillAmount = 0f; // 初始化冷却图像填充
        cooldownText.GetComponent<UnityEngine.UI.Text>().text = ""; // 初始化冷却文本
    }



    // Update is called once per frame
    void Update()
    {
        if(cdFlag)
        {
            cdTimer += Time.deltaTime;
            cooldownImage.GetComponent<UnityEngine.UI.Image>().fillAmount = 1 - (cdTimer / cdTime); // 更新冷却图像填充
            // 精确到整数
            cooldownText.GetComponent<UnityEngine.UI.Text>().text = Mathf.CeilToInt(cdTime - cdTimer).ToString(); // 更新冷却文本
            if(cdTimer >= cdTime)
            {
                cdFlag = false;
                cdTimer = 0.0f;
                cooldownImage.GetComponent<UnityEngine.UI.Image>().fillAmount = 0f; // 重置冷却图像填充
                cooldownText.GetComponent<UnityEngine.UI.Text>().text = ""; // 清空冷却文本
            }
        }
        if(isDurationActive)
        {
            durationTimer += Time.deltaTime;
            if(durationTimer >= durationTime)
            {
                isDurationActive = false;
                durationTimer = 0.0f;
                SwitchSceneType();
            }
        }
    }

    public SceneType GetCurrentSceneType(){
        Debug.Log("currentSceneType in manager GET is " + currentSceneType);
        return currentSceneType;
    }


    public void SwitchSceneType(){
        Debug.Log("switch be called");
        
        // 如果在冷却中，则不允许切换
        if(cdFlag)
        {
            Debug.Log($"场景切换正在冷却中，剩余时间：{cdTime - cdTimer:F1}秒");
            return;
        }

        if(currentSceneType == SceneType.QingTang)
        {
            Debug.Log("switch scene to 红油");
            currentSceneType = SceneType.HongYou;
            Recovery();
            isDurationActive = false;
            durationTimer = 0.0f;
            cdFlag = true;            // 设置冷却标志
            cooldownImage.GetComponent<UnityEngine.UI.Image>().fillAmount = 1f; // 重置冷却图像填充
            cooldownText.GetComponent<UnityEngine.UI.Text>().text = Mathf.CeilToInt(cdTime).ToString(); // 更新冷却文本

            // 切换动画和音效
            beijing1.SetBool("background",true);
            beijing2.SetTrigger("change");
            
            skillbuttion.SetBool("change", false);
            AudioManager.Instance.PlaySFX(1);
         //   audioSource.Play();
        }
        else if(currentSceneType == SceneType.HongYou)
        {
            Debug.Log("switch scene to 清汤");
            currentSceneType = SceneType.QingTang;
            isDurationActive = true;
            
            //cdFlag = true;            // 设置冷却标志
            
            // 切换动画和音效
            beijing1.SetBool("background", false);
            beijing2.SetTrigger("change");
            skillbuttion.SetBool("change", true);
            AudioManager.Instance.PlaySFX(1);
            //  audioSource.Play();
        }
    }

    // 获取剩余冷却时间
    public float GetCooldownRemaining()
    {
        if (cdFlag)
        {
            return cdTime - cdTimer;
        }
        return 0f;
    }

    private void Recovery(){
        Debug.Log("触发回收");
        turret.GetComponent<PlayerController>().RecycleBullet();
    }

    
}


