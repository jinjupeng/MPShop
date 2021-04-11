using ApiServer.Common.MiniProgram;
using Microsoft.AspNetCore.Http;

namespace ApiServer.Auth.Abstractions.LoginModels
{
    public class LoginContext
    {
        public HttpContext Context { get; set; }
        public LoginResult Token { get; set; }
        public Option Option { get; set; }

        public WeChatInfo WeChatInfo { get; set; }
    }
}
