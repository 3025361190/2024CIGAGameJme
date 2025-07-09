using System;
using UnityEngine;

namespace TapTap.TapAd
{
    public interface ITapBaseAd : IDisposable 
    {
        TapAdType AdType { get; }
        TapAdRequest Request { get; }
        ICommonLoadListener LoadLoadListener { get; }
        ICommonInteractionListener InteractionListener { get; }

        #if UNITY_ANDROID
        AndroidJavaObject AdNative { get; }
        #endif
        
        void Load();
        
        void Show();

        void SetLoadListener(ICommonLoadListener loadLoadListener);
        void SetInteractionListener(ICommonInteractionListener interactionListener);
    }
    
    public abstract class TapBaseAd : ITapBaseAd
    {
        public abstract TapAdType AdType { get; }
#if UNITY_ANDROID
        private AndroidJavaObject _adNative;
        public AndroidJavaObject AdNative => _adNative;
#endif
        private ICommonLoadListener _loadLoadListener;
        private ICommonInteractionListener _interactionListener;

        private readonly TapAdRequest _request;
        public TapAdRequest Request => _request;
        
        public ICommonLoadListener LoadLoadListener => _loadLoadListener;
        public ICommonInteractionListener InteractionListener => _interactionListener;

        public virtual bool IsReady
#if UNITY_ANDROID
            => _adNative != null;
#else
            => true;
#endif
        
        protected TapBaseAd(TapAdRequest request)
        {
            this._request = request;
        }

#if UNITY_ANDROID
        internal void SetAdNative(AndroidJavaObject adNative)
        {
            _adNative = adNative;
        }
#endif

        public void SetLoadListener(ICommonLoadListener loadLoadListener)
        {
            _loadLoadListener = loadLoadListener;
        }
        
        public void SetInteractionListener(ICommonInteractionListener interactionListener)
        {
            _interactionListener = interactionListener;
        }
        
        public void Load()
        {
            if (_loadLoadListener == null)
            {
                Debug.LogWarningFormat($"[TapTap:AD] 无法加载广告! 原因: 缺少LoadListener, 广告类型: {this.AdType}");
                return;
            }

            if (_request == null)
            {
                Debug.LogWarningFormat($"[TapTap:AD] 无法加载广告! 原因: 缺少Request参数, 广告类型: {this.AdType}");
                return;
            }

#if UNITY_ANDROID
            if (_adNative != null)
            {
                // maybe we should dispose first
                _adNative?.Dispose();
            }
#endif

            InternalLoad();
        }

        protected virtual void InternalLoad()
        {
            TapAdSdk.LoadAd(this);
        }
        
        public void Show()
        {
            if (_interactionListener == null)
            {
                Debug.LogWarningFormat($"[TapTap:AD] 无法加载广告! 原因: 缺少InteractionListener, 广告类型: {this.AdType}");
                return;
            }

#if UNITY_ANDROID
            if (_adNative == null)
            {
                Debug.LogWarningFormat($"[TapTap:AD] 无法加载广告! 原因: 缺少AdNative, 广告类型: {this.AdType}");
                return;
            }
#endif

            InternalShow();
        }

        protected virtual void InternalShow()
        {
            TapAdSdk.Show(this);
        }

        public virtual void Dispose()
        {
            _loadLoadListener = default;
            _interactionListener = default;
#if UNITY_ANDROID
            TapAdAndroid.RunOnAndroidUiThread(() =>
            {
                if (_adNative != null) {
                    _adNative.Call("dispose");
                    if (AdType == TapAdType.Splash) {
                        _adNative.Call("destroyView");
                    }
                    _adNative = null;
                } else {
                    string errrContent = string.Format($"[Unity:TapAd] call Dispose fail (_adNative is null) | Time: {DateTime.Now.ToString("g")} {AdType}");
                    Debug.LogFormat(errrContent);
                }
            });
#endif
        }
    }
}