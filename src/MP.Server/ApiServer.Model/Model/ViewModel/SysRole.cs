namespace ApiServer.Model.Model.ViewModel
{
    public class SysRole
    {
        public long id { get; set; }

        /// <summary>
        /// 角色名称(汉字)
        /// </summary>
        public string roleName { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string roleDesc { get; set; }

        /// <summary>
        /// 角色的英文code.如：ADMIN
        /// </summary>
        public string roleCode { get; set; }

        /// <summary>
        /// 角色顺序
        /// </summary>
        public int sort { get; set; }

        /// <summary>
        /// 0表示可用(false)
        /// </summary>
        public bool? status { get; set; }
    }
}
