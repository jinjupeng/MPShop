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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
        /// <returns></returns>
        [HttpPost]
        [Route("wexin-login2")]
        public async Task<IActionResult> wexin_login2()
        {
            return Ok(await Task.FromResult(""));
        }

        /// <summary>
        /// 我的购物车商品集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("my/carts")]
        public async Task<IActionResult> GetMyCarts(int user_id)
        {
            return Ok(await Task.FromResult(""));
        }

        /// <summary>
        /// 修改购物车数量
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("my/carts/{id}")]
        public async Task<IActionResult> UpdateMyCart()
        {
            return Ok(await Task.FromResult(""));
        }

        /// <summary>
        /// 删除购物车商品
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("my/carts")]
        public async Task<IActionResult> DelMyCart()
        {
            return Ok(await Task.FromResult(""));
        }

        /// <summary>
        /// 新增购物车商品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("my/carts")]
        public async Task<IActionResult> AddMyCart()
        {
            return Ok(await Task.FromResult(""));
        }

        /// <summary>
        /// 我的地址
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("my/address")]
        public async Task<IActionResult> GetMyAddress()
        {
            return Ok(await Task.FromResult(""));
        }

        /// <summary>
        /// 新增我的地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("my/address")]
        public async Task<IActionResult> AddMyAddress()
        {
            return Ok(await Task.FromResult(""));
        }

        /// <summary>
        /// 修改我的地址
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("my/address")]
        public async Task<IActionResult> UpdateMyAddress()
        {
            return Ok(await Task.FromResult(""));
        }

        /// <summary>
        /// 删除我的地址
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("my/address/{id}")]
        public async Task<IActionResult> DelMyAddress()
        {
            return Ok(await Task.FromResult(""));
        }
    }
}
