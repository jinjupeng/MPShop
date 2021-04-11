using ApiServer.BLL.IBLL;
using ApiServer.Model.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IBaseService<order> _baseService;

        public OrderController(IBaseService<order> baseService)
        {
            _baseService = baseService;
        }

        /// <summary>
        /// 下新订单，准备支付
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("my/order")]
        public async Task<IActionResult> Order()
        {
            return Ok(await Task.FromResult(""));
        }
    }
}
