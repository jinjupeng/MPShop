using ApiServer.Extensions.Validation;
using ApiServer.Model.Model.ViewModel;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiServer.Extensions.ServiceExensions
{
    public static class ValidationExtension
    {
        /// <summary>
        /// 添加FluentValidation
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IMvcBuilder AddValidation(this IMvcBuilder builder, IServiceCollection services)
        {
            builder.AddFluentValidation();

            services.AddTransient<IValidator<SysUser>, UserValidator>();
            return builder;
        }
    }
}
