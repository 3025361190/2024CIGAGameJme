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
    private float value;
    private float value_win;
    private float value_lose;
    private int originalcurrentBulletCount;

    public override void Init()
    {
        // 初始化buff参数
        buffId = 16;
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
        value_win = buffConfig["value_win"].ToObject<float>();
        value_lose = buffConfig["value_lose"].ToObject<float>();
        // 初始化运行时参数
        currentStack = 0;
        isActive = false;
        target = null;
    }

    public override void ActivateBuff()
    {
        // 保留父类逻辑
        base.ActivateBuff();
        originalcurrentBulletCount = GameManager.Instance.currentBulletCount;
        bool isWin = Random.Range(0, 100) < value * 100;
        if(stackType == 0)
        {
            if(isWin)
            {
                GameManager.Instance.currentBulletCount = Mathf.RoundToInt(GameManager.Instance.currentBulletCount + value_win);
            }
            else
            {
                GameManager.Instance.currentBulletCount = Mathf.RoundToInt(GameManager.Instance.currentBulletCount + value_lose);
            }
        }
        else if(stackType == 1)
        {
            if(isWin)
            {
                GameManager.Instance.currentBulletCount = Mathf.RoundToInt(GameManager.Instance.currentBulletCount * value_win);
            }
            else
            {
                GameManager.Instance.currentBulletCount = Mathf.RoundToInt(GameManager.Instance.currentBulletCount * value_lose);
            }
        }
        // Debug.Log("结果为：" + isWin + "，子弹变化为：" + originalcurrentBulletCount + "->" + GameManager.Instance.currentBulletCount);
    }

    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        GameManager.Instance.currentBulletCount = originalcurrentBulletCount;
    }
}
