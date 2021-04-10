using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ApiServer.Auth.WeChat.Pay
{
    public static class ServiceCollectionExtensions
    {
        //public static IServiceCollection AddWXPay(this IServiceCollection services, Action<Option> act)
        //{
        //    return services.AddWXPayCore().Configure(act);
        //}
        //public static IServiceCollection AddWXPay(this IServiceCollection services, IConfiguration config)
        //{
        //    return services.AddWXPayCore().Configure<Option>(config);
        //}
        //public static IServiceCollection AddWXPayFull(this IServiceCollection services, Action<Option> act)
        //{
        //    return services.AddBXJGCommon().AddWXPay(act).AddWXPayHttpClient();
        //}
        //public static IServiceCollection AddWXPayFull(this IServiceCollection services, IConfiguration act)
        //{
        //    return services.AddBXJGCommon().AddWXPay(act).AddWXPayHttpClient();
        //}

        public static IServiceCollection AddWXPayCore(this IServiceCollection services)
        {
            return services.AddSingleton<SecretHelper>()
                           .AddSingleton<ICertificateProvider, CertificateDefaultProvider>()
                           .AddTransient<ServiceV3>();
        }
        public static IServiceCollection AddWXPayHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient(Const.HttpClientKey, c =>
            {
                c.BaseAddress = new Uri("https://api.mch.weixin.qq.com/v3/");
            }).AddHttpMessageHandler<SignDelegatingHandler>();
            return services;
        }
        internal static HttpClient CreateClientPay(this IHttpClientFactory httpClientFactory)
        {
            return httpClientFactory.CreateClient(Const.HttpClientKey);
        }
    }
}
