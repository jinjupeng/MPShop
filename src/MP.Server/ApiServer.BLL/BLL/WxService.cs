using ApiServer.BLL.IBLL;
using ApiServer.Common;
using ApiServer.Common.Cache;
using ApiServer.Common.Config;
using ApiServer.Common.Encrypt;
using ApiServer.Common.Http;
using ApiServer.Model.Entity;
using ApiServer.Model.Model.MsgModel;
using ApiServer.Model.Model.ViewModel;
using ApiServer.Model.Model.WX;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiServer.BLL.BLL
{
    public class WxService : IWxService
    {
        private readonly ICacheService _cacheService;
        private readonly IBaseService<sys_user> _baseService;
        private readonly IJwtAuthService _jwtAuthService;
        private readonly string appId = ConfigTool.Configuration["wxmini:appid"];
        private readonly string appSecret = ConfigTool.Configuration["wxmini:secret"];

        public WxService(ICacheService cacheService, IBaseService<sys_user> baseService,
            IJwtAuthService jwtAuthService)
        {
            _cacheService = cacheService;
            _baseService = baseService;
            _jwtAuthService = jwtAuthService;
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
        public string GetSessionId(string code)
        {
            string url = $"https://api.weixin.qq.com/sns/jscode2session?appid={appId}&secret={appSecret}&js_code={code}&grant_type=authorization_code";
            // 发送get请求
            var res = HttpUtil.HttpGet(url);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
            var uuid = Guid.NewGuid().ToString();
            // 将值dict放入到缓存中
            var cacheKey = string.Format(CacheKey.Wx_Session_Id, uuid);
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
            var cacheKey = string.Format(CacheKey.Wx_Session_Id, sessionId);
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
        //public string GetSignature(string url)
        //{
        //    //WeChat access_token API endpoint
        //    var token_url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appId + "&secret=" + appSecret;
        //    //WeChat jsapi_ticket API endpoint
        //    var ticket_url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?type=jsapi&access_token=";
        //    Random rdm = new Random();
        //    var nonceStr = Math.Round(1).toString(36).substr(2, 15);

        //}
    }
}
