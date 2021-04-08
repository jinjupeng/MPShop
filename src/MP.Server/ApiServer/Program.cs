﻿using ApiServer.Common.Config;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace ApiServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.Debug() // 配置日志输出到控制台
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                //.Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())

            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel((context, options) =>
                {
                    options.Configure(context.Configuration.GetSection("Kestrel"));
                })
                //设置监听的端口
                //.UseUrls(ConfigTool.Configuration["Setting:ListenUrl"])
                .UseStartup<Startup>()
                // 将Serilog设置为日志提供程序
                .UseSerilog(); // Add this line;
            });
    }
}
