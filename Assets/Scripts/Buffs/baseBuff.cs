using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuff : MonoBehaviour
{
    #region 基础属性
    [Header("Buff基础属性")]
    [SerializeField] protected int buffId;              // Buff唯一标识符
    [SerializeField] protected string buffName;         // Buff名称
    [SerializeField, TextArea] protected string buffDescription;  // Buff描述文本
    [SerializeField] protected float buffDuration = -1; // Buff持续时间（-1表示永久）
    [SerializeField] protected bool buffStackable;      // 是否可叠加
    [SerializeField] protected int maxStack = 1;        // 最大叠加层数
    [SerializeField] protected Sprite buffIcon;         // Buff图标
    [SerializeField] protected int stackType;           // 叠加方式（0:加法叠加，1:乘法叠加）
    #endregion

    #region 运行时属性
    protected float remainingTime;    // 剩余持续时间
    protected int currentStack = 1;   // 当前叠加层数
    protected bool isActive = true;   // Buff是否激活
    #endregion

    #region 生命周期方法
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
    #endregion

    #region Buff操作方法
    /// <summary>
    /// 初始化Buff属性
    /// </summary>
    public virtual void InitializeBuff(int id, string name, string description, float duration = -1, 
                                     bool stackable = false, int maxStackCount = 1, 
                                     Sprite icon = null, int stack = 0)
    {
        buffId = id;
        buffName = name;
        buffDescription = description;
        buffDuration = duration;
        buffStackable = stackable;
        maxStack = maxStackCount;
        buffIcon = icon;
        stackType = stack;
        
        remainingTime = buffDuration;
    }

    /// <summary>
    /// 移除Buff效果
    /// </summary>
    public virtual void RemoveBuff()
    {
        if (!isActive) return;
        
        isActive = false;
        Destroy(this);
    }

    /// <summary>
    /// 刷新Buff持续时间
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
    #endregion

    #region 虚方法 - 由子类实现具体效果
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
    #endregion

    #region 属性访问器
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
    #endregion
}


// TODO: ai生成的文件，还没细看