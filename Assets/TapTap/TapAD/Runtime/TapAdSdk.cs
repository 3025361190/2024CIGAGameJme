using System;
using TapTap.TapAd.Internal;
using UnityEngine;

namespace TapTap.TapAd
{
    public static class TapAdSdk
    {
        private static TapAdBase worker;

        private static bool isInited;

        public static bool IsInited => isInited;
        
        static TapAdSdk()
        {
            isInited = false;
#if !UNITY_EDITOR
    #if UNITY_ANDROID
            worker = new TapAdAndroid();
    #elif UNITY_IOS

    #endif
#endif
        }
        
        /// <summary>
        /// 该API必须在主线程中调用，若未在主线程初始化则会抛出异常 "Wrong Thread ! Please exec TapAdSdk.init in main thread."
        /// </summary>
        /// <param name="config">初始化参数</param>
        /// <param name="customController">隐私参数</param>
        public static void Init(TapAdConfig config, ICustomController customController, Action onInited = null)
        {
            Debug.LogFormat($"[Unity] TapAd 开始初始化");
            worker?.Init(config, customController, () =>
            {
                isInited = true;
                onInited?.Invoke();
            });
        }

        /// <summary>
        /// 该API必须在主线程中调用，若未在主线程初始化则会抛出异常 "Wrong Thread ! Please exec TapAdSdk.updateAdConfig in main thread."
        /// </summary>
        /// <param name="config">初始化参数</param>
        /// <param name="customController">隐私参数</param>
        public static void UpdateAdConfig(TapAdConfig config)
        {
            Debug.LogFormat($"[Unity] TapAd 开始更新配置");
            worker?.UpdateAdConfig(config);
        }

        public static void RequestPermissionIfNecessary()
        {
            Debug.LogFormat($"[Unity] TapAd 请求权限");
            worker?.RequestPermissionIfNecessary();
        }

        public static void LoadAd(ITapBaseAd tapBaseAd)
        {
            if (IsReady() == false) return;
            worker?.LoadAd(tapBaseAd);
        }
        
        public static void Show(ITapBaseAd tapBaseAd)
        {
            if (IsReady() == false) return;
            worker?.Show(tapBaseAd);
        }

        public static void UploadUserAction(UserAction[] userActions, IUserAction callback)
        {
            worker?.UploadUserAction(userActions, callback);
        }
        
        public static void ToggleEncrypt(bool enableEncrypt)
        {
            worker?.ToggleEncrypt(enableEncrypt);
        }
        
        public static void ToggleDebugEnv(bool debugEnvToggle)
        {
            worker?.ToggleDebugEnv(debugEnvToggle);
        }

        public static void NativeLog(string log)
        {
            worker?.NativeLog(log);
        }
        
        public static void NativeError(string log)
        {
            worker?.NativeError(log);
        }

        private static bool IsReady()
        {
            if (isInited == false)
            {
                Debug.LogWarningFormat($"[TapTap:AD] TapAdSdk 没有初始化!");
                return false;
            }

            return true;
        }
    }
}