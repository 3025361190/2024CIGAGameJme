using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuff : MonoBehaviour
{

    [Header("Buff基础属性")]
    [SerializeField] protected int buffId;              // Buff唯一标识符
    [SerializeField] protected string buffName;         // Buff名称
    [SerializeField, TextArea] protected string buffDescription;  // Buff描述文本
    [SerializeField] protected float buffDuration = -1; // Buff持续时间（-1表示永久）
    [SerializeField] protected bool buffStackable;      // 是否可叠加
    [SerializeField] protected int maxStack = 1;        // 最大叠加层数
    [SerializeField] protected Sprite buffIcon;         // Buff图标
    [SerializeField] protected int stackType;           // 叠加方式（0:加法叠加，1:乘法叠加）


    // 运行时属性
    [SerializeField] protected float remainingTime;    // 剩余持续时间
    [SerializeField] protected int currentStack = 1;   // 当前叠加层数
    [SerializeField] protected bool isActive = true;   // Buff是否激活


    // 生命周期方法
    protected virtual void OnEnable()
    {
        remainingTime = buffDuration;
        OnBuffApplied();
    }

    protected virtual void OnDisable()
    {
        OnBuffRemoved();
    }

    protected virtual void Update()
    {
        if (!isActive) return;

        // 更新持续时间
        if (buffDuration > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                RemoveBuff();
            }
        }
    }


    // Buff操作方法

    /// <summary>
    /// 通过json初始化Buff属性,在子类中应该要override这个方法,把buff的专属属性值也进行初始化
    /// </summary>
    public virtual void InitBuffFromConfig(int buffId)
    {
        // // 获取JSON文本
        string jsonText = JsonLoader.LoadJsonText("buffs_config");
        // if (string.IsNullOrEmpty(jsonText))
        // {
        //     Debug.LogError("无法加载buff配置文件");
        //     return;
        // }

        // // 解析JSON文本
        // var buffData = JsonUtility.FromJson<BuffConfigWrapper>(jsonText);
        // if (buffData != null && buffData.buffs != null)
        // {
        //     // 查找对应ID的buff
        //     var targetBuff = System.Array.Find(buffData.buffs, b => b.buffId == buffId);
        //     if (targetBuff != null)
        //     {
        //         // 设置基础属性
        //         this.buffId = targetBuff.buffId;
        //         this.buffName = targetBuff.buffName;
        //         this.buffDescription = targetBuff.buffDescription;
        //         this.buffDuration = targetBuff.buffDuration;
        //         this.buffStackable = targetBuff.buffStackable;
        //         this.maxStack = targetBuff.maxStack;
        //         this.stackType = targetBuff.stackType;
                
        //         // 设置运行时属性
        //         this.remainingTime = this.buffDuration;
        //         this.currentStack = 1;
        //         this.isActive = true;
        //     }
        //     else
        //     {
        //         Debug.LogError($"找不到ID为{buffId}的buff配置");
        //     }
        // }
    }

    /// <summary>
    /// 移除Buff效果
    /// </summary>
    public virtual void RemoveBuff()
    {
        if (!isActive) return;
        
        isActive = false;
        // 是否需要销毁？还是用对象池的方式管理buff？
        // Destroy(this);
    }

    /// <summary>
    /// 重置Buff持续时间
    /// </summary>
    public virtual void RefreshDuration()
    {
        if (buffDuration > 0)
        {
            remainingTime = buffDuration;
        }
    }

    /// <summary>
    /// 尝试叠加Buff
    /// </summary>
    /// <returns>是否叠加成功</returns>
    public virtual bool TryStack()
    {
        if (!buffStackable || currentStack >= maxStack) return false;

        currentStack++;
        OnBuffStacked();
        return true;
    }


    // 虚方法 - 由子类实现具体效果

    /// <summary>
    /// Buff被应用时调用
    /// </summary>
    protected abstract void OnBuffApplied();

    /// <summary>
    /// Buff被移除时调用
    /// </summary>
    protected abstract void OnBuffRemoved();

    /// <summary>
    /// Buff叠加时调用
    /// </summary>
    protected virtual void OnBuffStacked()
    {
        // 默认实现为空，子类可以根据需要重写
    }

    // 属性访问器（即getter）
    /*
    等同于：
    public int BuffId
    {
        get { return buffId; }
    }
    */
    public int BuffId => buffId;
    public string BuffName => buffName;
    public string BuffDescription => buffDescription;
    public float BuffDuration => buffDuration;
    public bool BuffStackable => buffStackable;
    public int MaxStack => maxStack;
    public Sprite BuffIcon => buffIcon;
    public int StackType => stackType;
    public float RemainingTime => remainingTime;
    public int CurrentStack => currentStack;
    public bool IsActive => isActive;
}


// TODO: buff的细节还需定夺
// 如：
// 1.buff直接加在生效的物体上，还是加在buff管理器上？
// 2.buff的管理方式，是否销毁？还是用对象池的方式管理？

