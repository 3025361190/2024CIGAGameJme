#if UNITY_ANDROID
using UnityEngine;
using UnityEngine.Assertions;

namespace TapTap.TapAd.Internal
{
    internal sealed class SplashAdLoadListenerWrapper : AndroidJavaProxy
    {
        private readonly TapSplashAd _tapSplashAd;
        private readonly ISplashAdLoadListener _loadListener;

        public SplashAdLoadListenerWrapper(TapSplashAd ad) : base("com.tapsdk.tapad.TapAdNative$SplashAdListener")
        {
            _tapSplashAd = ad;
            _loadListener = _tapSplashAd.LoadLoadListener as ISplashAdLoadListener;
            Assert.IsNotNull(_loadListener, "_csharpListener == null !");
        }
        
        /// <summary>
        ///   <para>Called by the java vm whenever a method is invoked on the java proxy interface. You can override this to run special code on method invokation, or you can leave the implementation as is, and leave the default behavior which is to look for c# methods matching the signature of the java method.</para>
        /// </summary>
        /// <param name="methodName">Name of the invoked java method.</param>
        /// <param name="args">Arguments passed from the java vm - converted into AndroidJavaObject, AndroidJavaClass or a primitive.</param>
        /// <param name="javaArgs">Arguments passed from the java vm - all objects are represented by AndroidJavaObject, int for instance is represented by a java.lang.Integer object.</param>
        public override AndroidJavaObject Invoke(string methodName, object[] args) {
            if (methodName == "onError") {
                onError(-1, "Unknown error");
                return null;
            }
            return base.Invoke(methodName, args);
        }
        
        public void onError(int code, string message)
        {
            UnityDispatcher.Instance.PostTask(() => _loadListener?.OnError(code, message));
        }
        
        public void onSplashAdLoad(AndroidJavaObject ad)
        {
            Assert.IsNotNull(ad, "ad == null !");
            UnityDispatcher.Instance.PostTask(() =>
            {
                _tapSplashAd.SetAdNative(ad);
                _loadListener.OnSplashAdLoad(_tapSplashAd);
            });
        }
    }
}
#endif