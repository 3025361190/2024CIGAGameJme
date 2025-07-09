namespace TapTap.TapAd
{
    public sealed class TapSplashAd : TapBaseAd
    {
        public override TapAdType AdType => TapAdType.Splash;

        public TapSplashAd(TapAdRequest request) : base(request)
        {
        }
    }
}