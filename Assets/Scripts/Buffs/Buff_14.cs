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
    private float originalScaleX;
    private float originalScaleY;
    private float originalScaleZ;
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
        originalScaleX = GameManager.Instance.data.globalData[""].ToObject<float>(); // 获取原始缩放比例X
        originalScaleY = GameManager.Instance.data.globalData["playerScaleY"].ToObject<float>(); // 获取原始缩放比例Y
        originalScaleZ = GameManager.Instance.data.globalData["playerScaleZ"].ToObject<float>(); // 获取原始缩放比例Z
        originalColliderRadius = GameManager.Instance.data.globalData["playerColliderRadius"].ToObject<float>(); // 获取原始碰撞器半径
        // 初始化运行时参数
        currentStack = 0;
        isActive = false;
        target = null;
    }

    public override void ActivateBuff()
    {
        // 保留父类逻辑
        base.ActivateBuff();
        GameManager.Instance.data.globalData["playerScaleX"] = GameManager.Instance.data.globalData["playerScaleX"].ToObject<float>() * 0.5f; // 缩放比例X
        GameManager.Instance.data.globalData["playerScaleY"] = GameManager.Instance.data.globalData["playerScaleY"].ToObject<float>() * 0.5f; // 缩放比例Y
        GameManager.Instance.data.globalData["playerScaleZ"] = GameManager.Instance.data.globalData["playerScaleZ"].ToObject<float>() * 0.5f; // 缩放比例Z
        GameManager.Instance.data.globalData["playerColliderRadius"] = GameManager.Instance.data.globalData["playerColliderRadius"].ToObject<float>() * 0.5f; // 缩放碰撞器半径
    }

    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        GameManager.Instance.data.globalData["playerScaleX"] = originalScaleX; // 恢复原始缩放比例X
        GameManager.Instance.data.globalData["playerScaleY"] = originalScaleY; // 恢复原始缩放比例Y
        GameManager.Instance.data.globalData["playerScaleZ"] = originalScaleZ; // 恢复原始缩放比例Z
        GameManager.Instance.data.globalData["playerColliderRadius"] = originalColliderRadius; // 恢复原始碰撞器半径
    }    
}
