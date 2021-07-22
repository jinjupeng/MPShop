using ApiServer.BLL.BLL;
using ApiServer.BLL.IBLL;
using ApiServer.DAL.DAL;
using ApiServer.DAL.IDAL;
using ApiServer.DAL.UnitOfWork;
using ApiServer.Extensions.AOP;
using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiServer.Extensions.AutofacModule
{
    /// <summary>
    /// Module注册
    /// </summary>
    public class ModuleRegister : Module
    {
        /// <summary>
        /// https://cloud.tencent.com/developer/article/1511929
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            try
            {
                var assemblys = AppDomain.CurrentDomain.GetAssemblies();
                var interceptorServiceTypes = new List<Type>();

                // type对象注册
                builder.RegisterType<UnitOfWork>().As<IUnitOfWork>()
                    .AsImplementedInterfaces();

                // 注册拦截器，注意：不能在BaseService类中使用[Transaction]，因为无效
                builder.RegisterType<TransactionInterceptor>().AsSelf();
                interceptorServiceTypes.Add(typeof(TransactionInterceptor)); // 配置事务拦截器

                // 泛型注册
                builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IBaseService<>)).InstancePerDependency();

                // 批量注册 获取 Service 程序集服务，并注册，可以在实现方法中使用[Transaction]
                builder.RegisterAssemblyTypes(assemblys).InNamespace("ApiServer.BLL.BLL")
                    .Where(a => a.Name.EndsWith("Service"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope()
                    .EnableInterfaceInterceptors()//启用接口拦截
                    .InterceptedBy(interceptorServiceTypes.ToArray()); // 指定拦截器

                // 泛型注册
                builder.RegisterGeneric(typeof(BaseDal<>)).As(typeof(IBaseDal<>)).InstancePerDependency();

                // 批量注册 获取 Dal 程序集服务，并注册
                builder.RegisterAssemblyTypes(assemblys).InNamespace("ApiServer.DAL.DAL")
                    .Where(a => a.Name.EndsWith("Dal"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.InnerException);
            }
        }
    }
}
