namespace ApiServer.Model.Model.ViewModel
{
    public class SysOrg
    {
        public long id { get; set; }

        /// <summary>
        /// 上级组织编码
        /// </summary>
        public long orgPid { get; set; }

        /// <summary>
        /// 所有的父节点id
        /// </summary>
        public string orgPids { get; set; }

        /// <summary>
        /// 0:不是叶子节点，1:是叶子节点
        /// </summary>
        public bool isLeaf { get; set; }

        /// <summary>
        /// 组织名
        /// </summary>
        public string orgName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? sort { get; set; }

        /// <summary>
        /// 组织层级
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// 0:启用,1:禁用
        /// </summary>
        public bool status { get; set; }
    }
}
