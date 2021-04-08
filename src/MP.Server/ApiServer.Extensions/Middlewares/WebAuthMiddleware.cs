using ApiServer.BLL.IBLL;
using ApiServer.Common.Config;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ApiServer.Extensions.Middlewares
{
    public class WebAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWxService _wxService;
        private readonly string appId = ConfigTool.Configuration["wxmini:appid"];
        private readonly string appSecret = ConfigTool.Configuration["wxmini:secret"];
        private readonly string REBACK_URL = "/apis/backend";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        /// <param name="wxService"></param>
        public WebAuthMiddleware(RequestDelegate next, IWxService wxService)
        {
            _next = next;
            _wxService = wxService;
        }

        /// <summary>
        /// 处理网页授权的路由中间件
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext ctx)
        {
            var openid = ctx.Request.Cookies["user_openid"];
            if (string.IsNullOrEmpty(openid))
            {
                // 先看看是不是跳转回来的
                var queryCollection = ctx.Request.Query;
                var code = queryCollection["code"];
                var state = queryCollection["state"]; ;
                // 如果是跳转来的，处理跳转回来的逻辑
                if (!string.IsNullOrEmpty(code))
                {
                    var userinfoRange = state == "userinfo";
                    var res = await _wxService.GetOpenIdAsync(code);
                    openid = res["openid"];
                    if (string.IsNullOrEmpty(openid))
                    {
                        ctx.Response.StatusCode = 301;
                        ctx.Response.Redirect($"{ctx.Request.Host}{REBACK_URL}");
                    }
                    if (userinfoRange)
                    {
                        // 如果拉取全部用户信息，更新数据库中的用户信息，没有则先创建
                        res = await _wxService.GetUserInfoAsync(res["access_token"], res["openid"]);

                    }
                }
            }
        }
    }
}
