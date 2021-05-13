namespace ApiServer.Model.Model.ViewModel
{
    public class SysMenu
    {
        public int id { get; set; }

        /// <summary>
        /// 父菜单ID
        /// </summary>
        public int menuPid { get; set; }

        /// <summary>
        /// 当前菜单所有父菜单
        /// </summary>
        public string menuPids { get; set; }

        /// <summary>
        /// 0:不是叶子节点，1:是叶子节点
        /// </summary>
        public bool isLeaf { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string menuName { get; set; }

        /// <summary>
        /// 跳转URL
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? sort { get; set; }

        /// <summary>
        /// 菜单层级
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// 0:启用,1:禁用
        /// </summary>
        public bool status { get; set; }
    }
}
