namespace TapTap.TapAd.Internal
{
    public interface IInterstitialAdLoadListener : ICommonLoadListener
    {
        /// <summary>
        /// 当 Interstitial 加载完毕
        /// </summary>
        /// <param name="ad"></param>
        void OnInterstitialAdLoad(TapInterstitialAd ad);
    }
}