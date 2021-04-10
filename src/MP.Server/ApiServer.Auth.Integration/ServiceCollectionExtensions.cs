using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ApiServer.Auth.Integration
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Jwt认证
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddJwtAuth(this IServiceCollection services)
        {

            return services;
        }
    }
}
