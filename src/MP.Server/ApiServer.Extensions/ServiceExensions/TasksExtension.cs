using ApiServer.BLL.IBLL;
using ApiServer.Model.Entity;
using ApiServer.Tasks;
using Microsoft.AspNetCore.Builder;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ApiServer.Extensions.ServiceExensions
{
    public static class TasksExtension
    {
        public static IApplicationBuilder UseAddTaskSchedulers(this IApplicationBuilder app)
        {
            IServiceProvider services = app.ApplicationServices;
            IBaseService<sys_tasks> _tasksQzService = services.GetService<IBaseService<sys_tasks>>();
            ITaskSchedulerServer _schedulerServer = services.GetService<ITaskSchedulerServer>();

            var tasks = _tasksQzService.GetModels(m => m.IsStart).ToList();

            foreach (var task in tasks)
            {
                _schedulerServer.AddTaskScheduleAsync(task);
            }

            return app;
        }
    }
}
