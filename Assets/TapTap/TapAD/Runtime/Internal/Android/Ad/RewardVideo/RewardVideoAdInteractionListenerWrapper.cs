#if UNITY_ANDROID
using UnityEngine;
using UnityEngine.Assertions;

namespace TapTap.TapAd.Internal
{
    internal sealed class RewardVideoAdInteractionListenerWrapper : AndroidJavaProxy
    {
        private readonly TapRewardVideoAd _tapRewardAd;
        private readonly IRewardVideoInteractionListener _interactionListener;

        public RewardVideoAdInteractionListenerWrapper(TapRewardVideoAd ad) : base("com.tapsdk.tapad.TapRewardVideoAd$RewardAdInteractionListener")
        {
            _tapRewardAd = ad;
            _interactionListener = ad.InteractionListener as IRewardVideoInteractionListener;
            Assert.IsNotNull(_tapRewardAd, "_csharpListener == null !");
        }

        public void onAdShow()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdShow(_tapRewardAd));
        }

        public void onAdClose()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdClose(_tapRewardAd));
        }

        public void onVideoComplete()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnVideoComplete(_tapRewardAd));
        }
        
        public void onVideoError()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnVideoError(_tapRewardAd));
        }
        
        public void onRewardVerify(bool rewardVerify, int rewardAmount, string rewardName, int code, string msg)
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnRewardVerify(_tapRewardAd, rewardVerify, rewardAmount, rewardName, code, msg));
        }
        
        public void onSkippedVideo()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnSkippedVideo(_tapRewardAd));
        }

        public void onAdClick()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdClick(_tapRewardAd));
        }

        public void onAdValidShow()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdValidShow(_tapRewardAd));
        }
    }
}
#endif