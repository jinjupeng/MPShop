namespace ApiServer.Model.Model.ViewModel
{
    public class SysApi
    {
        public long id { get; set; }

        /// <summary>
        /// 接口父ID(即接口分组)
        /// </summary>
        public long apiPid { get; set; }

        /// <summary>
        /// 当前接口的所有上级id(即所有上级分组)
        /// </summary>
        public string apiPids { get; set; }

        /// <summary>
        /// 0:不是叶子节点，1:是叶子节点
        /// </summary>
        public bool isLeaf { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string apiName { get; set; }

        /// <summary>
        /// 跳转URL
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? sort { get; set; }

        /// <summary>
        /// 层级，1：接口分组，2：接口
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// 是否禁用，0:启用(否）,1:禁用(是)
        /// </summary>
        public bool status { get; set; }
    }
}
