namespace TapTap.TapAd
{
    public sealed class TapInterstitialAd : TapBaseAd
    {
        public override TapAdType AdType => TapAdType.Interstitial;
        
        public TapInterstitialAd(TapAdRequest request) : base(request)
        {
        }
    }
}