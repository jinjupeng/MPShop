using ApiServer.Common.Config;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiServer.Common.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtHelper
    {
        private static JwtSettings _settings;

        /// <summary>
        /// 
        /// </summary>
        public static JwtSettings Settings { set => _settings = value; }

        /// <summary>
        /// 秘钥，可以从配置文件中获取
        /// </summary>
        public static string SecurityKey = ConfigTool.Configuration["Jwt:SecurityKey"];

        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="payLoad"></param>
        /// <returns></returns>
        public static string IssueJwt(Dictionary<string, object> payLoad)
        {
            // 这里就是声明我们的claim
            var claims = new Claim[] {
                    #region token添加自定义参数

                    new Claim(ClaimAttributes.UserId, payLoad[ClaimAttributes.UserId].ToString()),
                    new Claim(ClaimAttributes.UserName, payLoad[ClaimAttributes.UserName].ToString()),

                    #endregion
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    // 这个就是过期时间，目前是过期60秒，可自定义，注意JWT有自己的缓冲过期时间
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(600)).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Iss,_settings.Issuer),
                    new Claim(JwtRegisteredClaimNames.Aud,_settings.Audience),
                    new Claim(JwtRegisteredClaimNames.Sub, "ApiServer")
                };

            // 密钥(SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                // expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }

        ///// <summary>
        ///// 获取jwt中的payLoad
        ///// </summary>
        ///// <param name="encodeJwt">格式：eyAAA.eyBBB.CCC</param>
        ///// <returns></returns>
        //public static Dictionary<string, string> GetPayLoad(string encodeJwt)
        //{
        //    var claimArr = Decode(encodeJwt);
        //    return claimArr.ToDictionary(x => x.Type, x => x.Value);
        //}

        /// <summary>
        /// token解码
        /// </summary>
        /// <param name="encodeJwt">格式：eyAAA.eyBBB.CCC</param>
        /// <returns></returns>
        public static Claim[] Decode(string encodeJwt)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(encodeJwt);
            return jwtSecurityToken?.Claims?.ToArray();
        }

        /// <summary>
        /// 刷新token值
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string RefreshToken(string token)
        {
            string tokenStr = token;
            var oldToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var oldClaims = oldToken.Claims;
            if (long.Parse(GetPayLoad(token)["exp"].ToString()) - ToUnixEpochDate(DateTime.UtcNow) <= 500)
            {
                // 这里就是声明我们的claim
                var claims = new List<Claim>(); // 从旧token中获取到Claim
                claims.AddRange(oldClaims.Where(t => t.Type != JwtRegisteredClaimNames.Iat));
                //重置token的发布时间为当前时间
                string nowDate = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                claims.Add(new Claim(JwtRegisteredClaimNames.Iat, nowDate, ClaimValueTypes.Integer64));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var jwtSecurityToken = new JwtSecurityToken
                (
                    issuer: _settings.Issuer,
                    audience: _settings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_settings.ExpireMinutes)),
                    signingCredentials: cred
                );
                tokenStr = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            }
            return tokenStr;
        }

        /// <summary>
        /// 验证身份 验证签名的有效性,
        /// </summary>
        /// <param name="encodeJwt">格式：eyAAA.eyBBB.CCC</param>
        /// 例如：payLoad["aud"]?.ToString() == "roberAuddience";
        /// 例如：验证是否过期 等
        /// <returns></returns>
        public static bool Validate(string encodeJwt)
        {
            var success = true;
            var jwtArr = encodeJwt.Split('.');
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(SecurityKey));
            var encodedSignature = Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(jwtArr[0], ".", jwtArr[1]))));

            //首先验证签名是否正确（必须的)
            success = string.Equals(jwtArr[2], encodedSignature);
            if (!success)
            {
                return false;
            }

            //其次验证是否在有效期内（也应该必须）
            var now = ToUnixEpochDate(DateTime.UtcNow);
            success = now <= long.Parse(payLoad["exp"].ToString()) && now >= long.Parse(payLoad["nbf"].ToString());
            return success;
        }

        /// <summary>
        /// 获取jwt中的payload
        /// </summary>
        /// <param name="encodeJwt">格式：Bearer eyAAA.eyBBB.CCC</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetPayLoad(string encodeJwt)
        {
            var jwtArr = encodeJwt.Split('.');
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));
            return payLoad;
        }

        /// <summary>
        /// datetime转时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long ToUnixEpochDate(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    }
}
