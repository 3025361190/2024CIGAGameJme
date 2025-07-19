/*
文件名：LevelConfig.cs
编辑人：fortunate瑞
文件描述：关卡配置类，用于存储关卡的配置信息
*/

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class levelConfig
    {
        public int levelId { get; set; }
        public int isBoss { get; set; }
        public int bossHealthPoint { get; set; }
        public int monsNum { get; set; }
        public int bossTime { get; set; }
        public List<int> buffId { get; set; }
        public int time { get; set; }
        public int enemyFrequency { get; set; }
        public float enemyMoveSpeed { get; set; }
        public int enemyDamage { get; set; }
        public float bossMoveSpeed { get; set; }
        public int bossDamage { get; set; }
    }
}

