
namespace TapTap.TapAd
{
    public sealed class TapRewardVideoAd : TapBaseAd
    {
        public override TapAdType AdType => TapAdType.RewardVideo;

        public TapRewardVideoAd(TapAdRequest request) : base(request)
        {
        }
    }
}