namespace ApiServer.Auth.WeChat.Pay
{
    /// <summary>
    /// 微信支付相关配置的对象
    /// </summary>
    public class Option
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public string Mchid { get; set; }
        /// <summary>
        /// apiV3对称加密密钥
        /// </summary>
        public string ApiV3SecretKey { get; set; }
        /// <summary>
        /// 商户证书.pem文件相对路径
        /// </summary>
        public string CertPath { get; set; }
        /// <summary>
        /// 商户私钥.pem文件相对路径
        /// </summary>
        public string PrivateKeyPath { get; set; }
    }
}
