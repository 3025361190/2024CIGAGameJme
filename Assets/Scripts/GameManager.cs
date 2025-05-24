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
using Newtonsoft.Json.Linq;
using Assets.Scripts.Data;

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
                GameObject go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    // 数据存储路径
    private string saveDataPath;

    // 关卡列表
    private List<levelConfig> levelList;
    // 当前关卡的配置
    private levelConfig currentLevelConfig;
    
    // 玩家数据
    private PlayerData playerData;
    public PlayerData PlayerData => playerData;

    // 游戏设置
    private GameSettings gameSettings;
    public GameSettings GameSettings => gameSettings;

    // global_config.json中的数据
    private int initialBulletCount; // 初始子弹数量
    // 当前子弹数量
    public int currentBulletCount;

    // 当前关卡，0为MainMenu
    public int currentLevel = 0;

    private void Awake()
    {
        // 确保单例
        if (instance != null && instance != this)
        {
            Debug.Log("销毁重复的GameManager实例");
            Destroy(gameObject);
            return;
        }

        instance = this;
        Debug.Log("GameManager初始化");
        // 标记为切换场景时，不会被销毁的对象
        DontDestroyOnLoad(gameObject);

        // 初始化终端设备数据路径
        InitializePaths();
        
        // 加载静态游戏数据
        LoadStaticGameData();

        // 初始化动态游戏数据
        InitializeGameData();
        
        Debug.Log("GameManager初始化完成");
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
        Debug.Log("开始加载静态游戏数据...");
        
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
        Debug.Log("加载关卡配置...");
        try
        {
            levelList = JsonLoader.LoadJsonAsJObject("StaticData/levels_config")["levels"].ToObject<List<levelConfig>>();
            Debug.Log($"成功加载 {levelList.Count} 个关卡配置");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取关卡配置数据失败: {e.Message}");
        }

        // 加载global_config.json
        Debug.Log("加载全局配置...");
        var globalConfig = JsonLoader.LoadJsonAsJObject("StaticData/global_config");
        if (globalConfig != null)
        {   
            // 获取global_config.json中的数据
            try
            {
                initialBulletCount = globalConfig["initialBulletCount"].ToObject<int>();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"读取初始子弹数量失败: {e.Message}");
            }
            Debug.Log($"全局配置加载完成: 初始子弹={initialBulletCount}");
        }
        else
        {
            Debug.LogError("加载全局配置失败！");
        }
    }

    // 初始化游戏数据
    private void InitializeGameData()
    {
        Debug.Log("初始化游戏数据...");
        // 初始化子弹数量
        currentBulletCount = initialBulletCount;
        Debug.Log($"初始化完成: 当前子弹数量={currentBulletCount}");
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
    public void setCurrentLevelConfig(int level)
    {
        // 根据level获取当前关卡的配置
        // 获取当前关卡的配置
        currentLevelConfig = levelList[level];
    }

    // 跳转至指定关卡
    public void jumpToLevel(int level)
    {
        Debug.Log($"尝试跳转到关卡 {level}");
        
        // 检查关卡是否有效
        if (level < 0 || level > levelList.Count)
        {
            Debug.LogError($"无效的关卡编号: {level}");
            return;
        }
        
        // 更新当前关卡
        currentLevel = level;
        Debug.Log($"更新当前关卡为: {level}");
        
        // 保存当前数据
        SaveAllData();
        
        // 加载场景
        if(level == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("level_scene");
        }
        setCurrentLevelConfig(level);
        Debug.Log($"关卡 {level} 配置已设置");
    }

    // 跳转至下一关
    public void nextLevel()
    {
        jumpToLevel(currentLevel + 1);
    }

    // 游戏暂停
    public void PauseGame(){
        Time.timeScale = 0;
    }

    // 游戏继续
    public void ResumeGame(){
        Time.timeScale = 1;
    }

    // 在应用退出时保存数据
    private void OnApplicationQuit()
    {
        Debug.Log("游戏退出，保存所有数据");
        SaveAllData();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


// TODO: 瑞，playerData.isNewPlayer在完成新手引导后，需要设置为false


// TODO：https://docs.qq.com/smartsheet/DWGdycUdPSmN0cmJj?groupUin=9dK6NFNlciGjyOzFoy3%252FTQ%253D%253D&ADUIN=1754594226&ADSESSION=1748067384&ADTAG=CLIENT.QQ.6067_.0&ADPUBNO=27427&jumpuin=1754594226&tab=t00i2h&viewId=v2JKhc