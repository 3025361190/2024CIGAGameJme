#if UNITY_ANDROID
using UnityEngine;
using UnityEngine.Assertions;

namespace TapTap.TapAd.Internal
{
    internal sealed class AndroidActionWrapper : AndroidJavaProxy
    {
        private readonly IUserAction _csharpCallback;
        public AndroidActionWrapper(IUserAction callback) : base("com.tapsdk.tapad.Callback")
        {
            _csharpCallback = callback;
            Assert.IsNotNull(_csharpCallback, "_csharpListener == null !");
        }

        public void onSuccess()
        {
            UnityDispatcher.Instance.PostTask(() => _csharpCallback.OnSuccess());
        }
        
        public void onError(AndroidJavaObject exception)
        {
            UnityDispatcher.Instance.PostTask(() =>
                _csharpCallback.OnError(exception.Get<int>("code"), exception.Get<string>("message")));
        }
    }
}
#endif