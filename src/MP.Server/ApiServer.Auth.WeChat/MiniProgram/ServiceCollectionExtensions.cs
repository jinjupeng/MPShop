using ApiServer.Common.MiniProgram;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace ApiServer.Auth.WeChat.MiniProgram
{
    public static class ServiceCollectionExtensions
    {
        public static HttpClient CreateClientMiniProgram(this IHttpClientFactory httpClientFactory)
        {
            return httpClientFactory.CreateClient(Const.HttpClientName);
        }
        public static IServiceCollection AddWXMiniProgramHttpClient(this IServiceCollection services, IConfiguration cfg)
        {
            //var config = new Option();
            //cfg.GetSection("WeChat:MiniProgram").Bind(config);
            services.Configure<Option>(cfg.GetSection("WeChat:MiniProgram"));
            services.AddHttpClient(Const.HttpClientName, c =>
            {
                c.BaseAddress = new Uri(Const.HttpClientBaseAddress);
            });
            return services;
        }

        public static IApplicationBuilder UseWeChatMiniProgram(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MPLoginMiddleware>();
        }
        //目前没有服务需要注册
        //Option对象使用原生方法就可以了
        //public static IServiceCollection AddWeChatMiniProgram(this IServiceCollection services)
        //{

        //}
    }
}
