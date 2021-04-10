using ApiServer.Auth.Abstractions;
using ApiServer.Auth.Abstractions.LoginModels;
using ApiServer.BLL.IBLL;
using ApiServer.Common.Attributes;
using ApiServer.Model.Entity;
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
using System.Threading.Tasks;

namespace ApiServer.BLL.Handler.LoginHandler
{
    [Singleton]
    public class MPLoginHandler : IMPLoginHandler
    {
        private readonly IBaseService<mp_user> _baseService;
        private readonly ILogger _logger;
        private readonly JwtSettings _jwtSettings;
        private HttpContext httpContext;
        private HttpResponse httpResponse;

        public MPLoginHandler(IBaseService<mp_user> baseService, ILogger<MPLoginHandler> logger,
            IOptions<JwtSettings> options)
        {
            _baseService = baseService;
            _logger = logger;
            _jwtSettings = options.Value;
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
            var mpUser = _baseService.GetModels(a => a.openId == context.Token.openid).SingleOrDefault();
            if (mpUser != null) // 获取用户成功
            {
                // 生成jwt，返回给小程序端
                var accessToken = IssueJwt(new List<Claim>());
                await WriteJsonAsync(new
                {
                    AccessToken = accessToken,
                    ExpireInSeconds = _jwtSettings.ExpireMinutes * 60
                });
            }

            if (mpUser == null) // 未找到关联本地账号
            {
                // 向微信服务器请求用户信息，并保存到本地数据库，同时生成jwt返回给小程序端
                // 将从微信服务器获取到的session_key，保存到jwt中
                var claim = new Claim("session_key", context.Token.session_key);
                var accessToken = IssueJwt(new List<Claim> { claim });
                await WriteJsonAsync(new
                {
                    AccessToken = accessToken,
                    ExpireInSeconds = _jwtSettings.ExpireMinutes * 60
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

    }
}
