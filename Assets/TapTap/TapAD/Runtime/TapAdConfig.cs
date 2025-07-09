namespace TapTap.TapAd
{
    public class TapAdConfig
    {
        /// <summary>
        /// 必选参数。为TapADN注册的媒体ID
        /// </summary>
        public long MediaId { get; }

        /// <summary>
        /// 必选参数。为TapADN注册的媒体名称
        /// </summary>
        public string MediaName { get; }

        /// <summary>
        /// 必须参数,应用的密钥（用于传输数据的解密）
        /// </summary>
        public string MediaKey { get; }

        /// <summary>
        /// 可选参数，是否打开debug调试信息输出：true打开、false关闭。默认false关闭
        /// </summary>
        public bool IsDebug { get; }

        /// <summary>
        /// 必选参数。媒体加密协议版本，可以在TapADN后台查看
        /// </summary>
        public string MediaVersion { get; }
        
        /// <summary>
        /// 必须渠道,版本
        /// </summary>
        public string Channel { get; }
        
        /// <summary>
        /// 可选参数。如果在开发者中心有创建应用，需要填入该参数
        /// </summary>
        public string TapClientId { get; }

        /// <summary>
        /// 可选参数。个性化推荐开关
        /// </summary>
        public string Data { get; }

        public bool ShakeEnabled { get; }

        public TapAdConfig(long mediaId, string mediaName, string key, bool isDebug, string version, string channel, string tapClientId, string data, bool shakeEnabled)
        {
            this.MediaId = mediaId;
            this.MediaName = mediaName;
            this.MediaKey = key;
            this.IsDebug = isDebug;
            this.MediaVersion = version;
            this.Channel = channel;
            this.TapClientId = tapClientId;
            this.Data = data;
            this.ShakeEnabled = shakeEnabled;
        }
        
        public class Builder
        {
            private long _mediaId;
            private string _mediaName;
            private string _mediaKey;
            private bool _isDebug;
            private string _mediaVersion;
            private string _channel;
            private string _tapClientId;

            private string _data;

            private bool _shakeEnabled = true;

            public Builder MediaId(long mediaId)
            {
                _mediaId = mediaId;
                return this;
            }
            
            public Builder MediaName(string mediaName)
            {
                _mediaName = mediaName;
                return this;
            }
            
            public Builder MediaKey(string mediaKey)
            {
                _mediaKey = mediaKey;
                return this;
            }
            
            public Builder EnableDebugLog(bool isDebug)
            {
                _isDebug = isDebug;
                return this;
            }
            
            public Builder MediaVersion(string mediaVersion)
            {
                _mediaVersion = mediaVersion;
                return this;
            }
            
            public Builder Channel(string channel)
            {
                _channel = channel;
                return this;
            }
            
            public Builder TapClientId(string tapClientId)
            {
                _tapClientId = tapClientId;
                return this;
            }

            public Builder Data(string data) {
                _data = data;
                return this;
            }

            public Builder ShakeEnabled(bool shakeEnabled) {
                _shakeEnabled = shakeEnabled;
                return this;
            }

            public TapAdConfig Build()
            {
                return new TapAdConfig(_mediaId, _mediaName, _mediaKey, _isDebug, _mediaVersion, _channel, _tapClientId, _data, _shakeEnabled);
            }
        }
    }
}