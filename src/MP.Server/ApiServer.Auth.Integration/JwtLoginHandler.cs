using ApiServer.Auth.Abstractions;
using ApiServer.Common.Attributes;
using ApiServer.Common.Result;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiServer.Auth.Integration
{
    [Singleton]
    public class JwtLoginHandler : ILoginHandler
    {
        private readonly ILogger _logger;
        private readonly JwtSettings _configProvider;

        public JwtLoginHandler(ILogger<JwtLoginHandler> logger, IOptions<JwtSettings> configProvider)
        {
            _logger = logger;
            _configProvider = configProvider.Value;
        }

        /// <summary>
        /// 普通用户登录处理
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="extendData"></param>
        /// <returns></returns>
        public IResultModel Login(List<Claim> claims, string extendData)
        {
            var token = IssueJwt(claims, _configProvider);

            _logger.LogDebug("生成JwtToken：{token}", token);

            return ResultModel.Success(token);
        }


        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private string IssueJwt(List<Claim> claims, JwtSettings config)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(config.Issuer, config.Audience, claims, DateTime.Now, DateTime.Now.AddMinutes(config.ExpireMinutes), signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
