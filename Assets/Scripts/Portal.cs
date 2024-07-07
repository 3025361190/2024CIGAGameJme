/*
文件名: Portal.cs
编辑人: Fortunate瑞
文件描述: 传送门脚本,挂载在传送门上,用于处理传送门的一些触发行为,是个不会物理碰撞的触发器
组件依赖: Collider(isTrigger), Rigidbody2D
*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameObject manager;

    void Start() {
        manager = GameObject.Find("SceneManagerObject");
    }
    // 当有物体进入触发器区域时调用
    void OnTriggerEnter(Collider other)
    {
        // 检查进入触发器的物体是否有 "Turret" 标签
        if (other.CompareTag("Turret"))
        {
            // 调用触发函数
            manager.GetComponent<Manager>().SwitchSceneType();
        }
    }
}
