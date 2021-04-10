namespace ApiServer.Auth.Abstractions
{

    /// <summary>
    /// JWT配置
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Key
        /// </summary>
        public string SecretKey { get; set; } = "twAJ$j5##pVc5*y&";

        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; } = "http://127.0.0.1:6220";

        /// <summary>
        /// 消费者
        /// </summary>
        public string Audience { get; set; } = "http://127.0.0.1:6220";

        /// <summary>
        /// 有效期(分钟，默认120)
        /// </summary>
        public int ExpireMinutes { get; set; } = 120;

        /// <summary>
        /// 刷新令牌有效期(单位：天，默认7)
        /// </summary>
        public int RefreshTokenExpires { get; set; } = 7;
    }

    /// <summary>
    /// 登录方式
    /// </summary>
    public class LoginModeConfig
    {
        /// <summary>
        /// 用户名登录
        /// </summary>
        public bool UserName { get; set; } = true;

        /// <summary>
        /// 邮箱登录
        /// </summary>
        public bool Email { get; set; }

        /// <summary>
        /// 用户名或邮箱登录
        /// </summary>
        public bool UserNameOrEmail { get; set; }

        /// <summary>
        /// 手机号登录
        /// </summary>
        public bool Phone { get; set; }

        /// <summary>
        /// 微信扫码登录
        /// </summary>
        public bool WeChatScanCode { get; set; }

        /// <summary>
        /// QQ登录
        /// </summary>
        public bool QQ { get; set; }

        /// <summary>
        /// GitHub
        /// </summary>
        public bool GitHub { get; set; }
    }
}
