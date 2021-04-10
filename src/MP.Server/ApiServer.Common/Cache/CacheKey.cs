using System.ComponentModel;

namespace ApiServer.Common.Cache
{
    /// <summary>
    /// 缓存键
    /// </summary>
    public static class CacheKey
    {
        /// <summary>
        /// 登录验证码
        /// <para>ADMIN:AUTH:VERIFYCODE:用户id</para>
        /// </summary>
        [Description("登录验证码")]
        public const string VERIFYCODE_KEY = "ADMIN:AUTH:VERIFYCODE:{0}";

        /// <summary>
        /// 邮箱验证码
        /// <para>ADMIN:EMAIL:VERIFYCODE:用户id</para>
        /// </summary>
        [Description("邮箱验证码")]
        public const string EMAIL_VERIFYCODE_KEY = "ADMIN:EMAIL:VERIFYCODE:{0}";

        /// <summary>
        /// 手机号验证码
        /// <para>ADMIN:PHONE:VERIFYCODE:用户id</para>
        /// </summary>
        [Description("手机号验证码")]
        public const string PHONE_VERIFYCODE_KEY = "ADMIN:PHONE:VERIFYCODE:{0}";

        /// <summary>
        /// 刷新令牌 
        /// <para>ADMIN:AUTH:REFRESHTOKEN:用户id</para>
        /// </summary>
        [Description("刷新令牌")]
        public const string AUTH_REFRESH_TOKEN = "ADMIN:AUTH:REFRESHTOKEN:{0}";

        /// <summary>
        /// 账户信息
        /// <para>ADMIN:ACCOUNT:INFO:用户id</para>
        /// </summary>
        [Description("账户信息")]
        public const string ACCOUNT = "ADMIN:ACCOUNT:INFO:{0}";

        /// <summary>
        /// 用户权限
        /// <para>ADMIN:ACCOUNT:PERMISSIONS:用户id</para>
        /// </summary>
        [Description("用户权限")]
        public const string USER_PERMISSIONS = "ADMIN:ACCOUNT:PERMISSIONS:{0}";

        /// <summary>
        /// 微信登录session_id
        /// <para>WX:SESSIONID:sessionid</para>
        /// </summary>
        [Description("微信登录")]
        public const string WX_SESSIONID_KEY = "WX:SESSIONID:{0}";

        /// <summary>
        /// 微信openid
        /// <para>WX:OPENID:openid</para>
        /// </summary>
        [Description("微信唯一标识符")]
        public const string WX_OPENID_KEY = "WX:OPENID:{0}";
    }
}
