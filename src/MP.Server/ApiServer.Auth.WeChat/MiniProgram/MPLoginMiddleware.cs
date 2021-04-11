using ApiServer.Auth.Abstractions;
using ApiServer.Auth.Abstractions.LoginModels;
using ApiServer.Common.Http;
using ApiServer.Common.MiniProgram;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiServer.Auth.WeChat.MiniProgram
{
    public class MPLoginMiddleware
    {
        private readonly Option option;
        private readonly RequestDelegate next;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMPLoginHandler _mPLoginHandler;

        public MPLoginMiddleware(RequestDelegate next, IOptionsMonitor<Option> option,
            IHttpClientFactory httpClientFactory, IMPLoginHandler mPLoginHandler)
        {
            this.next = next;
            this.httpClientFactory = httpClientFactory;
            this.option = option.CurrentValue;
            _mPLoginHandler = mPLoginHandler;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;

            //1、若没有匹配上则直接执行下一个中间件
            if (!request.Path.Value.Equals(Const.LoginEndpoint, StringComparison.OrdinalIgnoreCase))
            {
                await next(context);
                return;
            }

            #region 获取code

            Input input;
            WeChatInfo weChatInfo;
            using (var sr = new StreamReader(request.Body))
            {
                var str = await sr.ReadToEndAsync();
                input = JsonSerializer.Deserialize<Input>(str);
                weChatInfo = JsonSerializer.Deserialize<WeChatInfo>(str);
            }

            var loginContext = new LoginContext { Context = context, Option = option, Token = new LoginResult(), WeChatInfo = weChatInfo };

            #endregion

            context.Request.Headers.TryGetValue("Authorization", out StringValues tokenValue);
            var requestToken = tokenValue.ToString();

            if (!weChatInfo.sessionKeyIsValid || !requestToken.Contains('.')) // 如果之前的session_key不可用，则向微信服务器请求新的session_key
            {
                #region 获取session_key和openid

                var requestUrl = QueryHelpers.AddQueryString(Const.OpenIdEndpoint, new Dictionary<string, string>
                {
                    { "appid", option.AppId },
                    { "secret", option.AppSecret },
                    { "js_code",input.code },
                    { "grant_type", "authorization_code" },
                });

                var response = await httpClientFactory.CreateClientMiniProgram().GetStringAsync(requestUrl);
                Console.Write(response);
                var token = JsonSerializer.Deserialize<LoginResult>(response);

                #endregion

                loginContext.Token = token;
            }
            else
            {
                var jwtArr = requestToken.Split('.');
                var payLoad = JsonSerializer.Deserialize<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));
                loginContext.Token.session_key = payLoad["sessionKey"].ToString();
            }

            await _mPLoginHandler.LoginAsync(loginContext);
        }
    }
}
