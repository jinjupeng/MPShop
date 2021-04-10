using ApiServer.Auth.Abstractions;
using ApiServer.Common.Result;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ApiServer.Auth.Integration
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Jwt认证
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration cfg)
        {
            services.Configure<JwtSettings>(cfg.GetSection("JwtSettings"));
            var jwtSetting = new JwtSettings();
            cfg.Bind("JwtSettings", jwtSetting);

            #region JWT认证，core自带官方jwt认证
            // 开启Bearer认证
            // .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            services.AddAuthentication(s =>
            {
                //添加JWT Scheme
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // 添加JwtBearer验证服务：
            .AddJwtBearer(config =>
            {
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,// 是否验证Issuer
                    ValidateAudience = true,// 是否验证Audience
                    ValidateLifetime = true,// 是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(30),
                    ValidateIssuerSigningKey = true,// 是否验证SecurityKey
                    ValidAudience = jwtSetting.Audience,// Audience
                    ValidIssuer = jwtSetting.Issuer,// Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey))// 拿到SecurityKey
                };
                config.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // 如果token过期，则把<是否过期>添加到返回头信息中
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    },
                    //此处为权限验证失败后触发的事件
                    OnChallenge = context =>
                    {
                        //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须
                        context.HandleResponse();
                        if (!context.Response.HasStarted)
                        {
                            //自定义自己想要返回的数据结果，我这里要返回的是Json对象，通过引用Newtonsoft.Json库进行转换
                            // var payload = JsonConvert.SerializeObject(new { Code = 0, Message = "很抱歉，您无权访问该接口!" });
                            var payload = JsonConvert.SerializeObject(ResultModel.Failed("很抱歉，您无权访问该接口!"));
                            //自定义返回的数据类型
                            context.Response.ContentType = "application/json";
                            //自定义返回状态码，默认为401 我这里改成 200
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            //输出Json数据结果
                            context.Response.WriteAsync(payload);
                        }

                        return Task.FromResult(0);
                    }
                };
            });
            #endregion

            return services;
        }
    }
}
