using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Auth.WeChat.MiniProgram
{
    public class Const
    {
        public const string LoginEndpoint = "/wechat-miniProgram-login";
        public const string OpenIdEndpoint = "sns/jscode2session";
        public const string HttpClientBaseAddress = "https://api.weixin.qq.com/";
        public const string HttpClientName = "miniProgram";
        public const string RootConfigKey = "miniProgram";
    }
}
