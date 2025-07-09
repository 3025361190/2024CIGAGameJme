namespace TapTap.TapAd
{
    public interface IRewardVideoInteractionListener : ICommonInteractionListener
    {
        /// <summary>
        /// 当广告出现
        /// </summary>
        /// <param name="ad"></param>
        void OnAdShow(TapRewardVideoAd ad);
        
        /// <summary>
        /// 当广告关闭
        /// </summary>
        /// <param name="ad"></param>
        void OnAdClose(TapRewardVideoAd ad);

        /// <summary>
        /// 当视频完成
        /// </summary>
        /// <param name="ad"></param>
        void OnVideoComplete(TapRewardVideoAd ad);

        /// <summary>
        /// 视频出错
        /// </summary>
        /// <param name="ad"></param>
        void OnVideoError(TapRewardVideoAd ad);

        /// <summary>
        /// 奖励确认可以发放
        /// </summary>
        /// <param name="ad"></param>
        /// <param name="rewardVerify"></param>
        /// <param name="rewardAmount"></param>
        /// <param name="rewardName"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        void OnRewardVerify(TapRewardVideoAd ad, bool rewardVerify, int rewardAmount, string rewardName, int code, string msg);
        
        /// <summary>
        /// 当跳过视频
        /// </summary>
        /// <param name="ad"></param>
        void OnSkippedVideo(TapRewardVideoAd ad);

        /// <summary>
        /// 点击广告
        /// </summary>
        /// <param name="ad"></param>
        void OnAdClick(TapRewardVideoAd ad);
        
        /// <summary>
        /// 当广告被有效出现
        /// </summary>
        /// <param name="ad"></param>
        void OnAdValidShow(TapRewardVideoAd ad);
    }
}