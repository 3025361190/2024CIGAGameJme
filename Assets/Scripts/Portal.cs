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
    private bool canTrigger = true;  // 是否可以触发的标志位
    private float cooldownTime = 2.0f;  // 冷却时间

    void Start() {
        manager = GameObject.Find("SceneManagerObject");
    }
    // 当有物体进入触发器区域时调用
    void OnTriggerEnter2D(Collider2D other)
    {
        if (canTrigger)
        {
            // 检查进入触发器的物体是否有 "Turret" 标签
            if (other.gameObject.CompareTag("Turret"))
            {
                // 调用触发函数
                manager.GetComponent<Manager>().SwitchSceneType();
                StartCoroutine(Cooldown());  // 开始冷却协程
            }
        }
    }

    private IEnumerator Cooldown()
    {
        canTrigger = false;  // 设置不能触发
        yield return new WaitForSeconds(cooldownTime);  // 等待冷却时间
        canTrigger = true;  // 冷却完毕，可以再次触发
    }
}
