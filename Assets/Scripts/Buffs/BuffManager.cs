/*
文件名：BuffManager.cs
编辑人：fortunate瑞
文件描述：用于管理buff，暂定继承MonoBehaviour
绑定：在Mainmenu场景创建BuffManager对象，并绑定在BuffManager上
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // 添加UI组件的命名空间

public class BuffManager : MonoBehaviour
{
    private static BuffManager instance;
    public static BuffManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BuffManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("BuffManager");
                    instance = go.AddComponent<BuffManager>();
                }
            }
            return instance;
        }
    }

    // buff选择界面
    private GameObject buffChoose;

    // 用来存储绑定在当前GameObject上的buff（即已经实现的buff，buffid作为key）
    private Dictionary<int, BaseBuff> buffs = new();

    // 运行时

    // 一个list存储当前可以选择的普通buff id
    private List<int> normalBuffIds = new();

    // 一个list存储当前可以选择的boss buff id
    private List<int> bossBuffIds = new();

    // 一个list存储当前已经激活且持续的buffid
    private List<int> activeBuffIds = new();

    // 单例
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // 不可在Awake中调用Init，因为buff的初始化访问GameObject中的实例，而Awake阶段GameObject无法保证初始化完成
        Init();
    }

    // 游戏restart或者初次初始化时调用
    public void Init()
    {
        // 清空所有list
        normalBuffIds.Clear();
        bossBuffIds.Clear();
        activeBuffIds.Clear();
        buffs.Clear();
        // 获取当前GameObject上绑定的所有component
        Component[] allComponents = GetComponents<Component>();
        foreach (var comp in allComponents)
        {
            // 如果component是BaseBuff的子类
            if (comp is BaseBuff buff)
            {
                buff.Init();
                // 将buff添加到buffs字典中
                buffs[buff.buffId] = buff;
                // 根据buff类型添加到normalBuffIds或bossBuffIds
                if (buff.isNormalBuff)
                {
                    normalBuffIds.Add(buff.buffId);
                }
                else
                {
                    bossBuffIds.Add(buff.buffId);
                }
            }
        }
    }

    // 递归设置所有层级的Animator组件为UnscaledTime
    private void SetAnimatorUnscaledTimeRecursively(Transform transform)
    {
        // 设置当前物体的Animator
        var animator = transform.GetComponent<Animator>();
        if (animator != null)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        // 递归设置所有子物体
        foreach (Transform child in transform)
        {
            SetAnimatorUnscaledTimeRecursively(child);
        }
    }

    // 展示选择buff界面
    public void ShowBuffChoose(bool isBossLevel)
    {
        Debug.Log("展示buff选择界面");
        buffChoose = GameObject.Find("buffChoose");
        // 处理buff选择界面
        if (buffChoose != null)
        {
            // 设置buff选项
            if (isBossLevel)
            {
                List<int> bossBuffIndexs = new();
                List<int> normalBuffIndexs = new();
                // 如果可选的boss buff数量不足3个，则用普通buff补充
                int max = bossBuffIds.Count;
                if(max >= 3)
                {
                    bossBuffIndexs = RandomUtils.GetUniqueRandomIntegers(0, max, 3);
                    SetBuffCard(0,buffs[bossBuffIds[bossBuffIndexs[0]]]);
                    SetBuffCard(1,buffs[bossBuffIds[bossBuffIndexs[1]]]);
                    SetBuffCard(2,buffs[bossBuffIds[bossBuffIndexs[2]]]);
                }
                else
                {
                    for(int i = 0; i < max; i++)
                    {
                        SetBuffCard(i, buffs[bossBuffIds[i]]);
                    }
                    int n = 3-max;
                    normalBuffIndexs = RandomUtils.GetUniqueRandomIntegers(0, normalBuffIds.Count, n);
                    for(int i = 0; i < n; i++)
                    {
                        SetBuffCard(i+max, buffs[normalBuffIds[normalBuffIndexs[i]]]);
                    }
                }

                
            }
            else
            {
                List<int> normalBuffIndexs = new();
                int max = normalBuffIds.Count;
                normalBuffIndexs = RandomUtils.GetUniqueRandomIntegers(0, max, 3);
                for(int i = 0; i < 3; i++)
                {
                    SetBuffCard(i, buffs[normalBuffIds[normalBuffIndexs[i]]]);
                }
            }
            
            // 递归设置所有层级的Animator组件
            SetAnimatorUnscaledTimeRecursively(buffChoose.transform);

            // 设置buff选择界面及其子物体为激活状态
            foreach (Transform child in buffChoose.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("未找到名为buffChoose的预制体");
        }
    }

    // 设置buff卡面内容
    private void SetBuffCard(int index, BaseBuff buff)
    {
        // 获取对应的buff卡片（buff1/2/3）
        Transform buffCard = buffChoose.transform.Find($"buffChoose/layout/buff{index + 1}");
        if (buffCard != null)
        {
            // 设置图标
            Transform icon = buffCard.Find("icon");
            if (icon != null)
            {
                // 设置图标
                var iconImage = icon.GetComponent<Image>();
                if (iconImage != null)
                {
                    // 如果有自定义图标则加载，否则保持默认图标
                    if (!string.IsNullOrEmpty(buff.buffIcon))
                    {
                        Sprite iconSprite = Resources.Load<Sprite>($"BuffIcon/{buff.buffIcon}");
                        if (iconSprite != null)
                        {
                            iconImage.sprite = iconSprite;
                            Debug.Log($"buff{index+1}卡片的icon设置为{buff.buffIcon}");
                        }
                        else
                        {
                            Debug.LogError($"iconSprite不正确，buffIcon:{buff.buffIcon}，使用了占位图标");
                        }
                    }
                }
                else
                {
                    Debug.LogError($"未找到buff{index+1}卡片icon中的Image组件");
                }
            }
            else
            {
                Debug.LogError($"未找到buff{index+1}卡片的icon");
            }

            // 设置标题
            Transform describeTitle = buffCard.Find("describeTitle");
            if (describeTitle != null)
            {
                describeTitle.GetComponent<Text>().text = buff.buffName;
            }
            else
            {
                Debug.LogError($"未找到buff{index+1}卡片的describeTitle");
            }

            // 设置描述
            Transform describe = buffCard.Find("describe");
            if (describe != null)
            {
                describe.GetComponent<Text>().text = buff.buffDescription;
            }
            else
            {
                Debug.LogError($"未找到buff{index+1}卡片的describe");
            }
        }
        else
        {
            Debug.LogError($"未找到buff{index+1}卡片");
        }
    }

    // buff被选择
    public void BuffChoosen(int buffId)
    {
        // TODO: 处理buff选择
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
