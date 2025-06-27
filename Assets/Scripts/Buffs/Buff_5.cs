/*
文件名：Buff_5.cs
编辑人：没道理啊
文件描述：蹦蹦炸弹buff
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;
public class Buff_5 : BaseBuff
{
    private float value;
    private float originalchainExplosionRange;

    public override void Init()
    {
        // 初始化buff参数
        buffId = 5;
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
        originalchainExplosionRange = GameManager.Instance.data.enemyData["chainExplosionRange"].ToObject<float>();
        // 初始化运行时参数
        currentStack = 0;
        isActive = false;
        target = null;
    }

    public override void ActivateBuff()
    {
        // 保留父类逻辑
        base.ActivateBuff();
        if(stackType == 0)
        {
            GameManager.Instance.data.enemyData["chainExplosionRange"] = GameManager.Instance.data.enemyData["chainExplosionRange"].ToObject<float>() + value;
        }
        else if(stackType == 1)
        {
            GameManager.Instance.data.enemyData["chainExplosionRange"] = GameManager.Instance.data.enemyData["chainExplosionRange"].ToObject<float>() * value;
        }
    }

    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        GameManager.Instance.data.enemyData["chainExplosionRange"] = originalchainExplosionRange;
    }    
}
