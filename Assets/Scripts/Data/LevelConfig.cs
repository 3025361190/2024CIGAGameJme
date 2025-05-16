/*
文件名：LevelConfig.cs
编辑人：fortunate瑞
文件描述：关卡配置类，用于将json中的数据转换为对象
*/


using UnityEngine;

[System.Serializable]
public class LevelConfig
{
    public int levelId;                    // 关卡ID
    public int isBoss;                     // 是否是Boss关卡
    public int bossNum;                    // Boss数量
    public int monsNum;                    // 小怪数量
    public int bossTime;                   // Boss出现时间
    public int[] buffId;                   // BuffID列表
    public int time;                       // 关卡时间


} 