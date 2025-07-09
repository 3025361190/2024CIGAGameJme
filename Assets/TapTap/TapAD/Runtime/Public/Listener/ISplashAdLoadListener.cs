namespace TapTap.TapAd.Internal
{
    public interface ISplashAdLoadListener : ICommonLoadListener
    {
        /// <summary>
        /// 当 Splash 加载完毕
        /// </summary>
        /// <param name="ad"></param>
        void OnSplashAdLoad(TapSplashAd ad);
    }
}