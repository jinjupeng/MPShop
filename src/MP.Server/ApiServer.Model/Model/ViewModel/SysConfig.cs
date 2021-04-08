using System;

namespace ApiServer.Model.Model.ViewModel
{
    public class SysConfig
    {
        public long id { get; set; }

        /// <summary>
        /// 参数名称(中文)
        /// </summary>
        public string paramName { get; set; }

        /// <summary>
        /// 参数唯一标识(英文及数字)
        /// </summary>
        public string paramKey { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string paramValue { get; set; }

        /// <summary>
        /// 参数描述备注
        /// </summary>
        public string paramDesc { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createTime { get; set; }
    }
}
