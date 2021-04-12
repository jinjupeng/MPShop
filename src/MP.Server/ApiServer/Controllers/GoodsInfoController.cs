using ApiServer.BLL.IBLL;
using ApiServer.Common.Result;
using ApiServer.Model.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 商品详情
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsInfoController : ControllerBase
    {
        private readonly IBaseService<goods_info> _goodsInfoService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goodsInfoService"></param>
        public GoodsInfoController(IBaseService<goods_info> goodsInfoService)
        {
            _goodsInfoService = goodsInfoService;
        }

        /// <summary>
        /// 获取轮播图数据
        /// </summary>
        /// <returns></returns>
        [Route("GetCarousel")]
        public async Task<IActionResult> GetCarousel()
        {
            var goodsInfoList = await _goodsInfoService.SelectAsync(a => a.kind == 0);
            var groupByList = goodsInfoList.GroupBy(_ => _.goods_id).ToList();
            var carouselList = new List<goods_info>();
            foreach(var item in groupByList)
            {
                carouselList.Add(item.FirstOrDefault());
            }
            carouselList = carouselList.Where(a => a.goods_id < 5).ToList(); // 只显示某几条轮播信息
            return Ok(await Task.FromResult(ResultModel.Success(carouselList)));
        }
    }
}
