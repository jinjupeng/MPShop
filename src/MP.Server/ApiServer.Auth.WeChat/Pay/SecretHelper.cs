using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiServer.Auth.WeChat.Pay
{
    /// <summary>
    /// 微信支付过程中涉及的签名、验签、加解密等<br/>
    /// </summary>
    public class SecretHelper
    {
        /// <summary>
        /// 微信支付模块选项对象
        /// </summary>
        Option option;
        /// <summary>
        /// 微信支付平台证书提供器
        /// </summary>
        ICertificateProvider wxCertificateProvider
        {
            get
            {
                return serviceProvider.GetService<ICertificateProvider>();
            }
        }
        /// <summary>
        /// 时钟
        /// </summary>
        IClock clock;
        /// <summary>
        /// 商户api证书序号
        /// </summary>
        string serialNo;
        /// <summary>
        /// 商户api证书私钥的base64字节数组
        /// </summary>
        byte[] privateKeyRawData;
        /// <summary>
        /// 商户证书
        /// </summary>
        byte[] certData;
        /// <summary>
        /// web环境
        /// </summary>
        IEnv env;
        IServiceProvider serviceProvider;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="option"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="webEnvironment"></param>
        /// <param name="clock"></param>
        public SecretHelper(IOptionsMonitor<Option> option,
                            //ICertificateProvider wxCertificateProvider,//这样注册ioc容器会报循环依赖，非单例注册也许不会爆，没测试过
                            IServiceProvider serviceProvider,
                            IEnv webEnvironment,
                            IClock clock)
        {
            this.serviceProvider = serviceProvider;
            this.clock = clock;
            this.env = webEnvironment;
            this.option = option.CurrentValue;
            option.OnChange(opt => Init());
            Init();
        }

        void Init()
        {
            var path = Path.Combine(env.SecureDirectory, "wx", option.CertPath);
            string str = File.ReadAllText(path);


            certData = Convert.FromBase64String(str.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "").Trim());
            using (var cert = new X509Certificate2(certData, option.Mchid))//这里的商户id作为秘钥的，好像并不需要
            {
                serialNo = cert.SerialNumber;
            }

            //rsaCert = (RSACryptoServiceProvider)cert.PublicKey.Key;
            path = Path.Combine(env.SecureDirectory, "wx", option.PrivateKeyPath);
            str = File.ReadAllText(path);

            privateKeyRawData = Convert.FromBase64String(str.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "").Trim());

        }
        /// <summary>
        /// 对称秘钥解密
        /// </summary>
        /// <param name="associatedData">附件数据</param>
        /// <param name="nonce">随机数</param>
        /// <param name="ciphertext">密文</param>
        /// <returns></returns>
        public string AesGcmDecrypt(string associatedData, string nonce, string ciphertext)
        {
            GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
            AeadParameters aeadParameters = new AeadParameters(
                new KeyParameter(Encoding.UTF8.GetBytes(option.ApiV3SecretKey)),
                128,
                Encoding.UTF8.GetBytes(nonce),
                Encoding.UTF8.GetBytes(associatedData));
            gcmBlockCipher.Init(false, aeadParameters);

            byte[] data = Convert.FromBase64String(ciphertext);
            byte[] plaintext = new byte[gcmBlockCipher.GetOutputSize(data.Length)];
            int length = gcmBlockCipher.ProcessBytes(data, 0, data.Length, plaintext, 0);
            gcmBlockCipher.DoFinal(plaintext, length);
            return Encoding.UTF8.GetString(plaintext);
        }
        /// <summary>
        /// 验证微信支付服务端的签名<br />
        /// 参考：<seealso cref="" href="https://wechatpay-api.gitbook.io/wechatpay-api-v3/ren-zheng/qian-ming-he-zheng-shu#qing-qiu-qian-ming" />
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="body"></param>
        /// <param name="wechatpaySerial"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public async Task<bool> VerifyAsync(string timestamp, string nonce, string body, string wechatpaySerial, string signature, CancellationToken cancellationToken = default)
        {
            var message = timestamp + "\n" + nonce + "\n" + body + "\n";

            byte[] data = Encoding.UTF8.GetBytes(message);
            byte[] data1 = Convert.FromBase64String(signature);

            var wxptzs = await wxCertificateProvider.GetAsync(wechatpaySerial, cancellationToken);
            wxptzs = wxptzs.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "").Trim('\n');
            var rawData = Convert.FromBase64String(wxptzs);

            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(rawData, out int k);
                return rsa.VerifyData(data, data1, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }

            //using (var privateCert = new X509Certificate2(rawData))
            //{
            //    var rsaPrivateKey = (RSACryptoServiceProvider)privateCert.PublicKey.Key;
            //    return rsaPrivateKey.VerifyData(data, data1, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            //}
        }
        /// <summary>
        /// 使用商户私钥对请求做签名
        /// 参考：<seealso cref="" href="https://wechatpay-api.gitbook.io/wechatpay-api-v3/qian-ming-zhi-nan-1/qian-ming-sheng-cheng" />
        /// </summary>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<string> SignAsync(string method, string uri, string body = default(string), CancellationToken cancellationToken = default)
        {
            var timestamp = (await clock.GetNowOffsetAsync()).ToUnixTimeSeconds();
            string nonce = Path.GetRandomFileName();

            string message = method + "\n" + uri + "\n" + timestamp + "\n" + nonce + "\n" + body + "\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            string signature;
            // NOTE： 私钥不包括私钥文件起始的-----BEGIN PRIVATE KEY-----
            //        亦不包括结尾的-----END PRIVATE KEY-----
            //string privateKey = option.PrivateKeyPath;
            //byte[] keyData = Convert.FromBase64String(privateKey);
            //using (CngKey cngKey = CngKey.Import(privateKeyRawData, CngKeyBlobFormat.Pkcs8PrivateBlob))
            //{
            //    //var rsa = RSA.Create();//跨平台方案
            //    //RSACng会调用windows系统实现rsa处理

            //    using (RSACng rsa = new RSACng(cngKey))
            //    {
            //        byte[] data = Encoding.UTF8.GetBytes(message);
            //        signature = Convert.ToBase64String(rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
            //    }
            //}

            using (var rsa = RSA.Create())
            {
                rsa.ImportPkcs8PrivateKey(privateKeyRawData, out int k);
                signature = Convert.ToBase64String(rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
            }

            return "mchid=\"" + option.Mchid + "\",nonce_str=\"" + nonce + "\",timestamp=\"" + timestamp + "\",serial_no=\"" + serialNo + "\",signature=\"" + signature + "\"";
        }
    }

    /// <summary>
    /// 扩展<see cref="SecretHelper"/>，简化调用
    /// </summary>
    public static class SecretHelperExtensions
    {
        /// <summary>
        /// 使用商户私钥对请求做签名
        /// 参考：<seealso cref="" href="https://wechatpay-api.gitbook.io/wechatpay-api-v3/qian-ming-zhi-nan-1/qian-ming-sheng-cheng" />
        /// </summary>
        /// <param name="wxSignValidator"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<string> SignAsync(this SecretHelper wxSignValidator, HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            string method = request.Method.ToString();
            string body = "";
            if (method == "POST" || method == "PUT" || method == "PATCH")
                body = await request.Content.ReadAsStringAsync();

            string uri = request.RequestUri.PathAndQuery;
            return await wxSignValidator.SignAsync(method, uri, body, cancellationToken);
        }

        public static async Task<bool> VerifyAsync(this SecretHelper wxSignValidator, HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            var ps = response.Headers.ToDictionary(c => c.Key, c => c.Value.First());
            var body = await response.Content.ReadAsStringAsync();

            return await wxSignValidator.VerifyAsync(ps, body, cancellationToken);
        }
        /// <summary>
        /// 验证微信支付服务端的签名<br />
        /// 通常将Header转换为<see cref="IDictionary{string, string}"/><br />
        /// <seealso cref="" href="https://wechatpay-api.gitbook.io/wechatpay-api-v3/ren-zheng/qian-ming-he-zheng-shu#qing-qiu-qian-ming" />
        /// </summary>
        /// <param name="wxCertificateProvider"></param>
        /// <param name="requestHeader"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static Task<bool> VerifyAsync(this SecretHelper wxSignValidator, IDictionary<string, string> requestHeader, string body, CancellationToken cancellationToken = default)
        {
            var timestamp2 = requestHeader["Wechatpay-Timestamp"];
            var nonce = requestHeader["Wechatpay-Nonce"];
            var wechatpaySerial = requestHeader["Wechatpay-Serial"];
            var signature = requestHeader["Wechatpay-Signature"];
            return wxSignValidator.VerifyAsync(timestamp2, nonce, body, wechatpaySerial, signature, cancellationToken);
        }
    }
}
