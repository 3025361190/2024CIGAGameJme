namespace TapTap.TapAd
{
    public interface ICommonLoadListener
    {
        /// <summary>
        /// 加载出错回调
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="message">错误信息</param>
        void OnError(int code, string message);
    }
}