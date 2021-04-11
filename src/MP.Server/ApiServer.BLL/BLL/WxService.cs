using ApiServer.BLL.IBLL;
using ApiServer.Common;
using ApiServer.Common.Cache;
using ApiServer.Common.Config;
using ApiServer.Common.Encrypt;
using ApiServer.Common.Helpers;
using ApiServer.Common.Http;
using ApiServer.Model.Entity;
using ApiServer.Model.Model.MsgModel;
using ApiServer.Model.Model.ViewModel;
using ApiServer.Model.Model.WX;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.BLL.BLL
{
    public class WxService : IWxService
    {
        private readonly ICacheService _cacheService;
        private readonly IBaseService<sys_user> _baseService;
        private readonly IJwtAuthService _jwtAuthService;
        private readonly ILogger<WxService> _logger;
        private readonly string appId = ConfigTool.Configuration["wxmini:appid"];
        private readonly string appSecret = ConfigTool.Configuration["wxmini:secret"];
        private readonly string REBACK_URL = "/apis/backend";

        public WxService(ICacheService cacheService, IBaseService<sys_user> baseService,
            IJwtAuthService jwtAuthService, ILogger<WxService> logger)
        {
            _cacheService = cacheService;
            _baseService = baseService;
            _jwtAuthService = jwtAuthService;
            _logger = logger;
        }

        /// <summary>
        /// 微信授权登录
        /// </summary>
        /// <param name="wxAuth"></param>
        /// <returns></returns>
        public MsgModel AuthLogin(WXAuth wxAuth)
        {
            var wxDecrypt = WxDecrypt(wxAuth.EncryptData, wxAuth.SessionId, wxAuth.IV);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(wxDecrypt);
            var phoneNumber = dict["phoneNumber"].ToString();
            var user = _baseService.GetModels(a => a.phone == phoneNumber).SingleOrDefault();
            if (user != null) // 登录
            {
                return _jwtAuthService.Login(user.username, user.password);

            }
            else // 注册
            {
                // 加密登录密码
                var initPassword = PasswordEncoder.Encode(CommonUtils.GetStringRandom(10));
                var userDto = new SysUser
                {
                    phone = phoneNumber,
                    password = initPassword
                };
                return _jwtAuthService.SignUp(userDto);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<string> GetSessionIdAsync(string code)
        {
            string url = $"https://api.weixin.qq.com/sns/jscode2session?appid={appId}&secret={appSecret}&js_code={code}&grant_type=authorization_code";
            // 发送get请求
            var res = await HttpUtil.HttpGetAsync(url);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
            var uuid = Guid.NewGuid().ToString();
            // 将值dict放入到缓存中
            var cacheKey = string.Format(CacheKey.WX_SESSIONID_KEY, uuid);
            _cacheService.Add(cacheKey, dict);
            return uuid;
        }

        /// <summary>
        /// 微信加密数据解密
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="sessionId"></param>
        /// <param name="vi"></param>
        /// <returns></returns>
        public string WxDecrypt(string encryptedData, string sessionId, string vi)
        {
            var cacheKey = string.Format(CacheKey.WX_SESSIONID_KEY, sessionId);
            var dict = (Dictionary<string, object>)_cacheService.GetValue(cacheKey);
            var sessionKey = dict["session_key"].ToString();
            return WXBizDataCrypt.AESDecrypt(encryptedData, sessionKey, vi);
        }

        /// <summary>
        /// 创建hash字符串
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string CreateShaString(string ticket, string timestamp, string nonce, string url)
        {
            var string1 = "jsapi_ticket=" + ticket + "&noncestr=" + nonce + "&timestamp=" + timestamp + "&url=" + url;
            return HashTool.ComputeSha256Hash(string1);
        }

        //https://www.cnblogs.com/banluduxing/p/6383950.html
        //https://blog.csdn.net/cc365/article/details/50639769
        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetSignatureAsync(string url)
        {
            //WeChat access_token API endpoint
            var token_url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appId + "&secret=" + appSecret;
            //WeChat jsapi_ticket API endpoint
            var ticket_url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?type=jsapi&access_token=";
            var nonceStr = StringHelper.GetRandomString(16, true, true, true, false, "");
            var res = await HttpUtil.HttpGetAsync(token_url);
            if (string.IsNullOrEmpty(res))
            {
                _logger.LogError("拉取token发生错误，可能需要检查公众号ip白名单");
                return null;
            }
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
            var access_token = data["access_token"];
            var res2 = await HttpUtil.HttpGetAsync(ticket_url + access_token);
            data = JsonConvert.DeserializeObject<Dictionary<string, string>>(res2);
            var ticket = data["ticket"];
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            var sha1 = CreateShaString(ticket, timestamp, nonceStr, url);
            var dict = new Dictionary<string, string>
            {
                { "appId", appId },
                { "timestamp", timestamp },
                { "nonceStr", nonceStr },
                { "signature", sha1 },
                { "url", url }
            };
            return dict;
        }

        /// <summary>
        /// 获取openId
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetOpenIdAsync(string code)
        {
            var url = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid=${appId}&secret=${appSecret}&code=${code}&grant_type=authorization_code";
            var res = await HttpUtil.HttpGetAsync(url);
            if (string.IsNullOrEmpty(res))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
        }

        /// <summary>
        /// 获取用户的详细信息，需要先取到openid
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetUserInfoAsync(string access_token, string openid)
        {
            var url = $"https://api.weixin.qq.com/sns/userinfo?access_token=${access_token}&openid=${openid}&lang=zh_CN";
            var res = await HttpUtil.HttpGetAsync(url);
            if (string.IsNullOrEmpty(res))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
        }
    }
}
