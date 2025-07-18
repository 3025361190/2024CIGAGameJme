/*
文件名：GameManager.cs
编辑人：fortunate瑞
文件描述：GameManager类，全局的单例对象，用于管理游戏流程，关卡，全局数据等
通过单例 + DontDestroyOnLoad()确保GameManager对象在场景切换时不会被销毁且唯一

绑定：
1. 在场景中添加GameManager对象
2. 绑定GameManager脚本
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Assets.Scripts.Data;
using UnityEngine.Playables;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
// using static Unity.VisualScripting.Metadata;

public class GameManager : MonoBehaviour
{
    // 单例实例
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameManagerCreatedInDynamic");
                instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    // 根据设备自动创建的永久数据存储路径
    private string saveDataPath;
    // 数据类
    public Data data;
    // buffmanager
    public BuffManager buffManager;
    // 关卡列表
    private List<levelConfig> levelList;
    // 当前关卡的配置
    public levelConfig currentLevelConfig;
    
    // 玩家数据
    private PlayerData playerData;
    public PlayerData PlayerData => playerData;

    // 游戏设置
    private GameSettings gameSettings;
    public GameSettings GameSettings => gameSettings;


    // global_config.json中的数据
    private int initialBulletCount; // 初始子弹数量
    public int initialHealth; // 初始生命值


    // 计时器,每次加载完场景后代码绑定level_scene中的timer物体中的Timer脚本
    private Timer timer;

    // 当前子弹数量
    public int currentBulletCount;
    // 当前生命值
    public int currentHealth;
    // 当前关卡，0为MainMenu
    public int currentLevel = 0;


    // 敌人总类总数
    public int enemyTypeTotalCount;
    // 已消灭的敌人种类数量
    public int enemyTypeDeadCount;
    // 当前关卡敌人是否全部消灭
    public bool isAllEnemyDead = false;
    // 当前关卡boss是否全部消灭
    public bool isAllBossDead = false;
    // 当前关卡计时是否结束
    public bool isTimeOut = false;
    // 关卡是否已经结束（成功或失败）
    private bool isLevelEnded = false;

    // 操控方式：false 为全摇杆控制；true 为摇杆控制方向，触屏控制射击
    private bool controllMode = true;

    private void Awake()
    {
        // 确保单例
        if (instance != null && instance != this)
        {
            Debug.Log("销毁重复的GameManager实例");
            Destroy(gameObject);
            return;
        }
        // Debug.Log($"[GameManager] 挂载在对象: {gameObject.name}");


        instance = this;
        // Debug.Log("GameManager初始化");
        // 标记为切换场景时，不会被销毁的对象
        DontDestroyOnLoad(gameObject);

        // 绑定buffmanager
        buffManager = GameObject.Find("BuffManager").GetComponent<BuffManager>();

        // 注册场景加载完成的事件监听，并绑定回调方法OnSceneLoaded
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        // 初始化终端设备数据路径
        InitializePaths();
        
        // 加载静态游戏数据
        LoadStaticGameData();

        // 初始化动态游戏数据
        InitializeGameData();
        
        // Debug.Log("GameManager初始化完成");
    }

    // 每次场景加载完成时，都会执行的回调方法
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // Debug.Log($"场景 {scene.name} 加载完成");
        ResumeGame();
        // 每个关卡加载后的初始化
        if(scene.name == "level_scene")
        {
            // 获取EnemySpawner对象
            EnemySpawner[] enemySpawners = GameObject.Find("EnemySpawnerObject").GetComponents<EnemySpawner>();
            enemyTypeTotalCount = enemySpawners.Length;
            // Debug.Log($"当前关卡敌人种类总数: {enemyTypeTotalCount}");
            if(enemySpawners == null)
            {
                Debug.LogError("未找到EnemySpawner对象");
                return;
            }
            // 将敌人数量平均分配给每个EnemySpawner
            foreach (var enemySpawner in enemySpawners)
            {
                enemySpawner.maxEnemyCount = currentLevelConfig.monsNum/enemyTypeTotalCount;
            }
            // 绑定计时器
            timer = GameObject.Find("timer").GetComponent<Timer>();
            SetLevelText();
            AudioManager.Instance.PlayBGM(0);
        }

        // 如果场景是level_scene，level为1,，且isNewPlayer为true，则播放新手教程
        if(scene.name == "level_scene" && currentLevel == 1 && playerData.isNewPlayer)
        {
            // Debug.Log("进入新手引导");
            // 获取teachMgr物体
            GameObject teach = GameObject.Find("teach");
            if (teach == null)
            {
                Debug.LogError("未找到teach物体");
            }

            GameObject teachMgr = teach.transform.Find("teachMgr").gameObject;
            if (teachMgr == null)
            {
                Debug.LogError("未找到teachMgr物体");
            }
            teachMgr.SetActive(true);
            // playerData.isNewPlayer在完成新手引导后，需要设置为false，然后调用SavePlayerData()保存
            Debug.Log("完成新手引导");
            playerData.isNewPlayer = false;
            SavePlayerData();
        }

    }
    

    // 初始化数据路径
    private void InitializePaths()
    {
        // 设置游戏数据保存路径，使用Unity推荐的持久化数据路径
        // Application.persistentDataPath是Unity提供的跨平台本地存储路径
        // 在这里我们创建一个名为"GameData"的子文件夹来存储所有游戏相关数据
        saveDataPath = System.IO.Path.Combine(Application.persistentDataPath, "GameData");
        Debug.Log($"数据存储路径: {saveDataPath}");
        
        if (!Directory.Exists(saveDataPath))
        {
            Debug.Log("GameData文件夹不存在，创建新文件夹");
            Directory.CreateDirectory(saveDataPath);
        }
    }

    // 加载全部静态游戏数据
    private void LoadStaticGameData()
    {
        // Debug.Log("开始加载静态游戏数据...");
        
        // 创建Data实例
        data = new Data();

        // 加载玩家数据
        string playerDataPath = System.IO.Path.Combine(saveDataPath, "player_data.json");
        if (File.Exists(playerDataPath))
        {
            Debug.Log("加载已存在的玩家数据");
            string json = File.ReadAllText(playerDataPath);
            playerData = JsonConvert.DeserializeObject<PlayerData>(json);
            // 访问方法：
            // playerData.GrandPrize;
            // playerData.isNewPlayer;
        }
        else
        {
            Debug.Log("创建新的玩家数据");
            playerData = new PlayerData();
            SavePlayerData();
        }

        // 加载游戏设置
        string settingsPath = System.IO.Path.Combine(saveDataPath, "game_settings.json");
        if (File.Exists(settingsPath))
        {
            Debug.Log("加载已存在的游戏设置");
            string json = File.ReadAllText(settingsPath);
            gameSettings = JsonConvert.DeserializeObject<GameSettings>(json);
        }
        else
        {
            Debug.Log("创建新的游戏设置");
            gameSettings = new GameSettings();
            SaveGameSettings();
        }

        // 加载levels_config.json
        // Debug.Log("加载关卡配置...");
        try
        {
            levelList = data.levelsData["levels"].ToObject<List<levelConfig>>();
            Debug.Log($"成功加载 {levelList.Count} 个关卡配置");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取关卡配置数据失败: {e.Message}");
        }

        // 加载global_config.json
        var globalConfig = data.globalData;
        if (globalConfig != null)
        {   
            // 获取global_config.json中的数据
            try
            {
                initialBulletCount = globalConfig["initialBulletCount"].ToObject<int>();
                initialHealth = globalConfig["initialHealth"].ToObject<int>();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"读取初始子弹数量失败: {e.Message}");
            }
            // Debug.Log("全局配置加载完成");
        }
        else
        {
            Debug.LogError("加载全局配置失败！");
        }
    }

    // 初始化游戏数据
    private void InitializeGameData()
    {
        // Debug.Log("初始化游戏数据...");
        // 初始化子弹数量
        currentBulletCount = initialBulletCount;
        // 初始化生命值
        currentHealth = initialHealth;
        // Debug.Log($"初始化完成: 当前子弹数量={currentBulletCount}, 当前生命值={currentHealth}");
    }

    // 保存玩家数据
    public void SavePlayerData()
    {
        string json = JsonConvert.SerializeObject(playerData, Formatting.Indented);
        File.WriteAllText(System.IO.Path.Combine(saveDataPath, "player_data.json"), json);
    }

    // 保存游戏设置
    public void SaveGameSettings()
    {
        string json = JsonConvert.SerializeObject(gameSettings, Formatting.Indented);
        File.WriteAllText(System.IO.Path.Combine(saveDataPath, "game_settings.json"), json);
    }

    // 保存所有数据
    public void SaveAllData()
    {
        SavePlayerData();
        SaveGameSettings();
    }

    // 设置当前关卡的配置
    public void SetCurrentLevelConfig(int level)
    {
        // 根据level获取当前关卡的配置
        // 获取当前关卡的配置
        currentLevelConfig = levelList[level];
    }

    // 跳转至指定关卡
    public void JumpToLevel(int level)
    {
        // 重置关卡结束标志
        isLevelEnded = false;
        Debug.Log($"尝试跳转到关卡 {level}");
        
        // 检查关卡是否有效
        if (level < 0 )
        {
            Debug.LogError($"无效的关卡编号: {level}");
            return;
        }
        // 移至关卡结束时判断
        // if (level > levelList.Count)
        // {
        //     Debug.LogWarning("成功通关所有关卡！");
        //     // 进入通关结算界面
        //     RestartGame();
        //     return;
        // }
        
        // 更新当前关卡
        currentLevel = level;
        // Debug.Log($"更新当前关卡为: {level}");
        
        // 保存当前数据
        SaveAllData();
        
        SetCurrentLevelConfig(level);
        // Debug.Log($"关卡 {level} 配置已设置");

        // 重置关卡相关数据
        isAllEnemyDead = false;
        isAllBossDead = false;
        enemyTypeDeadCount = 0;
        if(currentLevelConfig.isBoss == 0)
        {
            isAllBossDead = true;
        }
        isTimeOut = false;

        // 加载场景
        if(level == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("level_scene");
        }
        
    }

    // 跳转至下一关
    public void NextLevel()
    {
        JumpToLevel(currentLevel + 1);
    }

    // 游戏暂停
    public void PauseGame(){
        Time.timeScale = 0;
    }

    // 游戏继续
    public void ResumeGame(){
        Time.timeScale = 1;
    }

    // 重新开始
    public void RestartGame(){
        JumpToLevel(0);
        // 重新初始化数据类
        data.Init();
        buffManager.Init();
        // 重新初始化动态游戏数据
        InitializeGameData();
    }

    // 在应用退出时保存数据
    private void OnApplicationQuit()
    {
        Debug.LogWarning("游戏退出，保存所有数据");
        SaveAllData();
    }

    // 计算以消灭的敌人比例
    public void AKindOfEnemyAllDead()
    {
        enemyTypeDeadCount++;
        // Debug.Log($"当前关卡已消灭敌人种类数量: {enemyTypeDeadCount}");
        if(enemyTypeDeadCount >= enemyTypeTotalCount)
        {
            isAllEnemyDead = true;
            // Debug.Log($"当前关卡所有敌人已消灭");
        }
    }
    // 获取当前关卡已经经过的时间
    public float GetPassedTime()
    {
        return timer.GetPassedTime();
    }

    // 判断关卡是否结束
    public void IsLevelEnd()
    {
        // 如果关卡已经结束，直接返回
        if (isLevelEnded) return;

        // boss全死，且当前生命值大于0时，时间结束或敌人全死即关卡成功
        if((isAllEnemyDead || isTimeOut) && isAllBossDead && currentHealth > 0)
        {
            isLevelEnded = true;
            StartCoroutine(LevelSuccess());
        }
        // 当前生命值小于0时，关卡失败
        else if(currentHealth <= 0)
        {
            isLevelEnded = true;
            LevelFail();
        }
        // 时间结束，且boss未死，关卡失败
        else if(isTimeOut && !isAllBossDead)
        {
            isLevelEnded = true;
            LevelFail();
        }
    }

    // 关卡成功结束
    public IEnumerator LevelSuccess()
    {
        Debug.LogWarning("关卡成功");
        // 如果当前关卡是最后一关，则直接显示通关界面，不需要剩余操作
        if (currentLevel == levelList.Count - 1)
        {
            Debug.LogWarning("成功通关所有关卡！");
            PauseGame();
            GameSuccess();
            yield break;
        }
        // 当前场景如果是白汤，则先主动调用回收子弹
        GameObject skillButtonObj = GameObject.Find("SkillButton");
        if (skillButtonObj == null)
        {
            Debug.LogError("未找到skillButton对象！");
            yield break;
        }
        
        SkillButton skillButton = skillButtonObj.GetComponent<SkillButton>();
        if (skillButton == null)
        {
            Debug.LogError("skillButton对象上未找到SkillButton组件！");
            yield break;
        }
        
        if(skillButton.currentSceneType == SceneType.QingTang)
        {
            skillButton.SwitchSceneType();
        }

        
        // 展示胜利界面
        GameObject winWindow = GameObject.Find("winWindow");
        if(winWindow != null)
        {
            SetAnimatorUnscaledTimeRecursively(winWindow.transform);
            foreach(Transform child in winWindow.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("未找到名为winWindow的预制体");
        }

        // 等待1秒
        yield return new WaitForSeconds(1.0f);
        // Debug.Log("1秒等待结束");


        // 暂停游戏
        PauseGame();

        // 展示buff选择界面
        buffManager.ShowBuffChoose(currentLevelConfig.isBoss == 1, currentLevelConfig.buffId);
    }

    // 关卡失败结束
    public void LevelFail()
    {
        Debug.LogWarning("关卡失败");
        // 进入失败结算界面
        PauseGame();
        Transform fail = GameObject.Find("Canvas").transform.Find("失败");
        if (fail == null)
        {
            Debug.LogError("未找到失败物体");
            return;
        }
        fail.gameObject.SetActive(true);
        
        // RestartGame();
    }

    // 游戏通关
    public void GameSuccess()
    {
        Debug.LogWarning("游戏通关");
         // 展示通关界面
        GameObject tongguanWindow = GameObject.Find("tongguanWindow");
        if(tongguanWindow != null)
        {
            SetAnimatorUnscaledTimeRecursively(tongguanWindow.transform);
            foreach(Transform child in tongguanWindow.transform)
            {
                child.gameObject.SetActive(true);
            }
            
            // 找到Button并绑定点击事件
            Button restartButton = tongguanWindow.transform.Find("Tongguo/Button (Legacy)").GetComponent<Button>();
            if(restartButton != null)
            {
                restartButton.onClick.AddListener(RestartGame);
            }
            else
            {
                Debug.LogError("未找到通关界面的回到主界面按钮");
            }
        }
        else
        {
            Debug.LogError("未找到名为tongguanWindow的物体");
        }
    }

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

    void SetLevelText()
    {
        GameObject LevelText = GameObject.Find("LevelText");
        Text LevelTextt = LevelText.GetComponent<Text>();
        LevelTextt.text = currentLevel.ToString();
    }

    public bool GetControllMode()
    {
        return controllMode;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IsLevelEnd();
    }
}




// TODO：https://docs.qq.com/smartsheet/DWGdycUdPSmN0cmJj?groupUin=9dK6NFNlciGjyOzFoy3%252FTQ%253D%253D&ADUIN=1754594226&ADSESSION=1748067384&ADTAG=CLIENT.QQ.6067_.0&ADPUBNO=27427&jumpuin=1754594226&tab=t00i2h&viewId=v2JKhc
// TODO：复活

// TODO：各种buff的实现
// TODO：项目改名

