namespace TapTap.TapAd.Internal
{
    public interface IInterstitialAdInteractionListener : ICommonInteractionListener
    {
        /// <summary>
        /// 当广告展示
        /// </summary>
        /// <param name="ad"></param>
        void OnAdShow(TapInterstitialAd ad);
        /// <summary>
        /// 当广告关闭
        /// </summary>
        /// <param name="ad"></param>
        void OnAdClose(TapInterstitialAd ad);
        /// <summary>
        /// 当广告出错
        /// </summary>
        /// <param name="ad"></param>
        void OnAdError(TapInterstitialAd ad);
        /// <summary>
        /// 当广告被有效展示
        /// </summary>
        /// <param name="ad"></param>
        void OnAdValidShow(TapInterstitialAd ad);
        /// <summary>
        /// 当广告被点击
        /// </summary>
        /// <param name="ad"></param>
        void OnAdClick(TapInterstitialAd ad);
        
    }
}