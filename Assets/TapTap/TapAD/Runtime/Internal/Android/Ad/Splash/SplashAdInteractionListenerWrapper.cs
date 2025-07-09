#if UNITY_ANDROID
using UnityEngine;
using UnityEngine.Assertions;

namespace TapTap.TapAd.Internal
{
    internal sealed class SplashAdInteractionListenerWrapper : AndroidJavaProxy
    {
        private readonly TapSplashAd _tapSplashAd;
        private readonly ISplashAdInteractionListener _interactionListener;

        public SplashAdInteractionListenerWrapper(TapSplashAd ad) : base("com.tapsdk.tapad.TapSplashAd$AdInteractionListener")
        {
            _tapSplashAd = ad;
            _interactionListener = ad.InteractionListener as ISplashAdInteractionListener;
            Assert.IsNotNull(_tapSplashAd, "_csharpListener == null !");
        }

        public void onAdSkip()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdSkip(_tapSplashAd));
        }
        
        public void onAdTimeOver()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdTimeOver(_tapSplashAd));
        }
 
        public void onAdClick()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdClick(_tapSplashAd));
        }

        public void onAdShow()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdShow(_tapSplashAd));
        }

        public void onAdValidShow()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdValidShow(_tapSplashAd));
        }
    }
}
#endif