namespace ApiServer.Common.MiniProgram
{
    public class Enums
    {
        /// <summary>
        /// 跳转小程序类型：developer为开发版；trial为体验版；formal为正式版；默认为正式版
        /// </summary>
        public enum miniprogram_state
        {
            /// <summary>
            /// 开发版
            /// </summary>
            developer,
            /// <summary>
            /// 正式版
            /// </summary>
            trial,
            /// <summary>
            /// 体验版
            /// </summary>
            formal
        }

        public enum lang
        {
            /// <summary>
            /// 简体中文
            /// </summary>
            zh_CN,
            /// <summary>
            /// 英文
            /// </summary>
            en_US,
            /// <summary>
            /// 繁体中文
            /// </summary>
            zh_HK,
            /// <summary>
            /// 繁体中文
            /// </summary>
            zh_TW
        }
    }
}
