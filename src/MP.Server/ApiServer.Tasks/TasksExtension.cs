using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using System;

namespace ApiServer.Tasks
{
    public static class TasksExtension
    {
        /// <summary>
        /// 任务调度
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTaskSchedulers(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IJobFactory, JobFactory>();

            services.AddSingleton<ITaskSchedulerServer, TaskSchedulerServer>();

            return services;
        }
    }
}
