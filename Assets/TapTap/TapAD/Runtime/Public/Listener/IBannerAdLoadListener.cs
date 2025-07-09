namespace TapTap.TapAd.Internal
{
    public interface IBannerAdLoadListener : ICommonLoadListener
    {
        /// <summary>
        /// 当 Banner 加载完毕
        /// </summary>
        /// <param name="ad"></param>
        void OnBannerAdLoad(TapBannerAd ad);
    }
}