using ApiServer.BLL.IBLL;
using ApiServer.Common.Attributes;
using ApiServer.Common.Auth;
using ApiServer.Common.Encrypt;
using ApiServer.Common.Utils;
using ApiServer.Model.Entity;
using Microsoft.AspNetCore.Authentication.WeChat.MiniProgram;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiServer.Extensions.Handlers
{
    public class MiniProgramLoginHandler : IMiniProgramLoginHandler
    {
        private readonly IBaseService<mp_user> _baseService;
        private readonly IBaseService<session_key> _sessionKeyService;
        private readonly ILogger _logger;
        private readonly JwtSettings _jwtSettings;
        private HttpContext httpContext;
        private HttpResponse httpResponse;

        public MiniProgramLoginHandler(IBaseService<mp_user> baseService, ILogger<MiniProgramLoginHandler> logger,
            IOptions<JwtSettings> options, IBaseService<session_key> sessionKeyService)
        {
            _baseService = baseService;
            _logger = logger;
            _jwtSettings = options.Value;
            _sessionKeyService = sessionKeyService;
        }

        /// <summary>
        /// 小程序登录处理
        /// </summary>
        /// <param name="context">保存小程序用户登录信息</param>
        /// <returns></returns>
        [Transaction]
        public async Task<bool> ExcuteAsync(MiniProgramLoginContext context)
        {
            try
            {
                var session_key = context.MiniProgramUser?.session_key;
                var encryptedData = context.MiniProgramUser.Input.GetProperty("encryptedData").GetString();
                var iv = context.MiniProgramUser.Input.GetProperty("iv").GetString();
                this.httpContext = context.HttpContext;
                this.httpResponse = httpContext.Response;
                var deMPUserInfo = WXBizDataCrypt.AESDecrypt(encryptedData, session_key, iv);
                var mpUser = JsonSerializer.Deserialize<mp_user>(deMPUserInfo);
                var openId = mpUser?.openId;
                var mpUserModel = _baseService.GetModels(a => a.openId == openId).SingleOrDefault();
                if (mpUserModel != default) // 如果登录用户存在
                {
                    mpUserModel.updatedAt = DateTime.Now;
                    await _baseService.UpdateAsync(mpUserModel);
                    var sessionKeyModel = await _sessionKeyService.GetEntityAsync(a => a.uid == mpUserModel.id);
                    sessionKeyModel.sessionKey = session_key;
                    sessionKeyModel.updatedAt = DateTime.Now;
                    await _sessionKeyService.UpdateAsync(sessionKeyModel);
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
                    var userInfo = MapUtils.ObjectToMap(mpUserModel);
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
                else // 未找到关联本地账号
                {
                    // 将小程序登录用户信息保存到本地数据库，同时生成jwt返回给小程序端
                    // 将session_key保存到缓存中或token中或数据库中
                    mpUser.createdAt = DateTime.Now;
                    var insertResult = await _baseService.AddAsync(mpUser);
                    var sessionKeyModel = new session_key
                    {
                        uid = insertResult.id,
                        createdAt = DateTime.Now,
                        sessionKey = session_key
                    };
                    await _sessionKeyService.InsertAsync(sessionKeyModel);
                    var sessionKey = new session_key
                    {
                        uid = mpUser.id,
                        sessionKey = session_key,
                        createdAt = DateTime.Now
                    };
                    var claims = new List<Claim>
                    {
                        new Claim("uid", mpUser.id.ToString()),
                        new Claim("nickName", mpUser.nickName),
                        new Claim("avatarUrl", mpUser.avatarUrl),
                        new Claim("openId", mpUser.openId),
                        new Claim("sessionKey", session_key)
                    };
                    var accessToken = IssueJwt(claims);
                    var userInfo = MapUtils.ObjectToMap(insertResult);
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

                return true;
            }
            catch (Exception)
            {
                return false;
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
    }
}
