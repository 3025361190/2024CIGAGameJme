/*
文件名：BuffManager.cs
编辑人：fortunate瑞
文件描述：用于管理buff，暂定继承MonoBehaviour
绑定：在Mainmenu场景创建BuffManager对象，并绑定在BuffManager上
      将buffshow预制体绑定在BuffManager上
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
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


    // 绑定在BuffManager上的buffShow预制体
    public GameObject buffShow;

    

    // 用来存储绑定在当前GameObject上的buff（即已经实现的buff，buffid作为key）
    private Dictionary<int, BaseBuff> buffs = new();

    // 运行时（整场游戏）

    // 一个list存储当前可以选择的普通buff id
    private List<int> normalBuffIds = new();

    // 一个list存储当前可以选择的boss buff id
    private List<int> bossBuffIds = new();

    // 一个list存储当前已经激活的buffid
    private List<int> activeBuffIds = new();

    // 运行时（单个关卡）
    // buff选择界面
    private GameObject buffChoose;
    // 一个List记录当前展示的buff卡面和buffid的映射
    private List<int> showBuffIds = new();
    // 记录当前选中的buff卡面index
    private int choosenBuffCardIndex = 0;

    // 代码中绑定场景中的buffChoosed
    private GameObject buffChoosed;

    private Button[] buffCardButtons = new Button[3];



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
        if(buffShow == null)
        {
            Debug.LogError("未绑定buffShow预制体");
        }
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
        showBuffIds.Clear();
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
        // Debug.Log($"normalBuffIds: {string.Join(", ", normalBuffIds)}");
        // Debug.Log($"bossBuffIds: {string.Join(", ", bossBuffIds)}");
        LevelInit();
    }


    // 关卡初始化
    public void LevelInit()
    {
        // 清空所有状态
        choosenBuffCardIndex = 0;
        showBuffIds.Clear();
        // 解除buffChoose的绑定
        buffChoose = null;
        // 解除buffChoosed的绑定
        buffChoosed = null;
        // 清空buffCardButtons
        for(int i = 0; i < 3; i++)
        {
            buffCardButtons[i] = null;
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


    // 获取指定buff的当前层数,0表示未激活或不存在
    public int GetBuffStack(int buffId)
    {
        if(buffs.ContainsKey(buffId))
        {
            return buffs[buffId].GetCurrentStack();
        }
        else
        {
            Debug.LogError($"GetBuffStack未找到buff：{buffId}");
            return 0;
        }
    }

    // 展示选择buff界面
    public void ShowBuffChoose(bool isBossLevel, List<int> levelBuffIds)
    {
        // 初始化状态
        LevelInit();
        
        // Debug.Log("展示buff选择界面");
        buffChoose = GameObject.Find("buffChoose");
        // 处理buff选择界面
        if (buffChoose != null)
        {
            // 设置buff选项
            if (isBossLevel)
            {
                // Boss关卡
                // 获取levelBuffIds和bossBuffIds的交集
                var intersectionBuffIds = levelBuffIds.Intersect(bossBuffIds).ToList();
                // 如果交集大于3，则随机选择3个buff
                if(intersectionBuffIds.Count >= 3)
                {
                    var bossBuffIndexs = RandomUtils.GetUniqueRandomIntegers(0, intersectionBuffIds.Count, 3);
                    for(int i = 0; i < 3; i++)
                    {
                        SetBuffCard(i, buffs[intersectionBuffIds[bossBuffIndexs[i]]]);
                    }
                }
                else
                {
                    // 如果交集小于3，用普通buff补足
                    Debug.LogWarning("关卡指定buff中可用的boss buff不足，用随机可用普通buff补足");
                    for(int i = 0;i < intersectionBuffIds.Count;i++)
                    {
                        SetBuffCard(i, buffs[intersectionBuffIds[i]]);
                    }
                    int n = 3-intersectionBuffIds.Count;
                    if(normalBuffIds.Count < n)
                    {
                        Debug.LogError($"可用的普通buff数量不足以补充！");
                        Debug.Break();  // 让编辑器暂停游戏（仅在编辑器中有效）
                        return;
                    }
                    var normalBuffIndexs = RandomUtils.GetUniqueRandomIntegers(0, normalBuffIds.Count, n);
                    for(int i = 0; i < n; i++)
                    {
                        SetBuffCard(i+intersectionBuffIds.Count, buffs[normalBuffIds[normalBuffIndexs[i]]]);
                    }
                }
            }
            else
            {
                // 普通关卡
                // 获取levelBuffIds和normalBuffIds的交集
                var intersectionBuffIds = levelBuffIds.Intersect(normalBuffIds).ToList();
                // 如果交集大于3，则随机选择3个buff
                if(intersectionBuffIds.Count >= 3)
                {
                    var normalBuffIndexs = RandomUtils.GetUniqueRandomIntegers(0, intersectionBuffIds.Count, 3);
                    for(int i = 0; i < 3; i++)
                    {
                        SetBuffCard(i, buffs[intersectionBuffIds[normalBuffIndexs[i]]]);
                    }
                }
                else
                {
                    Debug.LogError("关卡指定buff中可用的普通buff不足!");
                    Debug.Break();  // 让编辑器暂停游戏（仅在编辑器中有效）
                    return;
                }
            }
            
            // 递归设置所有层级的Animator组件
            SetAnimatorUnscaledTimeRecursively(buffChoose.transform);

            // 设置buff选择界面及其子物体为激活状态
            foreach (Transform child in buffChoose.transform)
            {
                child.gameObject.SetActive(true);
            }
            // 确保NextLevelBtn初始是未激活的
            Transform nextLevelBtn = buffChoose.transform.Find("buffChoose/NextLevelBtn");
            if (nextLevelBtn != null)
            {
                nextLevelBtn.gameObject.SetActive(false);
            }
            // 给按钮绑定事件
            Transform showBuffChoosedBtn = buffChoose.transform.Find("buffChoose/showBuffChoosedBtn");
            if (showBuffChoosedBtn != null)
            {
                showBuffChoosedBtn.GetComponent<Button>().onClick.AddListener(ShowChoosedBuff);
            }
            else
            {
                Debug.LogError("未找到showBuffChoosedBtn");
            }
            buffChoosed = buffChoose.transform.Find("buffChoose/buffChoosed").gameObject;
            buffChoosed.SetActive(false);
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
                            // Debug.Log($"buff{index+1}卡片的icon设置为{buff.buffIcon}");
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
            // 通过index记录当前展示的buff卡面
            showBuffIds.Insert(index, buff.buffId);
            // 为卡面添加click事件
            Button button = buffCard.GetComponent<Button>();
            buffCardButtons[index] = button;
            button.onClick.AddListener(() => BuffCardChoosen(index));
        }
        else
        {
            Debug.LogError($"未找到buff{index+1}卡片");
        }
    }

    // buff卡片被选中
    public void BuffCardChoosen(int cardIndex)
    {
        // Debug.Log($"buff卡片被选中：{cardIndex+1}");
        // 记录当前选中的buff卡面
        choosenBuffCardIndex = cardIndex;
        // EventSystem.current.SetSelectedGameObject(button.gameObject);
        // 将所有buff卡片的颜色重置为正常颜色，选中的卡片设置为selected color
        for(int i = 0; i < 3; i++)
        {
            buffCardButtons[i].image.color = buffCardButtons[i].colors.normalColor;
        }
       buffCardButtons[cardIndex].image.color = buffCardButtons[cardIndex].colors.selectedColor;


        
        // Debug.Log($"choosenBuffCardIndex：{choosenBuffCardIndex}");
        
        // 激活并绑定NextLevelBtn
        if (buffChoose != null)
        {
            Transform nextLevelBtn = buffChoose.transform.Find("buffChoose/NextLevelBtn");
            if (nextLevelBtn != null)
            {
                nextLevelBtn.gameObject.SetActive(true);
                nextLevelBtn.GetComponent<Button>().onClick.RemoveListener(BuffChooseConfirm); // 清除旧的BuffChooseConfirm监听器
                nextLevelBtn.GetComponent<Button>().onClick.AddListener(BuffChooseConfirm);
            }
            else
            {
                Debug.LogError("未找到NextLevelBtn");
            }
        }
    }

    // 确认选择buff
    public void BuffChooseConfirm()
    {
        // Debug.Log($"确认选择buff，当前选中的卡面index：{choosenBuffCardIndex}，showBuffIds：{string.Join(", ", showBuffIds)}");
        if(choosenBuffCardIndex == 0 || choosenBuffCardIndex == 1 || choosenBuffCardIndex == 2)
        {
            ActivateBuff(showBuffIds[choosenBuffCardIndex]);
        }
        else
        {
            Debug.LogError($"无效的buff卡面index：{choosenBuffCardIndex+1}，正确的输入应该为1、2或3");
        }
    }

    // 激活指定buff
    public void ActivateBuff(int buffId)
    {
        // Debug.Log($"激活buff：{buffId}");
        if(buffs.ContainsKey(buffId))
        {
            BaseBuff buff = buffs[buffId];
            buff.ActivateBuff();
            // 检查buffId是否已经存在
            if(!activeBuffIds.Contains(buffId))
            {
                activeBuffIds.Add(buffId);
            }
            int currentStack = buff.GetCurrentStack();
            bool isStackable = buff.buffStackable;
            if(!isStackable && currentStack == 1)
            {
                if(buff.isNormalBuff)
                {
                    normalBuffIds.Remove(buffId);
                    // Debug.Log($"normalBuffIds: {string.Join(", ", normalBuffIds)}");
                }
                else
                {
                    bossBuffIds.Remove(buffId);
                    // Debug.Log($"bossBuffIds: {string.Join(", ", bossBuffIds)}");
                }
            }
        }
        else
        {
            Debug.LogError($"未找到buff：{buffId}");
        }
    }

    // 移除指定已激活的buff
    public void RemoveBuff(int buffId)
    {
        if(!activeBuffIds.Contains(buffId))
        {
            Debug.LogError($"未找到已激活的buff：{buffId}");
            return;
        }
        if(buffs.ContainsKey(buffId))
        {
            buffs[buffId].DeactivateBuff();
            activeBuffIds.Remove(buffId);
            
            if(buffs[buffId].isNormalBuff)
            {
                normalBuffIds.Add(buffId);
            }
            else
            {
                bossBuffIds.Add(buffId);
            }
        }
    }


    // 展示已选buff界面
    public void ShowChoosedBuff()
    {
        if(buffChoosed == null)
        {
            Debug.LogError("未绑定buffChoosed");
            return;
        }
        buffChoosed.SetActive(true);
        // 获取buffchoosed实例下的layout的Transform
        Transform layout = buffChoosed.transform.Find("Viewport/layout");
        if(layout != null)
        {
            // 清空layout下的所有子物体
            foreach(Transform child in layout)
            {
                Destroy(child.gameObject);
            }
            // 在layout下创建buffshow实例
            foreach(int buffId in activeBuffIds)
            {
                BaseBuff buff = buffs[buffId];
                GameObject buffShowInstance = Instantiate(buffShow, layout);
                // 设置buff图标
                var iconImage = buffShowInstance.transform.Find("icon").GetComponent<Image>();
                if (!string.IsNullOrEmpty(buff.buffIcon))
                {
                    Sprite iconSprite = Resources.Load<Sprite>($"BuffIcon/{buff.buffIcon}");
                    if (iconSprite != null)
                    {
                        iconImage.sprite = iconSprite;
                        // Debug.Log($"buff{index+1}卡片的icon设置为{buff.buffIcon}");
                    }
                    else
                    {
                        Debug.LogError($"iconSprite不正确，buffIcon:{buff.buffIcon}，使用了占位图标");
                    }
                }
                // 设置buff名称
                var describeTitle = buffShowInstance.transform.Find("describeTitle");
                if (describeTitle != null)
                {
                    describeTitle.GetComponent<Text>().text = buff.buffName;
                }
                // 设置buff描述
                var describe = buffShowInstance.transform.Find("describe");
                if (describe != null)
                {
                    describe.GetComponent<Text>().text = buff.buffDescription;
                }
                // 设置buff层数
                var stack = buffShowInstance.transform.Find("num");
                if (stack != null)
                {
                    stack.GetComponent<Text>().text = buff.GetCurrentStack().ToString();
                }
            }
        }
        else
        {
            Debug.LogError("未找到layout");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}