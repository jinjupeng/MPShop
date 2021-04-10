using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 微信小程序用户登录
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MPUserController : ControllerBase
    {

        [HttpPost]
        [Route("wexin-login1")]
        public async Task<IActionResult> wexin_login1([FromBody] object obj)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(obj));
            var code = dict["code"];
            var token = "";
            return Ok(await Task.FromResult(token));
        }

        /// <summary>
        /// 正规的小程序登录方式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("wexin-login2")]
        //public async Task<IActionResult> wexin_login2([FromBody] object obj)
        //{
        //    var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(obj));
        //    var code = Convert.ToString(dict["code"]);
        //    var userInfo = Convert.ToString(dict["userInfo"]);
        //    var encryptedData = Convert.ToString(dict["encryptedData"]);
        //    var iv = Convert.ToString(dict["iv"]);
        //    var sessionKeyIsValid = Convert.ToBoolean(dict["sessionKeyIsValid"]);

        //    // 如果客户端有token，则传来，解析
        //    if (sessionKeyIsValid)
        //    {
        //        var token = 
        //    }

        //    return Ok(await Task.FromResult(msg));
        //}
    }
}
