using System;
using System.Collections.Generic;
using UnityEngine;

namespace TapTap.TapAd.Internal
{
    internal static class ListenerWrapperFactory
    {
#if UNITY_ANDROID
        public static AndroidJavaProxy CreateLoadWrapper(TapAdType tapAdType, ITapBaseAd tapBaseAd)
        {
            
            switch (tapAdType)
            {
                case TapAdType.RewardVideo:
                    return new RewardVideoAdLoadListenerWrapper(tapBaseAd as TapRewardVideoAd);
                case TapAdType.Banner: 
                    return new BannerAdLoadListenerWrapper(tapBaseAd as TapBannerAd);
                case TapAdType.Splash: 
                    return new SplashAdLoadListenerWrapper(tapBaseAd as TapSplashAd);
                case TapAdType.Interstitial: 
                    return new InterstitialAdLoadListenerWrapper(tapBaseAd as TapInterstitialAd);
            }
            return null;
        }
        
        public static AndroidJavaProxy CreateInteractionWrapper(TapAdType tapAdType, ITapBaseAd tapBaseAd)
        {
            switch (tapAdType)
            {
                case TapAdType.RewardVideo: 
                    return new RewardVideoAdInteractionListenerWrapper(tapBaseAd as TapRewardVideoAd);
                case TapAdType.Banner: 
                    return new BannerAdInteractionListenerWrapper(tapBaseAd as TapBannerAd);
                case TapAdType.Splash: 
                    return new SplashAdInteractionListenerWrapper(tapBaseAd as TapSplashAd);
                case TapAdType.Interstitial: 
                    return new InterstitialAdInteractionListenerWrapper(tapBaseAd as TapInterstitialAd);
            }
            return null;
        }
#endif
    }
}