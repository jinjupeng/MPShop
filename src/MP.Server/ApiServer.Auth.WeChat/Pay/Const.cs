using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Auth.WeChat.Pay
{
    /// <summary>
    /// 微信支付相关常量
    /// </summary>
    public class Const
    {
        /// <summary>
        /// 支付结果通知相对路径
        /// </summary>
        public const string PayNotifyUrl = "wx-pay-notify";
        /// <summary>
        /// 商户私钥pem文件的物理路径配置键
        /// </summary>
        public const string payAppIdConfigKey = "payAppId";
        /// <summary>
        /// APIV3秘钥配置键
        /// </summary>
        public const string ApiV3SecretKeyConfigKey = "apiV3SecretKey";
        /// <summary>
        /// 商户id配置键
        /// </summary>
        public const string mchidConfigKey = "mchid";
        /// <summary>
        /// 商户证书.pem文件物理路径配置键
        /// </summary>
        public const string certPathConfigKey = "certPath";
        /// <summary>
        /// 商户私钥pem文件的物理路径配置键
        /// </summary>
        public const string privateKeyPathConfigKey = "privateKeyPath";
        /// <summary>
        /// 命名化httpClient使用的名称
        /// </summary>
        public const string HttpClientKey = "wxPay";
        /// <summary>
        /// 微信模块下的支付的配置节点名
        /// </summary>
        public const string RootConfigKey = "pay";
    }
}
