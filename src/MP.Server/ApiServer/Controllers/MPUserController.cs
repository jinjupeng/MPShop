using ApiServer.BLL.IBLL;
using ApiServer.Common;
using ApiServer.Common.Result;
using ApiServer.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
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
        public readonly IBaseService<mp_user> _baseService;

        public MPUserController(IBaseService<mp_user> baseService)
        {
            _baseService = baseService;
        }

        #region 小程序端接口
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

        #endregion

        #region PC端接口

        /// <summary>
        /// 查询小程序用户
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pc/mpuser/query")]
        public async Task<IActionResult> GetMPUser([FromBody] Dictionary<string, string> pairs)
        {
            string nickname = pairs["nickname"];
            int pageIndex = Convert.ToInt32(pairs["pageNum"]);
            int pageSize = Convert.ToInt32(pairs["pageSize"]);
            var where = PredicateBuilder.True<mp_user>();
            if (!string.IsNullOrWhiteSpace(nickname))
            {
                where = where.And(a => a.nickName.Contains(nickname));
            }
            var result = _baseService.QueryByPage(pageIndex, pageSize, where, _ => _.id);
            return Ok(await Task.FromResult(ResultModel.Success(result)));
        }


        /// <summary>
        /// 用户管理：删除
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pc/mpuser/delete")]
        public async Task<IActionResult> Delete([FromForm] long userId)
        {
            var result = await _baseService.RemoveAsync(a => a.id == userId);
            return Ok(await Task.FromResult(ResultModel.Result(result)));
        }

        #endregion
    }
}
