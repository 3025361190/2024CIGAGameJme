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
    private float cdTimer = 0.0f;

    // public Sprite[] sceneResource;
    // Start is called before the first frame update
    void Start()
    {
        currentSceneType = SceneType.QingTang;
        // GetComponent<SpriteRenderer>().sprite = sceneResource[1];
        // currentSceneType = SceneType.HongYou;
        // GetComponent<SpriteRenderer>().sprite = sceneResource[0];
        turret =  GameObject.FindGameObjectsWithTag("Turret")[0];
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
        Debug.Log("currentSceneType in manager is " + currentSceneType);
        return currentSceneType;
    }

    public void SwitchSceneType(){
        if(currentSceneType == SceneType.QingTang){
            currentSceneType = SceneType.HongYou;
            Recovery();
            cdFlag = true;
            // 切换美术资源
            // GetComponent<SpriteRenderer>().sprite = sceneResource[0];
        }
        else if(cdFlag == false && currentSceneType == SceneType.HongYou)
        {
            currentSceneType = SceneType.QingTang;
            // GetComponent<SpriteRenderer>().sprite = sceneResource[1];
        }
    }

    private void Recovery(){
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
