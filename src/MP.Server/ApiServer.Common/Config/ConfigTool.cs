using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ApiServer.Common.Config
{
    /// <summary>
    /// 读取配置文件
    /// </summary>
    public class ConfigTool
    {
        public static IConfiguration Configuration { get; set; }
        static ConfigTool()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载            
            Configuration = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                .Build();

            //也可以使用：
            //Configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json").Build();
        }
    }
}
