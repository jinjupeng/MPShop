using ApiServer.BLL.IBLL;
using ApiServer.Common.Result;
using ApiServer.Common.Utils;
using ApiServer.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        private readonly IBaseService<goods> _baseService;
        private readonly IBaseService<goods_category> _goodsCategoryService;
        private readonly IBaseService<goods_sku> _goodsSkuService;
        private readonly IBaseService<goods_attr_key> _goodsAttrKeyService;
        private readonly IBaseService<goods_info> _goodsInfoService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseService"></param>
        /// <param name="goodsCategoryService"></param>
        /// <param name="goodsSkuService"></param>
        /// <param name="goodsAttrKeyService"></param>
        /// <param name="goodsInfoService"></param>
        public GoodsController(IBaseService<goods> baseService, IBaseService<goods_category> goodsCategoryService,
            IBaseService<goods_sku> goodsSkuService, IBaseService<goods_attr_key> goodsAttrKeyService,
            IBaseService<goods_info> goodsInfoService)
        {
            _baseService = baseService;
            _goodsCategoryService = goodsCategoryService;
            _goodsSkuService = goodsSkuService;
            _goodsAttrKeyService = goodsAttrKeyService;
            _goodsInfoService = goodsInfoService;
        }

        /// <summary>
        /// 获取分类集合
        /// </summary>
        /// <returns></returns>
        [Route("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _goodsCategoryService.SelectAsync();
            return Ok(await Task.FromResult(ResultModel.Success(categories)));
        }

        /// <summary>
        /// 获取产品集合
        /// </summary>
        /// <param name="category_id"></param>
        /// <param name="page_index"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        [Route("goods")]
        public async Task<IActionResult> GetGoods(int category_id, int page_index = 1, int page_size = 20)
        {
            var goods = _baseService.QueryByPage(page_index, page_size, a => a.category_id == category_id, _ => _.id);
            var goodsInfoList = _goodsInfoService.SelectAsync();
            var resultList = new List<Dictionary<string, object>>();
            foreach(var item in goods.records)
            {
                var dict = MapUtils.ObjectToMap(item);
                var goodsInfo = goodsInfoList.Result.Where(a => a.goods_id == item.id).ToList();
                dict.Add("goods_infos", goodsInfo);
                resultList.Add(dict);
            }
            var resultDict = new Dictionary<string, object>
            {
                { "count", goods.total },
                { "rows", resultList }
            };

            return Ok(await Task.FromResult(ResultModel.Success(resultDict)));
        }

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <returns></returns>
        [Route("goods/{goodsId}")]
        public async Task<IActionResult> GetGood(int goodsId)
        {
            var goods = await _baseService.SelectAsync(a => a.id == goodsId);

            return Ok(await Task.FromResult(ResultModel.Success(goods)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("goods/{goodsId}/sku")]
        public async Task<IActionResult> GetGoodSku(int goodsId)
        {
            var goodsSku = await _goodsSkuService.SelectAsync(a => a.goods_id == goodsId);
            var goodsAttrKeys = await _goodsAttrKeyService.SelectAsync(a => a.goods_id == goodsId);

            return Ok(await Task.FromResult(""));
        }

        // 获取轮播图产品信息

    }
}
