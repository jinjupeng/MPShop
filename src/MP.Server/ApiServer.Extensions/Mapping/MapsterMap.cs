using ApiServer.Model.Entity;
using ApiServer.Model.Model;
using ApiServer.Model.Model.Nodes;
using ApiServer.Model.Model.ViewModel;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ApiServer.Extensions.Mapping
{
    /// <summary>
    /// Mapster注入
    /// </summary>
    public static class MapsterMap
    {
        /// <summary>
        /// 自定义扩展service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMapster(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;

            #region 返回前端实体类映射

            config.NewConfig<sys_config, SysConfig>().NameMatchingStrategy(NameMatchingStrategy.ToCamelCase);
            config.NewConfig<sys_dict, SysDict>().NameMatchingStrategy(NameMatchingStrategy.ToCamelCase);
            config.NewConfig<sys_menu, SysMenuNode>().NameMatchingStrategy(NameMatchingStrategy.ToCamelCase);
            config.NewConfig<sys_api, SysApiNode>().NameMatchingStrategy(NameMatchingStrategy.ToCamelCase);
            config.NewConfig<sys_role, SysRole>().NameMatchingStrategy(NameMatchingStrategy.ToCamelCase);
            config.NewConfig<sys_org, SysOrgNode>().NameMatchingStrategy(NameMatchingStrategy.ToCamelCase);
            #endregion

            #region 接收前端实体类映射

            config.NewConfig<SysUser, sys_user>().NameMatchingStrategy(NameMatchingStrategy.FromCamelCase);
            config.NewConfig<SysRole, sys_role>().NameMatchingStrategy(NameMatchingStrategy.FromCamelCase);
            config.NewConfig<SysOrg, sys_org>().NameMatchingStrategy(NameMatchingStrategy.FromCamelCase);
            config.NewConfig<SysMenu, sys_menu>().NameMatchingStrategy(NameMatchingStrategy.FromCamelCase);
            config.NewConfig<SysDict, sys_dict>().NameMatchingStrategy(NameMatchingStrategy.FromCamelCase);
            config.NewConfig<SysConfig, sys_config>().NameMatchingStrategy(NameMatchingStrategy.FromCamelCase);
            config.NewConfig<SysApi, sys_api>().NameMatchingStrategy(NameMatchingStrategy.FromCamelCase);

            #endregion

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}
