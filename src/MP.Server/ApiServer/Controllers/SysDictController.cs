using ApiServer.BLL.IBLL;
using ApiServer.Model.Entity;
using ApiServer.Model.Model.ViewModel;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 系统数据字典配置控制层代码
    /// </summary>
    [Route("api/[controller]")]
    public class SysDictController : BaseController
    {
        private readonly ISysDictService _sysDictService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysDictService"></param>
        public SysDictController(ISysDictService sysDictService)
        {
            _sysDictService = sysDictService;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns>所有数据字典项</returns>
        [HttpPost]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            return Ok(await Task.FromResult(_sysDictService.All()));
        }

        /// <summary>
        /// 根据查询参数查询数据字典
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <param name="groupCode">分组编码</param>
        /// <returns>数据字典项列表</returns>
        [HttpPost]
        [Route("query")]
        public async Task<IActionResult> Query([FromForm] string groupName, string groupCode)
        {
            return Ok(await Task.FromResult(_sysDictService.Query(groupName, groupCode)));
        }

        /// <summary>
        /// 根据id更新数据数据字典项目
        /// </summary>
        /// <param name="sysDict">更新实体（必须包含id）</param>
        /// <returns>更新成功结果</returns>
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] SysDict sysDict)
        {
            var sys_Dict = sysDict.BuildAdapter().AdaptToType<sys_dict>();
            return Ok(await Task.FromResult(_sysDictService.Update(sys_Dict)));

        }

        /// <summary>
        /// 新增数据字典项
        /// </summary>
        /// <param name="sysDict">新增实体</param>
        /// <returns>更新成功结果</returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] SysDict sysDict)
        {
            var sys_Dict = sysDict.BuildAdapter().AdaptToType<sys_dict>();
            return Ok(await Task.FromResult(_sysDictService.Add(sys_Dict)));

        }

        /// <summary>
        /// 根据id删除数据字典项
        /// </summary>
        /// <param name="id">删除项id</param>
        /// <returns>删除成功结果</returns>
        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromForm] long id)
        {
            return Ok(await Task.FromResult(_sysDictService.Delete(id)));

        }
    }
}
