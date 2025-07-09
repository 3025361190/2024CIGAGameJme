#if UNITY_ANDROID
using UnityEngine;
using UnityEngine.Assertions;

namespace TapTap.TapAd.Internal
{
    internal sealed class InterstitialAdInteractionListenerWrapper : AndroidJavaProxy
    {
        private readonly TapInterstitialAd _tapInterstitialAd;
        private readonly IInterstitialAdInteractionListener _interactionListener;

        public InterstitialAdInteractionListenerWrapper(TapInterstitialAd ad) : base("com.tapsdk.tapad.TapInterstitialAd$InterstitialAdInteractionListener")
        {
            _tapInterstitialAd = ad;
            _interactionListener = ad.InteractionListener as IInterstitialAdInteractionListener;
            Assert.IsNotNull(_tapInterstitialAd, "_csharpListener == null !");
        }

        public void onAdShow()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdShow(_tapInterstitialAd));
        }
        
        public void onAdClose()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdClose(_tapInterstitialAd));
        }
        
        public void onAdError()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdError(_tapInterstitialAd));
        }

        public void onAdValidShow()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdValidShow(_tapInterstitialAd));
        }

        public void onAdClick()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdClick(_tapInterstitialAd));
        }
    }
}
#endif