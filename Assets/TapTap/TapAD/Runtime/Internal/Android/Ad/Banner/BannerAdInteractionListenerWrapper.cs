#if UNITY_ANDROID
using UnityEngine;
using UnityEngine.Assertions;

namespace TapTap.TapAd.Internal
{
    internal sealed class BannerAdInteractionListenerWrapper : AndroidJavaProxy
    {
        private readonly TapBannerAd _tapBannerAd;
        private readonly IBannerAdInteractionListener _interactionListener;

        public BannerAdInteractionListenerWrapper(TapBannerAd ad) : base("com.tapsdk.tapad.TapBannerAd$BannerInteractionListener")
        {
            _tapBannerAd = ad;
            _interactionListener = ad.InteractionListener as IBannerAdInteractionListener;
            Assert.IsNotNull(_tapBannerAd, "_csharpListener == null !");
        }

        public void onAdShow()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdShow(_tapBannerAd));
        }

        public void onAdClose()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdClose(_tapBannerAd));
        }

        public void onAdClick()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdClick(_tapBannerAd));
        }
        
        public void onDownloadClick()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnDownloadClick(_tapBannerAd));
        }

        public void onAdValidShow()
        {
            UnityDispatcher.Instance.PostTask(() => _interactionListener?.OnAdValidShow(_tapBannerAd));
        }
    }
}
#endif