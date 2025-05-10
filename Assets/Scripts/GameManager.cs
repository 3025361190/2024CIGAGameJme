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
    
    // 当前关卡，0为MainMenu
    public int currentLevel = 0;
    
    // 玩家数据
    public PlayerData playerData;

    // 游戏设置
    public GameSettings gameSettings;

    private void Awake()
    {
        // 确保单例
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // 初始化数据路径
        InitializePaths();
        
        // 加载游戏数据
        LoadGameData();
    }

    // 初始化数据路径
    private void InitializePaths()
    {
        // 设置游戏数据保存路径，使用Unity推荐的持久化数据路径
        // Application.persistentDataPath是Unity提供的跨平台本地存储路径
        // 在这里我们创建一个名为"GameData"的子文件夹来存储所有游戏相关数据
        saveDataPath = System.IO.Path.Combine(Application.persistentDataPath, "GameData");
        if (!Directory.Exists(saveDataPath))
        {
            
            Debug.Log("GameData文件夹不存在，默认为新玩家");
            Directory.CreateDirectory(saveDataPath);

            // TODO: 触发新手教程

        }
    }

    // 加载全部游戏数据
    private void LoadGameData()
    {
        // 加载玩家数据
        string playerDataPath = System.IO.Path.Combine(saveDataPath, "player_data.json");
        if (File.Exists(playerDataPath))
        {
            string json = File.ReadAllText(playerDataPath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            playerData = new PlayerData();
            SavePlayerData();
        }

        // 加载游戏设置
        string settingsPath = System.IO.Path.Combine(saveDataPath, "game_settings.json");
        if (File.Exists(settingsPath))
        {
            string json = File.ReadAllText(settingsPath);
            gameSettings = JsonUtility.FromJson<GameSettings>(json);
        }
        else
        {
            gameSettings = new GameSettings();
            SaveGameSettings();
        }
    }

    // 保存玩家数据
    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(System.IO.Path.Combine(saveDataPath, "player_data.json"), json);
    }

    // 保存游戏设置
    public void SaveGameSettings()
    {
        string json = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(System.IO.Path.Combine(saveDataPath, "game_settings.json"), json);
    }

    // 保存所有数据
    public void SaveAllData()
    {
        SavePlayerData();
        SaveGameSettings();
    }


    // 在应用退出时保存数据
    private void OnApplicationQuit()
    {
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

// 玩家数据类
[System.Serializable]
public class PlayerData
{
    public int grandPrize = 0;
    // 可以添加更多玩家相关数据
}

// 游戏设置类
[System.Serializable]
public class GameSettings
{
    public float musicVolume = 1f;
    public float soundVolume = 1f;
    public bool isFullscreen = true;
    public bool isVibration = true;
    // 可以添加更多游戏设置
}

// TODO: 增加config.json的读取，可能要新建多个类
// TODO: 关卡管理函数，例如nextLevel()等
