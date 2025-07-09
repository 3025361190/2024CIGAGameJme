#if UNITY_ANDROID
using System;
using TapTap.TapAd.Internal;
using UnityEngine;
using UnityEngine.Assertions;
using Task = System.Threading.Tasks.Task;

namespace TapTap.TapAd
{
    public class TapAdAndroid : TapAdBase
    {
        private static AndroidJavaObject activity;

        private static AndroidJavaObject tapAdNative;
        
        private static AndroidJavaObject tapAdManager;

        private static AndroidJavaObject TapAdManager => tapAdManager ?? (tapAdManager = GetTapAdManagerInstance());
        
        private static AndroidJavaObject TapAdNative => tapAdNative ?? (tapAdNative = CreateAdNative());

        private static AndroidJavaObject Activity
        {
            get
            {
                if (activity != null) return activity;
                var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                return activity;
            }
        }
        
        /// <summary>
        /// 创建TapAdNative对象用于加载广告 必须采用activity
        /// </summary>
        /// <returns></returns>
        private static AndroidJavaObject GetTapAdManagerInstance()
        {
            var adManagerObject = new AndroidJavaObject("com.tapsdk.tapad.TapAdManager");
            return adManagerObject.CallStatic<AndroidJavaObject>("get");
        }
        
        /// <summary>
        /// 创建TapAdNative对象用于加载广告 必须采用activity
        /// </summary>
        /// <returns></returns>
        private static AndroidJavaObject CreateAdNative()
        {
            var adManager = TapAdManager;
            var result = adManager.Call<AndroidJavaObject>("createAdNative", Activity);
            Assert.IsNotNull(result);
            return result;
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config"></param>
        internal override async void Init(TapAdConfig config, ICustomController customController, Action onInited)
        {
            // create UnityDispatcher instance
            var _ = UnityDispatcher.Instance;
            var adConfigBuilder = new AndroidJavaObject("com.tapsdk.tapad.TapAdConfig$Builder");
            var adConfig = adConfigBuilder
                .Call<AndroidJavaObject>("withMediaId", config.MediaId)
                .Call<AndroidJavaObject>("withMediaName", config.MediaName)
                .Call<AndroidJavaObject>("withMediaKey", config.MediaKey)
                .Call<AndroidJavaObject>("enableDebug", config.IsDebug)
                .Call<AndroidJavaObject>("withMediaVersion", config.MediaVersion)
                .Call<AndroidJavaObject>("withGameChannel", config.Channel)
                .Call<AndroidJavaObject>("withTapClientId", config.TapClientId)
                .Call<AndroidJavaObject>("withData", config.Data)
                .Call<AndroidJavaObject>("shakeEnabled", config.ShakeEnabled)
                .Call<AndroidJavaObject>("build");
            var nativeTapAdKitPlugin = new AndroidJavaObject("com.tapsdk.tapad.NativeTapADKitPlugin");
            Assert.IsNotNull(nativeTapAdKitPlugin);
            var customControllerWrapper = new CustomControllerWrapper(customController);
            RunOnAndroidUiThread(() => nativeTapAdKitPlugin.Call("init", Activity, adConfig, customControllerWrapper));
            RunOnAndroidUiThread(() =>
            {
                var tmp = TapAdNative;
            });
            
            while (tapAdNative == null)
            {
                await Task.Yield();
            }
            
            onInited?.Invoke();
        }

                
        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="config"></param>
        internal override async void UpdateAdConfig(TapAdConfig config) {
            var _ = UnityDispatcher.Instance;
            var adConfigBuilder = new AndroidJavaObject("com.tapsdk.tapad.TapAdConfig$Builder");
            var adConfig = adConfigBuilder
                .Call<AndroidJavaObject>("withData", config.Data)
                .Call<AndroidJavaObject>("build");
            var nativeTapAdKitPlugin = new AndroidJavaObject("com.tapsdk.tapad.NativeTapADKitPlugin");
            Assert.IsNotNull(nativeTapAdKitPlugin);
            RunOnAndroidUiThread(() => nativeTapAdKitPlugin.Call("updateAdConfig", adConfig));
        }
        
        /// <summary>
        /// 请求隐私权限
        /// </summary>
        internal override void RequestPermissionIfNecessary()
        {
            var adManager = TapAdManager;
            RunOnAndroidUiThread(()=>adManager.Call("requestPermissionIfNecessary", Activity));
        }
        
        /// <summary>
        /// 加载广告
        /// </summary>
        /// <param name="tapBaseAd"></param>
        internal override void LoadAd(ITapBaseAd tapBaseAd)
        {
            var adType = tapBaseAd.AdType;
            var request = tapBaseAd.Request;
            AndroidJavaProxy proxy = null;
            var nativeMethodName = "";
            switch (adType)
            {
                case TapAdType.RewardVideo:
                    proxy = ListenerWrapperFactory.CreateLoadWrapper(adType, tapBaseAd as TapRewardVideoAd);
                    nativeMethodName = "loadRewardVideoAd";
                    break;
                case TapAdType.Banner:
                    proxy = ListenerWrapperFactory.CreateLoadWrapper(adType, tapBaseAd as TapBannerAd);
                    nativeMethodName = "loadBannerAd";
                    break;
                case TapAdType.Splash:
                    proxy = ListenerWrapperFactory.CreateLoadWrapper(adType, tapBaseAd as TapSplashAd);
                    nativeMethodName = "loadSplashAd";
                    break;
                case TapAdType.Interstitial:
                    proxy = ListenerWrapperFactory.CreateLoadWrapper(adType, tapBaseAd as TapInterstitialAd);
                    nativeMethodName = "loadInterstitialAd";
                    break;
            }

            // create AdRequest
            var adRequestBuilder = new AndroidJavaObject("com.tapsdk.tapad.AdRequest$Builder");
            var adRequest = adRequestBuilder
                .Call<AndroidJavaObject>("withSpaceId", request.SpaceId)
                .Call<AndroidJavaObject>("withExtra1", request.Extra1)
                .Call<AndroidJavaObject>("withRewardName", request.RewardName)
                .Call<AndroidJavaObject>("withRewardAmount", request.RewardCount)
                .Call<AndroidJavaObject>("withUserId", request.UserId)
                .Call<AndroidJavaObject>("build");
            TapAdNative.Call(nativeMethodName, adRequest, proxy);
        }

        internal override void Show(ITapBaseAd tapBaseAd)
        {
            var adType = tapBaseAd.AdType;
            AndroidJavaProxy proxy = null;
            var setListenerNativeMethodName = "";
            var showNativeMethodName = "";
            switch (adType)
            {
                case TapAdType.RewardVideo:
                    proxy = ListenerWrapperFactory.CreateInteractionWrapper(adType, tapBaseAd as TapRewardVideoAd);
                    setListenerNativeMethodName = "setRewardAdInteractionListener";
                    showNativeMethodName = "showRewardVideoAd";
                    break;
                case TapAdType.Banner:
                    proxy = ListenerWrapperFactory.CreateInteractionWrapper(adType, tapBaseAd as TapBannerAd);
                    setListenerNativeMethodName = "setBannerInteractionListener";
                    showNativeMethodName = "show";
                    break;
                case TapAdType.Splash:
                    proxy = ListenerWrapperFactory.CreateInteractionWrapper(adType, tapBaseAd as TapSplashAd);
                    setListenerNativeMethodName = "setSplashInteractionListener";
                    showNativeMethodName = "show";
                    break;
                case TapAdType.Interstitial:
                    proxy = ListenerWrapperFactory.CreateInteractionWrapper(adType, tapBaseAd as TapInterstitialAd);
                    setListenerNativeMethodName = "setInteractionListener";
                    showNativeMethodName = "show";
                    break;
            }

            var ad = tapBaseAd.AdNative; 
            RunOnAndroidUiThread(() =>
            {
                ad.Call(setListenerNativeMethodName, proxy);
                switch (adType)
                {
                    case TapAdType.RewardVideo:
                    case TapAdType.Splash:
                    case TapAdType.Interstitial:
                        ad.Call(showNativeMethodName, Activity);
                        break;
                    case TapAdType.Banner:
                        var bannerAd = tapBaseAd as TapBannerAd;
                        ad.Call(showNativeMethodName, Activity, bannerAd.baseline, bannerAd.offset);
                        break;
                }
            });
        }

        internal override void ToggleEncrypt(bool enableEncrypt)
        {
            var adManager = TapAdManager;
            adManager.Call("enableEncrypt", enableEncrypt);
        }
        
        internal override void ToggleDebugEnv(bool debugEnvToggle)
        {
            var adManager = TapAdManager;
            adManager.Call("switchTestEnvironment", debugEnvToggle);
        }
        
        internal override void UploadUserAction(UserAction[] userActions, IUserAction callback)
        {
            var arrayClass  = new AndroidJavaClass("java.lang.reflect.Array");
            var arrayObject = arrayClass.CallStatic<AndroidJavaObject>("newInstance", new AndroidJavaClass("com.tapsdk.tapad.UserAction"), userActions.Length);
            for (int i = 0; i < userActions.Length; i++)
            {
                var userActionBuilder = new AndroidJavaObject("com.tapsdk.tapad.UserAction$Builder");
                var userAction = userActionBuilder
                    .Call<AndroidJavaObject>("withActionType", userActions[i].ActionType)
                    .Call<AndroidJavaObject>("withActionTime", userActions[i].ActionTime)
                    .Call<AndroidJavaObject>("withAmount", userActions[i].Amount)
                    .Call<AndroidJavaObject>("withWinStatus", userActions[i].WinStatus)
                    .Call<AndroidJavaObject>("build");
                arrayClass.CallStatic("set", arrayObject, i, userAction);
            }
            var adManager = TapAdManager;
            var wrapper = new AndroidActionWrapper(callback);
            RunOnAndroidUiThread(()=>adManager.Call("uploadUserAction", arrayObject, wrapper));

        }

        internal override void NativeLog(string log)
        {
            var tapAdLogger  = new AndroidJavaClass("com.tapsdk.tapad.internal.utils.TapADLogger");
            tapAdLogger.CallStatic("setIsDebug", true);
            tapAdLogger.CallStatic("d", log);
        }
        
        internal override void NativeError(string log)
        {
            var tapAdLogger  = new AndroidJavaClass("com.tapsdk.tapad.internal.utils.TapADLogger");
            tapAdLogger.CallStatic("e", log);
        }

        internal static void RunOnAndroidUiThread(Action action)
        {
            if (action == null) return;
            var activity = Activity;
            var runnable = new AndroidJavaRunnable(action);
            activity.Call("runOnUiThread", runnable);
        }
    }
}
#endif