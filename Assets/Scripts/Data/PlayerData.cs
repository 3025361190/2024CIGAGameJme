using System;
using Newtonsoft.Json;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PlayerData
    {
        // 是否获得大奖
        public int grandPrize { get; set; } = 0;
        // 是否未通过新手教程
        public bool isNewPlayer { get; set; } = true;
    }
} 