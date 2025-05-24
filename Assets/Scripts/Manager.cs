/*
文件名：Manager.cs
编辑人：豪哥，fortunate瑞，没道理啊
文件描述：Manager类，用于管理游戏的场景切换，ui界面，游戏流程等
*/

using System.Collections;
using System.Collections.Generic;
// using System.Diagnostics;
using System.Threading;
using UnityEngine;



public class Manager : MonoBehaviour
{
    public GameObject turret;
    public SceneType currentSceneType;
    private bool cdFlag = false;      // 冷却标志
    private float cdTime = 5.0f;      // 冷却时间设置为5秒
    private float cdTimer;            // 冷却计时器

    //小途的
    public GameObject background;
    public GameObject effect;
    private Animator beijing1;
    private Animator beijing2;
    public AudioSource audioSource;//音效
    public GameObject plableNew;
    // [Header("是否开启新手教学")]
    // public bool isNew = true;

    // public Sprite[] sceneResource;      // 在unity中拖拽设置场景资源
    // Start is called before the first frame update
    void Awake()
    {
        currentSceneType = SceneType.QingTang;
        Debug.Log("currentSceneType in manager AWAKE is " + currentSceneType);
        // GetComponent<SpriteRenderer>().sprite = sceneResource[1];
        // currentSceneType = SceneType.HongYou;
        // GetComponent<SpriteRenderer>().sprite = sceneResource[0];
        turret =  GameObject.FindGameObjectsWithTag("Turret")[0];
        beijing1 = background.GetComponent<Animator>();
        beijing2 = effect.GetComponent<Animator>();
        cdTimer = 0.0f;              // 初始化计时器
        // if (!isNew)
        // {
        //     plableNew.SetActive(false);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        // 处理冷却时间
        if(cdFlag)
        {
            cdTimer += Time.deltaTime;
            if(cdTimer >= cdTime)
            {
                cdFlag = false;
                cdTimer = 0.0f;
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
            cdFlag = true;            // 设置冷却标志
            
            // 切换动画和音效
            beijing1.SetBool("background",true);
            beijing2.SetTrigger("change");
            audioSource.Play();
        }
        else if(currentSceneType == SceneType.HongYou)
        {
            Debug.Log("switch scene to 清汤");
            currentSceneType = SceneType.QingTang;
            cdFlag = true;            // 设置冷却标志
            
            // 切换动画和音效
            beijing1.SetBool("background", false);
            beijing2.SetTrigger("change");
            audioSource.Play();
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

    // 游戏暂停
    public void PauseGame(){
        Time.timeScale = 0;
    }

    // 游戏继续
    public void ResumeGame(){
        Time.timeScale = 1;
    }

    // 返回主菜单
    public void BackToMenu(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    // 开始游戏
    public void StartGame(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    // 重新开始游戏
    public void RestartGame(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    
}
