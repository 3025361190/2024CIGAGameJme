/*
文件名：BaseBuff.cs
编辑人：fortunate瑞
文件描述：buff基类
绑定：在BuffManager上绑定所有已实现的buff子类
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuff : MonoBehaviour
{
    // 基础属性
    public int buffId;
    public string buffName;
    public string buffDescription;
    public string buffIcon;
    public float buffDuration;
    public bool buffStackable;
    public int stackType;
    public bool isNormalBuff;


    // 运行时属性

    // 当前buff的层数
    protected int currentStack = 0;
    
    // buff是否激活
    protected bool isActive = false;
    
    // 目标对象
    protected GameObject target;
    
    // 初始化buff，子类必须实现
    public abstract void Init();

    // 更新buff效果（buff激活后每帧调用，子类可以重写）
    public virtual void UpdateBuff()
    {
        // nothing
    }
    
    // 激活buff
    public virtual void ActivateBuff()
    {
        if (isActive)
        {
            // 如果buff可叠加，则增加层数
            if (buffStackable)
            {
                currentStack++;
            }
            // 如果buff不可叠加，则直接激活
            else
            {
                Debug.LogError($"buff {buffId} 不可叠加，无法激活");
            }
        }
        else
        {
            currentStack = 1;
            isActive = true;
        }
        Debug.Log($"激活buff：{buffId}：{buffName}，当前层数：{currentStack}");
    }
    
    // 停用buff
    public virtual void DeactivateBuff()
    {
        isActive = false;
        currentStack = 0;
    }
    
     // 检查buff是否激活
    public virtual bool IsActive()
    {
        return isActive;
    }

    // 获取当前层数
    public virtual int GetCurrentStack()
    {
        return currentStack;
    }
    
    // MonoBehaviour的生命周期方法
    public void Start()
    {
        Init();
    }

    public void Update()
    {
        if(isActive)
        {
            UpdateBuff();
        }
    }
}

