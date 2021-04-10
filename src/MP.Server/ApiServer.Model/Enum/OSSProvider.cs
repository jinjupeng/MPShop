using System.ComponentModel;

namespace ApiServer.Model.Enum
{
    /// <summary>
    /// OSS提供器
    /// </summary>
    public enum OSSProvider
    {
        [Description("本地存储")]
        Local,
        /// <summary>
        /// 七牛
        /// </summary>
        [Description("七牛")]
        Qiniu,
        /// <summary>
        /// 阿里云
        /// </summary>
        [Description("阿里云")]
        Aliyun
    }
}
