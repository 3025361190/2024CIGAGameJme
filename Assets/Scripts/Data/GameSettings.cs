using System;
using Newtonsoft.Json;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class GameSettings
    {
        public float musicVolume { get; set; } = 1.0f;

        public float soundVolume { get; set; } = 1.0f;

        public bool isFullscreen { get; set; } = true;

        public bool isVibration { get; set; } = true;

        // true表示点击屏幕射击，false表示摇杆射击
        public bool shootingMode { get; set; } = false;
    }
} 