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
    private bool cdFlag = false;
    private float cdTime = 5.0f;
    private float cdTimer;

    //小途的
    public GameObject background;
    public GameObject effect;
    private Animator beijing1;
    private Animator beijing2;

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
        cdTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(cdFlag){
            cdTimer += Time.deltaTime;
            if(cdTimer >= cdTime){
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
        if(currentSceneType == SceneType.QingTang)
        {
            Debug.Log("switch scene to 红油");
            currentSceneType = SceneType.HongYou;
            Recovery();
            cdFlag = true;
            // 切换美术资源
            // GetComponent<SpriteRenderer>().sprite = sceneResource[0];
            //清汤切红油动画
            beijing1.SetBool("background",true);
            beijing2.SetTrigger("change");

        }
        else if(currentSceneType == SceneType.HongYou)
        {
            if(cdFlag == false){
                Debug.Log("switch scene to 清汤");
                currentSceneType = SceneType.QingTang;
                Recovery();
                cdFlag = true;
                // GetComponent<SpriteRenderer>().sprite = sceneResource[1];
                //红油切清汤动画
                beijing1.SetBool("background", false);
                beijing2.SetTrigger("change");
            }
            else
            {
                Debug.Log("红油to清汤is in cd");
            
            }
        }
        // else if(cdFlag == false && currentSceneType == SceneType.HongYou)
        // {
        //     Debug.Log("switch scene to 清汤");
        //     currentSceneType = SceneType.QingTang;
        //     // GetComponent<SpriteRenderer>().sprite = sceneResource[1];
        //     //红油切清汤动画
        //     beijing1.SetBool("background", false);
        //     beijing2.SetTrigger("change");
        // }
    }

    private void Recovery(){
        Debug.Log("触发回收");
        turret.GetComponent<ShootBullet>().RecycleBullet();
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
