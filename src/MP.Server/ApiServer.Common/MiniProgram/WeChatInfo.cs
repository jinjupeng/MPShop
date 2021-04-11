using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Common.MiniProgram
{
    /// <summary>
    /// 获取登录用户信息
    /// </summary>
    public class WeChatInfo
    {
        public string code { get; set; }
        public string encryptedData { get; set; }
        public string iv { get; set; }
        public object userInfo { get; set; }
        public bool sessionKeyIsValid { get; set; }
    }
}
