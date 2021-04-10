using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Auth.WeChat.Common
{
    public class AccessTokenResult : WechatResult
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒。目前是7200秒之内的值。
        /// </summary>
        public int expires_in { get; set; }
    }
}
