/*
文件名：Buff_1.cs
编辑人：fortunate瑞
文件描述：强力射速buff
*/

using UnityEngine;

public class Buff_1 : BaseBuff
{

    private float value;
    private float originalFiringRate;
    private float originalRageFiringRate;

    public override void Init()
    {
        // 初始化buff参数
        buffId = 1;
        var buffConfig = GameManager.Instance.data.buffData[buffId];
        buffName = buffConfig["buffName"].ToString();
        buffDescription = buffConfig["buffDescription"].ToString();
        buffIcon = buffConfig["buffIcon"].ToString();
        buffDuration = buffConfig["buffDuration"].ToObject<float>();
        buffStackable = buffConfig["buffStackable"].ToObject<bool>();
        stackType = buffConfig["stackType"].ToObject<int>();
        isNormalBuff = buffConfig["isNormalBuff"].ToObject<bool>();
        value = buffConfig["value"].ToObject<float>();
        originalFiringRate = GameManager.Instance.data.bulletData["firingRate"].ToObject<float>();
        originalRageFiringRate = GameManager.Instance.data.rageData["rageFiringRate"].ToObject<float>();
        // 初始化运行时参数
        currentStack = 0;
        isActive = false;
        target = null;
    }

    // public override void UpdateBuff()
    // {
    //     // nothing
    // }

    public override void ActivateBuff()
    {
        // 保留父类逻辑
        base.ActivateBuff();
        // 增加基础和狂暴子弹发射速度
        if(stackType == 0)
        {
            GameManager.Instance.data.bulletData["firingRate"] = GameManager.Instance.data.bulletData["firingRate"].ToObject<float>() + value;
            GameManager.Instance.data.rageData["rageFiringRate"] = GameManager.Instance.data.rageData["rageFiringRate"].ToObject<float>() + value;
        }
        else if(stackType == 1)
        {
            GameManager.Instance.data.bulletData["firingRate"] = GameManager.Instance.data.bulletData["firingRate"].ToObject<float>() * value;
            GameManager.Instance.data.rageData["rageFiringRate"] = GameManager.Instance.data.rageData["rageFiringRate"].ToObject<float>() * value;
        }
    }

    public override void DeactivateBuff()
    {
        base.DeactivateBuff();
        GameManager.Instance.data.bulletData["firingRate"] = originalFiringRate;
        GameManager.Instance.data.rageData["rageFiringRate"] = originalRageFiringRate;
    }    
}