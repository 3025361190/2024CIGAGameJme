/*
文件名：Buff_2.cs
编辑人：没道理啊
文件描述：不要靠近buff
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;



public class Buff_2 : BaseBuff
{
    private float value;
    private float originalSpeed;
    // private float originalBossSpeed;

    public override void Init()
    {
        // 初始化buff参数
        buffId = 2;
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
        // originalBossSpeed = GameManager.Instance.data.bossData["moveSpeed"].ToObject<float>();
        // 初始化运行时参数
        currentStack = 0;
        isActive = false;
        target = null;
    }

    public override void ActivateBuff()
    {
        // 保留父类逻辑
        base.ActivateBuff();
        originalSpeed = GameManager.Instance.data.enemyData["moveSpeed"].ToObject<float>();
        if(stackType == 0)
        {
            GameManager.Instance.data.enemyData["moveSpeed"] = GameManager.Instance.data.enemyData["moveSpeed"].ToObject<float>() + value;
            // GameManager.Instance.data.bossData["moveSpeed"] = GameManager.Instance.data.bossData["moveSpeed"].ToObject<float>() + value;
        }
        else if(stackType == 1)
        {
            GameManager.Instance.data.enemyData["moveSpeed"] = GameManager.Instance.data.enemyData["moveSpeed"].ToObject<float>() * value;
            // GameManager.Instance.data.bossData["moveSpeed"] = GameManager.Instance.data.bossData["moveSpeed"].ToObject<float>() * value;
        }
    }

    // 取消buff所有层数，恢复原始速度
    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        GameManager.Instance.data.enemyData["moveSpeed"] = originalSpeed;
        // GameManager.Instance.data.bossData["moveSpeed"] = originalBossSpeed;
    }    
}
