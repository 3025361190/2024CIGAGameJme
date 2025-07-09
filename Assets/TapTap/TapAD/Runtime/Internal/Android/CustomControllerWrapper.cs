#if UNITY_ANDROID
using UnityEngine;
using UnityEngine.Assertions;

namespace TapTap.TapAd.Internal
{
    public sealed class CustomControllerWrapper : AndroidJavaProxy
    {
        private ICustomController _interface;
        public CustomControllerWrapper(ICustomController customController) : base("com.tapsdk.tapad.CustomController")
        {
            _interface = customController;
            Assert.IsNotNull(_interface);
        }

        public bool isCanUseLocation()
        {
            return _interface.CanUseLocation;
        }

        public AndroidJavaObject getTapAdLocation()
        {
            var location = _interface.GetTapAdLocation;
            if (location != null)
            {
                var result = new AndroidJavaObject("com.tapsdk.tapad.TapAdLocation", location.latitude, location.longitude, location.accuracy);
                return result;
            }
            else
            {
                return null;
            }
        }
        
        public bool isCanUsePhoneState()
        {
            return _interface.CanUsePhoneState;
        }
        
        public string getDevImei()
        {
            return _interface.GetDevImei;
        }
        
        public bool isCanUseWifiState()
        {
            return _interface.CanUseWifiState;
        }
        
        public bool isCanUseWriteExternal()
        {
            return _interface.CanUseWriteExternal;
        }
        
        public string getDevOaid()
        {
            return _interface.GetDevOaid;
        }

        public bool alist()
        {
            return _interface.Alist;
        }
        
        public bool isCanUseAndroidId()
        {
            return _interface.CanUseAndroidId;
        }
        
        public AndroidJavaObject provideCustomUser()
        {
            var custom = _interface?.ProvideCustomer();
            if (custom == null)
            {
                return null;
            }
            else
            {
                var androidCustomBuilder = new AndroidJavaObject("com.tapsdk.tapad.CustomUser$Builder");
                var androidCustom = androidCustomBuilder
                    .Call<AndroidJavaObject>("withRealAge", custom.realAge)
                    .Call<AndroidJavaObject>("withRealSex", custom.realSex)
                    .Call<AndroidJavaObject>("withAvatarLevel", custom.avatarLevel)
                    .Call<AndroidJavaObject>("withAvatarSex", custom.avatarSex)
                    .Call<AndroidJavaObject>("withNewUserStatus", custom.newUserStatus)
                    .Call<AndroidJavaObject>("withPayedUserStatus", custom.payedUserStatus)
                    .Call<AndroidJavaObject>("withBeginMissionFinished", custom.beginMissionFinished)
                    .Call<AndroidJavaObject>("withAvatarPayedToolCnt", custom.avatarPayedToolCnt)
                    .Call<AndroidJavaObject>("build");
                return androidCustom;
            }
        }
    }
}
#endif