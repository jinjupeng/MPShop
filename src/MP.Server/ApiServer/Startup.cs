using ApiServer.Auth.WeChat.MiniProgram;
using ApiServer.BLL.BLL;
using ApiServer.BLL.IBLL;
using ApiServer.Common;
using ApiServer.Common.Auth;
using ApiServer.Common.Config;
using ApiServer.Extensions.Attributes;
using ApiServer.Extensions.Auth;
using ApiServer.Extensions.AutofacModule;
using ApiServer.Extensions.Filters;
using ApiServer.Extensions.Mapping;
using ApiServer.Extensions.ServiceExensions;
using ApiServer.Model.Entity;
using ApiServer.Model.Enum;
using ApiServer.Model.Model.Config;
using ApiServer.Model.Model.MsgModel;
using ApiServer.RabbitMQ;
using AspNetCoreRateLimit;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ApiServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // 使用DI将服务注入到容器中
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAllServices();
            services.AddControllers().AddNewtonsoftJson(
            options =>
            {
                // 序列化时忽略循环
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // 使用驼峰命名
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                // Enum转换为字符串
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                // Int64转换为字符串
                options.SerializerSettings.Converters.Add(new Int64ToStringConvert());
                options.SerializerSettings.Converters.Add(new NullableInt64ToStringConvert());
                // 序列化时是否忽略空值
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                // 序列化时的时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });

            services.Configure<CacheConfig>(Configuration.GetSection("Cache"));
            var cacheConfig = new CacheConfig();
            Configuration.Bind("Cache", cacheConfig);

            if (cacheConfig.Provider == CacheProvider.MemoryCache)
            {
                #region MemoryCache缓存

                services.AddMemoryCache(options =>
                {
                    // SizeLimit缓存是没有大小的，此值设置缓存的份数
                    // 注意：netcore中的缓存是没有单位的，缓存项和缓存的相对关系
                    options.SizeLimit = 1024;
                    // 缓存满的时候压缩20%的优先级较低的数据
                    options.CompactionPercentage = 0.2;
                    // 两秒钟查找一次过期项
                    options.ExpirationScanFrequency = TimeSpan.FromSeconds(2);
                });
                // MemoryCache缓存注入
                services.AddTransient<ICacheService, MemoryCacheService>();

                #endregion
            }
            else if (cacheConfig.Provider == CacheProvider.Redis)
            {
                #region Redis缓存

                services.AddDistributedRedisCache(options =>
                {
                    options.InstanceName = cacheConfig.Redis.Prefix;
                    options.Configuration = cacheConfig.Redis.ConnectionString;
                    options.ConfigurationOptions.DefaultDatabase = cacheConfig.Redis.DefaultDb;
                });
                // Redis缓存注入
                services.AddSingleton<ICacheService, RedisCacheService>();

                #endregion
            }

            services.AddHttpClient();

            services.AddRabbitMQ(Configuration);

            services.AddWXMiniProgramHttpClient(Configuration);

            #region 配置文件绑定

            // oss配置绑定
            services.Configure<OSSConfig>(Configuration.GetSection("OSS"));
            // 文件路径配置绑定
            services.Configure<FilePathConfig>(Configuration.GetSection("FilePath"));

            #endregion

            // 数据库上下文注入
            services.AddDbContext<ContextMySql>(option => option.UseMySql(ConfigTool.Configuration["Setting:Conn"]));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region JwtSetting类注入
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            var jwtSetting = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSetting);
            JwtHelper.Settings = jwtSetting;
            #endregion

            #region 基于策略模式的授权
            // jwt服务注入
            services.AddAuthorization(options =>
            {
                // 增加定义策略
                options.AddPolicy("Permission", policy => policy.Requirements.Add(new PermissionRequirement()));

            })

            #region JWT认证，core自带官方jwt认证
            // 开启Bearer认证
            // .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddAuthentication(s =>
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
                            var payload = JsonConvert.SerializeObject(MsgModel.Fail(403, "很抱歉，您无权访问该接口!"));
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



            #endregion

            #region Cors 跨域
            services.AddCors(options =>
            {
                // 浏览器会发起2次请求,使用OPTIONS发起预检请求，第二次才是api请求
                options.AddPolicy("cors", policy =>
                {
                    policy
                    .SetIsOriginAllowed(origin => true)
                    .SetPreflightMaxAge(new TimeSpan(0, 10, 0))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); //指定处理cookie
                });
            });
            #endregion

            #region 注入自定义策略

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            services.AddMapster();

            #endregion

            #region Swagger UI
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API171",
                    Version = "v1",
                    Description = "基于.NET Core 3.1 的JWT 身份验证",
                    Contact = new OpenApiContact
                    {
                        Name = "jinjupeng",
                        Email = "im.jp@outlook.com.com",
                        Url = new Uri("http://cnblogs.com/jinjupeng"),
                    },
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "ApiServer.xml");
                c.IncludeXmlComments(xmlPath);
            });
            #endregion


            services.AddMvc(options =>
            {
                // 注册全局过滤器
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            }).AddValidation(services); // 添加验证器

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //services.Configure<ForwardedHeadersOptions>(options =>
            //{
            //    options.ForwardedHeaders =
            //        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            //    options.KnownNetworks.Clear();
            //    options.KnownProxies.Clear();
            //});

            #region IP限流
            // https://marcus116.blogspot.com/2019/06/netcore-aspnet-core-webapi-aspnetcoreratelimit-throttle.html

            // 将速限计数器资料储存在 Memory 中
            services.AddMemoryCache();

            // 从 appsettings.json 读取 IpRateLimiting 设置
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            // 从 appsettings.json 读取 Ip Rule 设置
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            // 注入 counter and IP Rules
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            // the clientId/ clientIp resolvers use it.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Rate Limit configuration 设置
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // 配置HTTP请求管道
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseForwardedHeaders();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // http重定向到https
            //app.UseHttpsRedirection();

            // 启用限流,需在UseMvc前面
            app.UseIpRateLimiting();

            app.UseWeChatMiniProgram(); //注册微信小程序登陆中间件

            // 允许跨域
            app.UseCors("cors");

            // app.UseMiddleware<RefererMiddleware>(); // 判断Referer请求来源是否合法
            // app.UseMiddleware<ExceptionMiddleware>(); // 全局异常过滤
            app.UseRouting();

            // 先开启认证
            app.UseAuthentication();

            // 后开启授权
            app.UseAuthorization();

            // 添加请求日志中间件
            app.UseSerilogRequestLogging();

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //要在应用的根(http://localhost:<port>/) 处提供 Swagger UI，请将 RoutePrefix 属性设置为空字符串
                c.RoutePrefix = string.Empty;
                //swagger集成auth验证
            });
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Autofac注册
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ModuleRegister());
        }
    }
}
