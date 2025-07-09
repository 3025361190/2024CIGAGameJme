namespace TapTap.TapAd
{
    public sealed class TapBannerAd : TapBaseAd
    {
        public override TapAdType AdType => TapAdType.Banner;

        /// <summary>
        /// 0：顶部对齐;1：底部对齐
        /// </summary>
        public int baseline;
        
        /// <summary>
        /// 距离基准位置 px
        /// </summary>
        public int offset;
        
        public TapBannerAd(TapAdRequest request) : base(request)
        {
        }
    }
}