/*
文件名：Buff_16.cs
编辑人：没道理啊
文件描述：碰碰运气buff
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;


public class Buff_16 : BaseBuff
{
    private int value;
    private int originalcurrentBulletCount;

    public override void Init()
    {
        // 初始化buff参数
        buffId = 16;
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
        int randomValue = Random.Range(1, 101);
        if(randomValue <= 70)
        {
            GameManager.Instance.currentBulletCount = GameManager.Instance.currentBulletCount * 3;
        }else
        {
            GameManager.Instance.currentBulletCount = GameManager.Instance.currentBulletCount / 2;
        }
    }

    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        GameManager.Instance.currentBulletCount = originalcurrentBulletCount;
    }
}
