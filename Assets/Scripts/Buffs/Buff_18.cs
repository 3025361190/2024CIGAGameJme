/*
文件名：Buff_18.cs
编辑人：fortunate瑞
文件描述：克制敌人buff
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;



public class Buff_18 : BaseBuff
{
    private float value;
    // private float originalBossSpeed;

    public override void Init()
    {
        // 初始化buff参数
        buffId = 18;
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
        // 初始化运行时参数
        currentStack = 0;
        isActive = false;
        target = null;
    }

    public override void ActivateBuff()
    {
        // 保留父类逻辑
        base.ActivateBuff();
        // originalBossSpeed = GameManager.Instance.data.bossData["moveSpeed"].ToObject<float>();
        if(stackType == 0)
        {
            // GameManager.Instance.data.bossData["moveSpeed"] = GameManager.Instance.data.bossData["moveSpeed"].ToObject<float>() + value;
            Debug.LogError("Buff_18不允许加法叠加，请修改配置文件");
        }
        else if(stackType == 1)
        {
            GameManager.Instance.data.bossData["moveSpeed"] = GameManager.Instance.data.bossData["moveSpeed"].ToObject<float>() * value;
        }
    }

    // 取消buff所有层数，恢复原始速度
    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        GameManager.Instance.data.bossData["moveSpeed"] = 1.0f;
    }    
}
