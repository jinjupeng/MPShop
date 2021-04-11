using ApiServer.Auth.Abstractions;
using ApiServer.Auth.Abstractions.LoginModels;
using ApiServer.BLL.IBLL;
using ApiServer.Common.Attributes;
using ApiServer.Common.Encrypt;
using ApiServer.Common.MiniProgram;
using ApiServer.Common.Utils;
using ApiServer.Model.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiServer.BLL.Handler.LoginHandler
{
    [Singleton]
    public class MPLoginHandler : IMPLoginHandler
    {
        private readonly IBaseService<mp_user> _baseService;
        private readonly IBaseService<session_key> _sessionKeyService;
        private readonly ILogger _logger;
        private readonly JwtSettings _jwtSettings;
        private HttpContext httpContext;
        private HttpResponse httpResponse;

        public MPLoginHandler(IBaseService<mp_user> baseService, ILogger<MPLoginHandler> logger,
            IOptions<JwtSettings> options, IBaseService<session_key> sessionKeyService)
        {
            _baseService = baseService;
            _logger = logger;
            _jwtSettings = options.Value;
            _sessionKeyService = sessionKeyService;
        }

        /// <summary>
        /// 微信小程序登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task LoginAsync(LoginContext context)
        {
            this.httpContext = context.Context;
            this.httpResponse = httpContext.Response;
            var session_key = context.Token?.session_key;
            var deMPUserInfo = WXBizDataCrypt.AESDecrypt(context.WeChatInfo.encryptedData, session_key, context.WeChatInfo.iv);
            var mpUser = JsonSerializer.Deserialize<mp_user>(deMPUserInfo);
            var openId = mpUser?.openId;
            var mpUserIsExist = _baseService.GetModels(a => a.openId == openId).Any();
            if (mpUserIsExist) // 如果登录用户存在
            {
                // 生成jwt，返回给小程序端
                var claims = new List<Claim>
                {
                    new Claim("uid", mpUser.id.ToString()),
                    new Claim("nickName", mpUser.nickName),
                    new Claim("avatarUrl", mpUser.avatarUrl),
                    new Claim("openId", mpUser.openId),
                    new Claim("sessionKey", session_key)
                };

                var accessToken = IssueJwt(claims);
                var userInfo = MapUtils.ObjectToMap(mpUser);
                userInfo.Add("authorizationToken", accessToken);
                this.httpResponse.ContentType = "application/json";
                this.httpResponse.StatusCode = 200;
                await WriteJsonAsync(new
                {
                    code = 200,
                    msg = "ok",
                    data = userInfo
                });
            }

            if (mpUser == null) // 未找到关联本地账号
            {
                // 向微信服务器请求用户信息，并保存到本地数据库，同时生成jwt返回给小程序端
                // 将session_key保存到缓存中huotoken中或数据库中
                await _baseService.Insert(mpUser);
                var sessionKey = new session_key {
                    uid = mpUser.id,
                    sessionKey = session_key,
                    createdAt = DateTime.Now
                };
                await _sessionKeyService.Insert(sessionKey);
                var claims = new List<Claim>
                {
                    new Claim("uid", mpUser.id.ToString()),
                    new Claim("nickName", mpUser.nickName),
                    new Claim("avatarUrl", mpUser.avatarUrl),
                    new Claim("openId", mpUser.openId),
                    new Claim("sessionKey", session_key)
                };
                var accessToken = IssueJwt(claims);
                var userInfo = MapUtils.ObjectToMap(mpUser);
                userInfo.Add("authorizationToken", accessToken);
                this.httpResponse.ContentType = "application/json";
                this.httpResponse.StatusCode = 200;
                await WriteJsonAsync(new
                {
                    code = 200,
                    msg = "ok",
                    data = userInfo
                });
            }
        }

        /// <summary>
        /// 生成jwt字符串
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public string IssueJwt(List<Claim> claims)
        {
            claims.AddRange(new Claim[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Sub, "MPShop")
                });

            // 密钥(SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpireMinutes),
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }

        public Task WriteJsonAsync(object o)
        {
            return this.httpResponse.WriteAsync(JsonExtensions.SerializeToJson(o), this.httpContext.RequestAborted);
        }

        /// <summary>
        /// 获取jwt中的payload
        /// </summary>
        /// <param name="encodeJwt">格式：Bearer eyAAA.eyBBB.CCC</param>
        /// <returns></returns>
        public Dictionary<string, object> GetPayLoad(string encodeJwt)
        {
            var jwtArr = encodeJwt.Split('.');
            var payLoad = JsonSerializer.Deserialize<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));
            return payLoad;
        }
    }
}
