using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ApiServer.Auth.WeChat.Pay
{
    /// <summary>
    /// 自定义的DelegatingHandler处理微信接口通讯中的签名和验签<br/>
    /// 我方向微信支付服务端发起请求时，使用我方私钥进行签名<br/>
    /// 微信支付服务端响应时，我方使用微信支付服务端的公钥进行验签<br/>
    /// 参考：<seealso cref="" href="https://wechatpay-api.gitbook.io/wechatpay-api-v3/qian-ming-zhi-nan-1" />
    /// </summary>
    public class SignDelegatingHandler : DelegatingHandler
    {
        /*
         * 微软秘钥服务文档：
         * https://docs.microsoft.com/zh-cn/dotnet/standard/security/cryptographic-services
         * 
         * 跨平台的RSA：
         * https://docs.microsoft.com/zh-cn/dotnet/api/system.security.cryptography.rsa?view=net-5.0
         * https://docs.microsoft.com/zh-cn/dotnet/api/system.net.http.httpclient?view=netframework-4.6.1#remarks
         * 
         * 《webapi2框架揭秘》书中有对HttpClient的详细说明
         */

        SecretHelper secretHelper;
        public SignDelegatingHandler(SecretHelper secretHelper)
        {
            InnerHandler = new HttpClientHandler();
            this.secretHelper = secretHelper;
        }

        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            //使用商户api证书私钥签名
            var auth = await secretHelper.SignAsync(request);
            string value = $"WECHATPAY2-SHA256-RSA2048 {auth}";
            request.Headers.Add("Authorization", value);
            request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.67 Safari/537.36 Edg/87.0.664.47");
            //发送请求
            var response = await base.SendAsync(request, cancellationToken);

            //使用微信支付平台证书中的公钥解密
            if (!(await secretHelper.VerifyAsync(response)))
                throw new ApplicationException("微信支付响应时，验证签名失败！");
            return response;
        }
    }
}
