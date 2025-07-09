namespace TapTap.TapAd
{
    public class UserAction
    {
        /// <summary>
        /// 行为动作类型,可自定义
        /// </summary>
        public int ActionType { get; }
        /// <summary>
        /// 行为发生时间（单位：秒）
        /// </summary>
        public long ActionTime { get; }
        /// <summary>
        /// 付费金额
        /// </summary>
        public int Amount{ get; }
        /// <summary>
        /// 是否胜利.0-失败;1-胜利
        /// </summary>
        public int WinStatus{ get; }

        public UserAction(int actionType, long actionTime, int amount, int winStatus) 
        {
            this.ActionType = actionType;
            this.ActionTime = actionTime;
            this.Amount = amount;
            this.WinStatus = winStatus;
        }
    }
}