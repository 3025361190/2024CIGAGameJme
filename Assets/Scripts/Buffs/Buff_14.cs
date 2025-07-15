/*
文件名：Buff_14.cs
编辑人：没道理啊
文件描述：我要变小buff
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;

public class Buff_14 : BaseBuff
{
    private float value;
    private GameObject player;
    private Vector3 originalScale;
    private float originalColliderRadius;

    public override void Init()
    {
        // 初始化buff参数
        buffId = 14;
        // 在buffs数组中查找对应buffId的配置
        var buffsArray = GameManager.Instance.data.buffData["buffs"] as JArray;
        var buffConfig = buffsArray.FirstOrDefault(b => b["buffId"].ToObject<int>() == buffId);
        buffName = buffConfig["buffName"].ToString();
        buffDescription = buffConfig["buffDescription"].ToString();
        buffIcon = buffConfig["buffIcon"].ToString();
        buffDuration = buffConfig["buffDuration"].ToObject<float>();
        buffStackable = buffConfig["buffStackable"].ToObject<bool>();
        stackType = buffConfig["stackType"].ToObject<int>();
        isNormalBuff = buffConfig["isNormalBuff"].ToObject<bool>();
        value = buffConfig["value"].ToObject<float>();
        player = GameObject.FindWithTag("Player");
        originalScale = player.transform.localScale; // 假设玩家的缩放是均匀的
        originalColliderRadius = player.GetComponent<CircleCollider2D>().radius; // 获取原始碰撞器半径
        // 初始化运行时参数
        currentStack = 0;
        isActive = false;
        target = null;
    }

    public override void ActivateBuff()
    {
        // 保留父类逻辑
        base.ActivateBuff();
        player.transform.localScale *= 0.5f;
        player.GetComponent<CircleCollider2D>().radius *= 0.5f;
    }

    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        player.transform.localScale = originalScale; // 恢复原始缩放比例
        player.GetComponent<CircleCollider2D>().radius = originalColliderRadius; // 恢复原始碰撞器半径
    }    
}
