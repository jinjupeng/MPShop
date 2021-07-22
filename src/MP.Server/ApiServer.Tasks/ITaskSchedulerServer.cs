using ApiServer.Model.Entity;
using Common.Utility.Models;
using System;
using System.Threading.Tasks;

namespace ApiServer.Tasks
{
	public interface ITaskSchedulerServer
	{
		Task<ResultModel<string>> StartTaskScheduleAsync();

		Task<ResultModel<string>> StopTaskScheduleAsync();

		Task<ResultModel<string>> AddTaskScheduleAsync(sys_tasks tasksQz);

		Task<ResultModel<string>> PauseTaskScheduleAsync(sys_tasks tasksQz);

		Task<ResultModel<string>> ResumeTaskScheduleAsync(sys_tasks tasksQz);

		Task<ResultModel<string>> DeleteTaskScheduleAsync(sys_tasks tasksQz);
	}
}
