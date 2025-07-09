namespace TapTap.TapAd
{
    /// <summary>
    /// 地理位置说明
    /// </summary>
    public sealed class TapAdLocation 
    {
        /// <summary>
        /// 纬度
        /// </summary>
        public double latitude;
        /// <summary>
        /// 经度
        /// </summary>
        public double longitude;
        /// <summary>
        /// 精度
        /// </summary>
        public double accuracy;
        
        /// <summary>
        /// 设置经纬度
        /// </summary>
        /// <param name="latitude">纬度</param>
        /// <param name="longitude">经度</param>
        /// <param name="accuracy">精度</param>
        public TapAdLocation(double latitude, double longitude, double accuracy)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.accuracy = accuracy;
        }
    }
    
    /// <summary>
    /// 用户信息
    /// </summary>
    public sealed class CustomUser
    {
        public int realAge;
        // 0.男;1.女
        public int realSex;
        // 角色性别 0.男;1.女
        public int avatarSex;
        // 角色等级
        public int avatarLevel;
        // 是否新玩家 0.否;1.是
        public int newUserStatus;
        // 是否为付费用户 0.否;1.是
        public int payedUserStatus;
        // 是否通过新手教程 0.否;1.是
        public int beginMissionFinished;
        // 角色当前付费道具数量
        public int avatarPayedToolCnt;
    }
    
    public interface ICustomController
    {
        /// <summary>
        /// 是否允许SDK主动使用地理位置信息
        /// </summary>
        bool CanUseLocation { get; }
        /// <summary>
        /// 当isCanUseLocation=false时，可传入地理位置信息，TapAd使用您传入的地理位置信息
        /// </summary>
        TapAdLocation GetTapAdLocation { get; }
        /// <summary>
        /// 是否允许SDK主动使用手机硬件参数，如：imei
        /// </summary>
        bool CanUsePhoneState { get; }
        /// <summary>
        /// 当isCanUsePhoneState=false时，可传入imei信息，TapAd使用您传入的imei信息
        /// </summary>
        string GetDevImei { get; }
        /// <summary>
        /// 是否允许SDK主动使用ACCESS_WIFI_STATE权限
        /// </summary>
        bool CanUseWifiState { get; }
        /// <summary>
        /// 是否允许SDK主动使用WRITE_EXTERNAL_STORAGE权限
        /// </summary>
        bool CanUseWriteExternal { get; }
        /// <summary>
        /// 开发者可以传入oaid
        /// 信通院OAID的相关采集——如何获取OAID：
        /// 1. 移动安全联盟官网http://www.msa-alliance.cn/
        /// 2. 信通院统一SDK下载http://msa-alliance.cn/col.jsp?id=120
        /// </summary>
        string GetDevOaid { get; }
        /// <summary>
        /// 是否允许SDK主动获取设备上应用安装列表的采集权限
        /// </summary>
        bool Alist { get; }
        /// <summary>
        /// 是否允许SDK主动获取ANDROID_ID
        /// </summary>
        bool CanUseAndroidId { get; }
        /// <summary>
        /// 提供自定义的用户信息
        /// </summary>
        /// <returns></returns>
        CustomUser ProvideCustomer();
    }
}