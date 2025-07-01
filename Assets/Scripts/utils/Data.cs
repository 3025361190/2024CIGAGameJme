/*
文件名：Data.cs
编辑人：fortunate瑞
文件描述：用于存储运行时的游戏数据，buff的修改在此处更改
*/


using Newtonsoft.Json.Linq;
using UnityEngine;

public class Data
{
    public JObject buffData;
    public JObject bulletData;
    public JObject enemyData;
    public JObject globalData;
    public JObject levelsData;
    public JObject modeData;
    public JObject rageData;
    public JObject bossData;


    // 初始化数据
    public Data()
    {
        Init();
    }
    public void Init()
    {
        buffData = JsonLoader.LoadJsonAsJObject("StaticData/buffs_config");
        bulletData = JsonLoader.LoadJsonAsJObject("StaticData/bullet_config");
        enemyData = JsonLoader.LoadJsonAsJObject("StaticData/enemy_config");
        globalData = JsonLoader.LoadJsonAsJObject("StaticData/global_config");
        levelsData = JsonLoader.LoadJsonAsJObject("StaticData/levels_config");
        modeData = JsonLoader.LoadJsonAsJObject("StaticData/mode_config");
        rageData = JsonLoader.LoadJsonAsJObject("StaticData/rage_config");
        bossData = JsonLoader.LoadJsonAsJObject("StaticData/boss_config");
        IsDataValid();
    }

    public void IsDataValid()
    {
        try
        {
            if(buffData == null || bulletData == null || enemyData == null || globalData == null || levelsData == null || modeData == null || rageData == null || bossData == null)
            {
                throw new System.Exception("数据初始化失败");
            }
            
        }
        catch(System.Exception e)
        {
            Debug.LogError("数据初始化失败: " + e.Message);
        }
    }
}

// 使用示例
// try
// {
//     levelList = GameManager.Instance.data.levelsData["levels"].ToObject<List<levelConfig>>();
//     Debug.Log($"成功加载 {levelList.Count} 个关卡配置");
// }
// catch (System.Exception e)
// {
//     Debug.LogError($"读取关卡配置数据失败: {e.Message}");
// }