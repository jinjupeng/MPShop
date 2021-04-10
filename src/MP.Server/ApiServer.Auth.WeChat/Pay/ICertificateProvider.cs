using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiServer.Auth.WeChat.Pay
{
    /// <summary>
    /// 微信支付平台证书提供器<br/>
    /// 参考文档：<see cref="" href="https://wechatpay-api.gitbook.io/wechatpay-api-v3/qian-ming-zhi-nan-1/wei-xin-zhi-fu-ping-tai-zheng-shu-geng-xin-zhi-yin" />
    /// </summary>
    public interface ICertificateProvider
    {
        /// <summary>
        /// 获取微信支付平台证书的base64字符串<br/>
        /// 参考文档：<see cref="" href="https://wechatpay-api.gitbook.io/wechatpay-api-v3/qian-ming-zhi-nan-1/wei-xin-zhi-fu-ping-tai-zheng-shu-geng-xin-zhi-yin" />
        /// </summary>
        /// <param name="wechatpaySerial">证书序号</param>
        /// <returns></returns>
        ValueTask<string> GetAsync(string wechatpaySerial, CancellationToken cancellationToken = default);
    }
}
