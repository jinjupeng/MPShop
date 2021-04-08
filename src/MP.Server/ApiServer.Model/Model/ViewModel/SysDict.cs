using System;

namespace ApiServer.Model.Model.ViewModel
{
    public class SysDict
    {
        public long id { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string groupName { get; set; }

        /// <summary>
        /// 分组编码
        /// </summary>
        public string groupCode { get; set; }

        /// <summary>
        /// 字典项名称
        /// </summary>
        public string itemName { get; set; }

        /// <summary>
        /// 字典项Value
        /// </summary>
        public string itemValue { get; set; }

        /// <summary>
        /// 字典项描述
        /// </summary>
        public string itemDesc { get; set; }

        /// <summary>
        /// 字典项创建时间
        /// </summary>
        public DateTime createTime { get; set; }
    }
}
