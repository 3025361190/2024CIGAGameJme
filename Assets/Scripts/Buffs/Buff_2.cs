using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
文件名：Buff_2.cs
编辑人：没道理啊
文件描述：不要靠近buff
*/


public class Buff_2 : BaseBuff
{
    private float value;
    private float originalSpeed;

    public override void Init()
    {
        // 初始化buff参数
        buffId = 2;
        var buffConfig = GameManager.Instance.data.buffData[buffId];
        buffName = buffConfig["buffName"].ToString();
        buffDescription = buffConfig["buffDescription"].ToString();
        buffIcon = buffConfig["buffIcon"].ToString();
        buffDuration = buffConfig["buffDuration"].ToObject<float>();
        buffStackable = buffConfig["buffStackable"].ToObject<bool>();
        stackType = buffConfig["stackType"].ToObject<int>();
        isNormalBuff = buffConfig["isNormalBuff"].ToObject<bool>();
        value = buffConfig["value"].ToObject<float>();
        originalSpeed = GameManager.Instance.data.enemyData["moveSpeed"].ToObject<float>();
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
            GameManager.Instance.data.enemyData["moveSpeed"] = GameManager.Instance.data.enemyData["moveSpeed"].ToObject<float>() + value;
        }
        else if(stackType == 1)
        {
            GameManager.Instance.data.enemyData["moveSpeed"] = GameManager.Instance.data.enemyData["moveSpeed"].ToObject<float>() * value;
        }
    }

    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        GameManager.Instance.data.enemyData["moveSpeed"] = originalSpeed;
    }    
}
