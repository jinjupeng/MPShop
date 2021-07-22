﻿using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ApiServer.Tasks
{
    public class JobBase
    {
        /// <summary>
        /// 执行指定任务
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        public async Task<string> ExecuteJob(IJobExecutionContext context, Func<Task> job)
        {
            string jobHistory = $"Run Job [Id：{context.JobDetail.Key.Name}，Group：{context.JobDetail.Key.Group}]";

            try
            {
                var s = context.Trigger.Key.Name;
                //记录Job时间
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //执行任务
                await job();
                stopwatch.Stop();
                jobHistory += $"， Succeed ， Elapsed：{stopwatch.Elapsed.TotalMilliseconds} ms";
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new JobExecutionException(ex);
                //true  是立即重新执行任务 
                e2.RefireImmediately = true;
                jobHistory += $"， Fail ，Exception：{ex.Message}";
            }

            return jobHistory;
        }
    }
}
