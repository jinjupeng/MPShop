using ApiServer.Auth.WeChat.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Auth.WeChat.MiniProgram
{
    public class LoginResult : WechatResult
    {
        public string openid { get; set; }
        public string session_key { get; set; }
        public string unionid { get; set; }
    }
}
