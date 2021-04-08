using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ApiServer.Extensions.Middlewares
{
    /// <summary>
    /// 对请求头Referer判断过滤
    /// </summary>
    public class RefererMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        public RefererMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 对referer来源判断
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var applicationUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}";
            var headers = httpContext.Request.Headers;
            var urlReferer = headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(urlReferer) && !urlReferer.StartsWith(applicationUrl))
            {
                var payload = JsonConvert.SerializeObject(new { code = 0, msg = "请求来源不合法！" });
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status200OK;
                // httpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                // 输出Json数据结果
                await httpContext.Response.WriteAsync(payload);
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}