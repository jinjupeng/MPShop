using ApiServer.BLL.IBLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiServer.Extensions.Auth
{
    /// <summary>
    /// 权限授权处理器
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ISysApiService _sysApiService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="sysApiService"></param>
        public PermissionHandler(IHttpContextAccessor accessor, ISysApiService sysApiService)
        {
            this._accessor = accessor;
            _sysApiService = sysApiService;
        }

        /// <summary>
        /// 常用自定义验证策略
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // 赋值用户权限，也可直接从数据库获取
            var userPermissions = _sysApiService.GetAllApiOfRole();
            var httpContext = _accessor.HttpContext;

            //获取请求头部信息token
            var result = httpContext.Request.Headers.TryGetValue("Authorization", out StringValues authStr);
            //判断token是否为空
            if (!result || string.IsNullOrEmpty(authStr.ToString()))
            {
                return Task.CompletedTask;
            }

            // 请求Url
            var questUrl = httpContext.Request.Path.Value.ToLower().Replace("/api", "");
            // 是否经过验证
            var isAuthenticated = httpContext.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                if (userPermissions.GroupBy(g => g.Url).Any(w => w.Key.ToLower() == questUrl))
                {
                    // 用户名
                    var userName = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Name)?.Value;
                    // 角色
                    var userRole = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Role)?.Value;
                    if (userPermissions.Any(w => w.Role == userRole && w.Url.ToLower() == questUrl))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        // 无权限跳转到拒绝页面
                        httpContext.Response.Redirect(requirement.DeniedAction);
                    }
                }
                else
                {
                    context.Fail();
                }
            }
            return Task.CompletedTask;
        }
    }
}
