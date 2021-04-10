using ApiServer.Auth.WeChat.Pay.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ApiServer.Auth.WeChat.Pay
{
    /// <summary>
    /// 默认的微信支付平台证书提供器<br/>
    /// 参考文档：<see cref="" href="https://wechatpay-api.gitbook.io/wechatpay-api-v3/qian-ming-zhi-nan-1/wei-xin-zhi-fu-ping-tai-zheng-shu-geng-xin-zhi-yin" />
    /// </summary>
    public class CertificateDefaultProvider : ICertificateProvider
    {
        /// <summary>
        /// 微信平台证书获取接口返回的原始数据
        /// </summary>
        private WXCertificateResult wxCertificateResult;
        /// <summary>
        /// 微信支付模块选项对象
        /// </summary>
        private readonly Option wxPaymentOption;
        /// <summary>
        /// 时钟 用于获取准确的当前时间
        /// </summary>
        private readonly IClock clock;
        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILogger logger { get; set; } = NullLogger.Instance;
        /// <summary>
        /// 微信支付模块使用的HttpClientFactory
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;
        /// <summary>
        /// 加解密、签名、验签等
        /// </summary>
        private readonly SecretHelper secretHelper;
        /// <summary>
        /// 存储微信平台证书的文件
        /// </summary>
        private readonly string wxCertPath;
        /// <summary>
        /// 实例化WXCertificateDefaultProvider
        /// </summary>
        /// <param name="wxPaymentOption"></param>
        /// <param name="wxClientFactory"></param>
        /// <param name="secretHelper"></param>
        /// <param name="clock"></param>
        /// <param name="secureDirectory"></param>
        /// <param name="logger"></param>
        public CertificateDefaultProvider(IOptionsMonitor<Option> wxPaymentOption,
                                          IHttpClientFactory wxClientFactory,
                                          SecretHelper secretHelper,
                                          IClock clock,
                                          IEnv secureDirectory)
        {
            this.wxPaymentOption = wxPaymentOption.CurrentValue;
            this.httpClientFactory = wxClientFactory;
            this.clock = clock;
            this.logger = logger;
            this.secretHelper = secretHelper;
            this.wxCertPath = Path.Combine(secureDirectory.SecureDirectory, "wx", "wxpaycert.json");

            var txt = File.ReadAllText(wxCertPath);
            wxCertificateResult = JsonSerializer.Deserialize<WXCertificateResult>(txt);
            wxCertificateResult.data.Select(c => c.cert = secretHelper.AesGcmDecrypt(c.encrypt_certificate.associated_data, c.encrypt_certificate.nonce, c.encrypt_certificate.ciphertext));

            //定时任务检查微信支付平台证书
            var t = new Task(async () =>
            {
                while (true)
                {
                    try
                    {
                        var xzs = wxCertificateResult.data.OrderBy(c => c.effective_time).First();//获取最新的证书
                        var now = await clock.GetNowAsync();
                        if (xzs.expire_time.AddDays(-9) <= now)
                            await UpdateCertAsync();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "微信支付证书更新失败！");
                        //记录日志、触发事件等
                    }
                    await Task.Delay(1000 * 60 * 60 * 23);
                }
            }, TaskCreationOptions.LongRunning);
            t.Start();
        }
        /// <summary>
        /// 获取有效的微信支付平台证书
        /// </summary>
        /// <param name="wechatPaySerial">证书的序号</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask<string> GetAsync(string wechatPaySerial, CancellationToken cancellationToken = default)
        {
            //var token = cancellationToken == null ? CancellationToken.None : cancellationToken.Value;
            var now = await clock.GetNowAsync();
            //if (!wxCertificateResult.data.Any(c => c.serial_no == wechatPaySerial))
            //    await UpdateCertAsync(cancellationToken);
            var zs = wxCertificateResult.data.Single(c => c.serial_no == wechatPaySerial);
            if (zs == null)
                throw new Exception("未找到证书" + wechatPaySerial + "！");
            if (zs.effective_time > now)
                throw new Exception("证书" + wechatPaySerial + "尚未生效！");
            if (zs.expire_time <= now)
                throw new Exception("证书" + wechatPaySerial + "已过期！");
            return zs.cert;
        }
        private async Task UpdateCertAsync(CancellationToken cancellationToken = default)
        {
            var temp = await GetCertAsync(cancellationToken);
            var str = JsonSerializer.Serialize(temp);
            await File.WriteAllTextAsync(wxCertPath, str);
            temp.data.Select(c => c.cert = secretHelper.AesGcmDecrypt(c.encrypt_certificate.associated_data, c.encrypt_certificate.nonce, c.encrypt_certificate.ciphertext));
            this.wxCertificateResult = temp;
        }
        //IWXcertificateProvider的不同实现类可能都需要此方法，可以、但暂时没有进行封装
        /// <summary>
        /// 调用微信接口 获取 微信支付平台证书
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<WXCertificateResult> GetCertAsync(CancellationToken cancellationToken = default)
        {
            //api路径基地址在httpClient中设置
            //var apiUrl = "https://api.mch.weixin.qq.com/v3/certificates";
            var apiUrl = "certificates";
            var response = await httpClientFactory.CreateClientPay().GetAsync(apiUrl);
            return await response.Content.ReadAsAsync<WXCertificateResult>(cancellationToken);
        }
    }
}
