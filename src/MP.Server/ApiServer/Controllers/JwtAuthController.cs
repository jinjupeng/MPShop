using ApiServer.BLL.IBLL;
using ApiServer.Common.Auth;
using ApiServer.Model.Model.MsgModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class JwtAuthController : BaseController
    {
        private readonly IJwtAuthService _jwtAuthService;
        private readonly ISysRoleService _sysRoleService;
        private readonly IHttpContextAccessor _accessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtAuthService"></param>
        /// <param name="sysRoleService"></param>
        /// <param name="accessor"></param>
        public JwtAuthController(IJwtAuthService jwtAuthService, ISysRoleService sysRoleService,
            IHttpContextAccessor accessor)
        {
            _jwtAuthService = jwtAuthService;
            _sysRoleService = sysRoleService;
            _accessor = accessor;
        }

        /// <summary>
        /// 使用用户名密码来换取jwt令牌
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("authentication")]
        public async Task<IActionResult> Login([FromBody] Dictionary<string, string> pairs)
        {
            var username = pairs["username"];
            var password = pairs["password"];
            var msg = _jwtAuthService.Login(username, password);
            return Ok(await Task.FromResult(msg));
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("refreshtoken")]
        public async Task<IActionResult> RefreshToken()
        {
            var httpContext = _accessor.HttpContext;
            //获取请求头部信息token
            var result = httpContext.Request.Headers.TryGetValue("Authorization", out StringValues oldToken);
            //判断token是否为空
            if (!result || !oldToken.ToString().StartsWith("ey"))
            {
                return Ok(await Task.FromResult(MsgModel.Fail(StatusCodes.Status401Unauthorized, "用户登录信息已失效，请重新登录！")));
            }
            string refreshToken = JwtHelper.RefreshToken(oldToken);
            return Ok(await Task.FromResult(MsgModel.Success((object)refreshToken)));
        }
    }
}
