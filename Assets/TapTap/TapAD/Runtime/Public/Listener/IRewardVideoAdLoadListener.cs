namespace TapTap.TapAd
{
    public interface IRewardVideoAdLoadListener : ICommonLoadListener
    {
        /// <summary>
        /// 当激励视频被Cache完毕
        /// </summary>
        /// <param name="ad"></param>
        void OnRewardVideoAdCached(TapRewardVideoAd ad);
        /// <summary>
        /// 当激励视频被Load完毕
        /// </summary>
        /// <param name="ad"></param>
        void OnRewardVideoAdLoad(TapRewardVideoAd ad);
    }
}