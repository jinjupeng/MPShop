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
    }
}
