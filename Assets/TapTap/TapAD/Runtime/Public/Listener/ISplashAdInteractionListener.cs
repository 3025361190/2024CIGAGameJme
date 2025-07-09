namespace TapTap.TapAd.Internal
{
    public interface ISplashAdInteractionListener : ICommonInteractionListener
    {
        /// <summary>
        /// 点击跳过
        /// </summary>
        /// <param name="ad"></param>
        void OnAdSkip(TapSplashAd ad);
        /// <summary>
        /// 广告时间到
        /// </summary>
        /// <param name="ad"></param>
        void OnAdTimeOver(TapSplashAd ad);
        /// <summary>
        /// 广告被点击
        /// </summary>
        /// <param name="ad"></param>
        void OnAdClick(TapSplashAd ad);
        /// <summary>
        /// 广告被展示
        /// </summary>
        /// <param name="ad"></param>
        void OnAdShow(TapSplashAd ad);

        /// <summary>
        /// 广告被有效展示
        /// </summary>
        /// <param name="ad"></param>
        void OnAdValidShow(TapSplashAd ad);
    }
}