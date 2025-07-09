namespace TapTap.TapAd
{
    public interface IUserAction
    {
        /// <summary>
        /// 上报成功
        /// </summary>
        void OnSuccess();
        /// <summary>
        /// 上报失败
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="message">信息</param>
        void OnError(int code, string message);
    }
}