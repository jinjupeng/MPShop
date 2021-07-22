using System;

namespace ApiServer.Model.Entity
{
    ///<summary>
    ///任务调度
    ///</summary>
    public class sys_tasks
    {

        public string ID { get; set; }

        /// <summary>
        /// 描述 : 任务名称 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述 : 任务分组 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// 描述 : 运行时间表达式 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// 描述 : 程序集名称 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// 描述 : 任务所在类 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 描述 : 任务描述 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 描述 : 执行次数 
        /// 空值 : False
        /// 默认 : 0
        /// </summary>
        public int RunTimes { get; set; }

        /// <summary>
        /// 描述 : 开始时间 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 描述 : 结束时间 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 描述 : 触发器类型（0、simple 1、cron） 
        /// 空值 : False
        /// 默认 : 1
        /// </summary>
        public int TriggerType { get; set; }

        /// <summary>
        /// 描述 : 执行间隔时间(单位:秒) 
        /// 空值 : False
        /// 默认 : 0
        /// </summary>
        public int IntervalSecond { get; set; }

        /// <summary>
        /// 描述 : 是否启动 
        /// 空值 : False
        /// 默认 : 0
        /// </summary>
        public bool IsStart { get; set; }

        /// <summary>
        /// 描述 : 传入参数 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        public string JobParams { get; set; }

        /// <summary>
        /// 描述 : 创建时间 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 描述 : 最后更新时间 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 描述 : 创建人编码 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        public string CreateID { get; set; }

        /// <summary>
        /// 描述 : 创建人 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 描述 : 更新人编码 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        public string UpdateID { get; set; }

        /// <summary>
        /// 描述 : 更新人 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        public string UpdateName { get; set; }

    }
}
