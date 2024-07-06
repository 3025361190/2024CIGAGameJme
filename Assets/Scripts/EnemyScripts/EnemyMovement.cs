/*
文件名：EnemyMovement.cs
编辑人：Fortunate瑞
文件描述：敌人移动脚本,挂载在敌人预制体上,用于控制敌人的移动
组件依赖：Rigidbody2D, CircleCollider2D
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
  
    
    void Start()
    {
        
    }

    // 碰撞检测,当敌人碰撞到home时触发,停止前进
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("home"))
        {
            
        }
    }

    void Update()
    {

    }


}
