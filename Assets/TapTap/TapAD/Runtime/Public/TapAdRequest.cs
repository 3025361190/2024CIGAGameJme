namespace TapTap.TapAd
{
    public class TapAdRequest
    {
        /// <summary>
        /// 广告位id
        /// </summary>
        public long SpaceId { get; }
        
        /// <summary>
        /// 用于用户自定义的上报
        /// </summary>
        public string Extra1 { get; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// 激励广告额外信息
        /// 选填,奖励的名称
        /// </summary>
        public string RewardName { get; }

        /// <summary>
        /// 激励广告额外信息
        /// 选填,奖励的数量
        /// </summary>
        public int RewardCount { get; }


        public TapAdRequest(long spaceId, string extra1,
            string userId, string rewardName,
            int rewardCount)
        {
            this.SpaceId = spaceId;
            this.Extra1 = extra1;
            this.UserId = userId;
            this.RewardName = rewardName;
            this.RewardCount = rewardCount;
        }

        public class Builder
        {
            private long  _spaceId;
            private string _extra1;
            private string _userId;
            private string _rewardName;
            private int _rewardCount;

            public Builder SpaceId(long spaceId)
            {
                 _spaceId = spaceId;
                return this;
            }
            
            public Builder Extra1(string extra1)
            {
                _extra1 = extra1;
                return this;
            }

            public Builder UserId(string userId)
            {
                _userId = userId;
                return this;
            }
            
            public Builder RewardName(string rewardName)
            {
                _rewardName = rewardName;
                return this;
            } 
            
            public Builder RewardCount(int rewardCount)
            {
                _rewardCount = rewardCount;
                return this;
            }

            public TapAdRequest Build()
            {
                return new TapAdRequest(_spaceId, _extra1, _userId, _rewardName, _rewardCount);
            }
        }
    }
}