using ApiServer.Auth.Abstractions;
using ApiServer.Auth.Abstractions.LoginModels;
using ApiServer.Common.MiniProgram;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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


            Input input;
            using (var sr = new StreamReader(request.Body))
            {
                var str = await sr.ReadToEndAsync();
                input = System.Text.Json.JsonSerializer.Deserialize<Input>(str);
            }
            var requestUrl = QueryHelpers.AddQueryString(Const.OpenIdEndpoint, new Dictionary<string, string>
            {
                { "appid", option.AppId },
                { "secret", option.AppSecret },
                { "js_code",input.code },
                { "grant_type", "authorization_code" },
            });
            var response = await httpClientFactory.CreateClientMiniProgram().GetStringAsync(requestUrl);
            var token = System.Text.Json.JsonSerializer.Deserialize<LoginResult>(response);
            await _mPLoginHandler.LoginAsync(new LoginContext { Context = context, Option = option, Token = token });
        }
    }
}
