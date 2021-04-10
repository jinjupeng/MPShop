

namespace ApiServer.Common.MiniProgram
{
    public class LoginResult : WechatResult
    {
        public string openid { get; set; }
        public string session_key { get; set; }
        public string unionid { get; set; }
    }
}
