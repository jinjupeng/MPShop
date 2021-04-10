using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ApiServer.Auth.WeChat.Pay.Entities
{
    public class WXCertificateResult
    {
        public WXCertificate[] data { get; set; }
    }
    /// <summary>
    /// 微信平台证书
    /// 调用微信证书获取接口的返回值类型
    /// </summary>
    public class WXCertificate
    {
        /// <summary>
        /// 证书序号
        /// </summary>
        public string serial_no { get; set; }
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime effective_time { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime expire_time { get; set; }
        /// <summary>
        /// 证书
        /// </summary>
        public EncryptCertificate encrypt_certificate { get; set; }
        /// <summary>
        /// base64格式的明文
        /// </summary>
        [JsonIgnore]
        public string cert { get; set; }
    }
    /// <summary>
    /// 已加密的证书
    /// </summary>
    public class EncryptCertificate
    {
        /// <summary>
        /// 加密算法
        /// </summary>
        public string algorithm { get; set; }
        /// <summary>
        /// 随机数
        /// </summary>
        public string nonce { get; set; }
        /// <summary>
        /// 附件数据
        /// </summary>
        public string associated_data { get; set; }
        /// <summary>
        /// 密文
        /// </summary>
        public string ciphertext { get; set; }
    }
}
