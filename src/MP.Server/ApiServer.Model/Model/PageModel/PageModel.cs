using System.Collections.Generic;

namespace ApiServer.Model.Model.PageModel
{
    /// <summary>
    /// 通用分页信息类
    /// </summary>
    public class PageModel<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int pageNum { get; set; } = 1;

        /// <summary>
        /// 每页数量
        /// </summary>
        public int pageSize { get; set; } = 10;

        /// <summary>
        /// 总记录数
        /// </summary>
        public int total { get; set; } = 0;

        /// <summary>
        /// 当前页的数量
        /// </summary>
        public int size { set; get; }

        /// <summary>
        /// 结果集
        /// </summary>
        public List<T> records { get; set; }

    }
}
