namespace TapTap.TapAd.Internal
{
    public interface IBannerAdInteractionListener : ICommonInteractionListener
    {
        /// <summary>
        /// 当广告展示
        /// </summary>
        /// <param name="ad"></param>
        void OnAdShow(TapBannerAd ad);
        /// <summary>
        /// 当广告关闭
        /// </summary>
        /// <param name="ad"></param>
        void OnAdClose(TapBannerAd ad);
        /// <summary>
        /// 当点击广告
        /// </summary>
        /// <param name="ad"></param>
        void OnAdClick(TapBannerAd ad);
        /// <summary>
        /// 当点击下载
        /// </summary>
        /// <param name="ad"></param>
        void OnDownloadClick(TapBannerAd ad);
        /// <summary>
        /// 当广告被有效展示
        /// </summary>
        /// <param name="ad"></param>
        void OnAdValidShow(TapBannerAd ad);
        
    }
}