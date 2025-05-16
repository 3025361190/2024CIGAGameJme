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
    }
} 