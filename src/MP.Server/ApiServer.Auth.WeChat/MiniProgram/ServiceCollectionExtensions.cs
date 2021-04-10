using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ApiServer.Auth.WeChat.MiniProgram
{
    public static class ServiceCollectionExtensions
    {
        public static HttpClient CreateClientMiniProgram(this IHttpClientFactory httpClientFactory)
        {
            return httpClientFactory.CreateClient(Const.HttpClientName);
        }
        public static IServiceCollection AddWXMiniProgramHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient(Const.HttpClientName, c =>
            {
                c.BaseAddress = new Uri(Const.HttpClientBaseAddress);
            });
            return services;
        }
        //目前没有服务需要注册
        //Option对象使用原生方法就可以了
        //public static IServiceCollection AddWeChatMiniProgram(this IServiceCollection services)
        //{

        //}
    }
}
