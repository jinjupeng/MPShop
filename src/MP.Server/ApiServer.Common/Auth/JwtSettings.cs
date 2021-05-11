namespace ApiServer.Common.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// 证书颁发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 允许使用的角色
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 加密字符串
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 过期时间（分钟）
        /// </summary>
        public int ExpireMinutes { get; set; }
    }
}
