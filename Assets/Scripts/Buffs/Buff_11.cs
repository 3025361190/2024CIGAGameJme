/*
文件名：Buff_11.cs
编辑人：没道理啊
文件描述：多多子弹buff
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;
public class Buff_11 : BaseBuff
{
    private int value;
    private int originalinitialBulletCount;
    private int originalcurrentBulletCount;

    public override void Init()
    {
        // 初始化buff参数
        buffId = 11;
        // 在buffs数组中查找对应buffId的配置
        var buffsArray = GameManager.Instance.data.globalData["initialBulletCount"] as JArray;
        var buffConfig = buffsArray.FirstOrDefault(b => b["buffId"].ToObject<int>() == buffId);
        buffName = buffConfig["buffName"].ToString();
        buffDescription = buffConfig["buffDescription"].ToString();
        buffIcon = buffConfig["buffIcon"].ToString();
        buffDuration = buffConfig["buffDuration"].ToObject<float>();
        buffStackable = buffConfig["buffStackable"].ToObject<bool>();
        stackType = buffConfig["stackType"].ToObject<int>();
        isNormalBuff = buffConfig["isNormalBuff"].ToObject<bool>();
        value = buffConfig["value"].ToObject<int>();
        originalinitialBulletCount = GameManager.Instance.data.globalData["initialBulletCount"].ToObject<int>();
        originalcurrentBulletCount = GameManager.Instance.currentBulletCount;
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
            GameManager.Instance.data.globalData["initialBulletCount"] = GameManager.Instance.data.globalData["initialBulletCount"].ToObject<float>() + value;
            GameManager.Instance.currentBulletCount = GameManager.Instance.currentBulletCount + value;
        }
        else if(stackType == 1)
        {
            GameManager.Instance.data.globalData["initialBulletCount"] = GameManager.Instance.data.globalData["initialBulletCount"].ToObject<float>() * value;
            GameManager.Instance.currentBulletCount = GameManager.Instance.currentBulletCount * value;
        }
    }

    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        GameManager.Instance.data.globalData["initialBulletCount"] = originalinitialBulletCount;
        GameManager.Instance.currentBulletCount = originalcurrentBulletCount;
    }    
}
