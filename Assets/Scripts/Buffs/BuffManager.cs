/*
文件名：BuffManager.cs
编辑人：fortunate瑞
文件描述：用于管理buff，暂定继承MonoBehaviour
绑定：在Mainmenu场景创建BuffManager对象，并绑定在BuffManager上
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // 用来存储绑定在当前GameObject上的buff（即已经实现的buff，buffid作为key）
    private Dictionary<int, BaseBuff> buffs = new();

    // 运行时

    // 一个list存储当前可以选择的普通buff id
    private List<int> normalBuffIds = new();

    // 一个list存储当前可以选择的boss buff id
    private List<int> bossBuffIds = new();

    // 一个list存储当前已经激活的buffid
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
            // 如果component是BaseBuff的子类，则将buff添加到buffs字典中
            if (comp is BaseBuff buff)
            {
                buff.Init();
                buffs[buff.buffId] = buff;
            }
        }
    }

    // 展示选择buff界面
    public void ShowBuffChoose(bool isBossLevel)
    {
        // 处理buff选择界面
        GameObject buffChoose = GameObject.Find("buffChoose");
        if (buffChoose != null)
        {
            // 设置buff选项
            // TODO: 根据isBossLevel设置buff选项

            
            // 显示buff选择界面
            foreach (Transform child in buffChoose.transform)
            {
                child.gameObject.SetActive(true);
            }
            buffChoose.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        }
        else
        {
            Debug.LogError("未找到名为buffChoose的预制体");
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
