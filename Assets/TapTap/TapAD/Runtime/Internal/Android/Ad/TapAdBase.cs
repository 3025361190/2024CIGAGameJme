using System;

namespace TapTap.TapAd
{
    public abstract class TapAdBase
    {
        internal abstract void Init(TapAdConfig config, ICustomController customController, Action onInited);

        internal abstract void UpdateAdConfig(TapAdConfig config);
        internal abstract void RequestPermissionIfNecessary();
        internal abstract void LoadAd(ITapBaseAd tapBaseAd);
        internal abstract void Show(ITapBaseAd tapBaseAd);
        internal abstract void ToggleEncrypt(bool enableEncrypt);
        internal abstract void ToggleDebugEnv(bool debugEnvToggle);
        internal abstract void UploadUserAction(UserAction[] userActions, IUserAction callback);
        internal abstract void NativeLog(string log);
        internal abstract void NativeError(string log);
    }
}