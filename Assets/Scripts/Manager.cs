using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



public class Manager : MonoBehaviour
{
    public GameObject turret;
    private SceneType currentSceneType = SceneType.HongYou;

    public SceneType GetCurrentSceneType(){
        return currentSceneType;
    }

    private void SwitchSceneType(){
        if(currentSceneType == SceneType.QingTang){
            currentSceneType = SceneType.HongYou;
            Recovery();
        }else{
            currentSceneType = SceneType.QingTang;
        }
    }

    private void Recovery(){
        turret.GetComponent<ShootBullet>().RecycleBullet();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
