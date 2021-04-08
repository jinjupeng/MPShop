using ApiServer.BLL.IBLL;
using ApiServer.Model.Model.WX;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WxController : ControllerBase
    {
        private readonly IWxService _wxService;

        public WxController(IWxService wxService)
        {
            _wxService = wxService;
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="wxAuth"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("authLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthLogin([FromBody] WXAuth wxAuth)
        {
            var result = _wxService.AuthLogin(wxAuth);
            return Ok(await Task.FromResult(result));
        }
    }
}
