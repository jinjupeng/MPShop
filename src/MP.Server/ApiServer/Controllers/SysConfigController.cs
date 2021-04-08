using ApiServer.BLL.IBLL;
using ApiServer.Model.Entity;
using ApiServer.Model.Model.ViewModel;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("api/[controller]")]
    public class SysConfigController : BaseController
    {
        private readonly ISysConfigService _sysConfigService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysConfigService"></param>
        public SysConfigController(ISysConfigService sysConfigService)
        {
            _sysConfigService = sysConfigService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            return Ok(await Task.FromResult(_sysConfigService.GetSysConfigList()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh()
        {
            return Ok(await Task.FromResult(_sysConfigService.GetSysConfigList()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configLike"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("query")]
        public async Task<IActionResult> Query([FromForm] string configLike)
        {
            return Ok(await Task.FromResult(_sysConfigService.QueryConfigs(configLike)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysConfig"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] SysConfig sysConfig)
        {
            var sys_Config = sysConfig.BuildAdapter().AdaptToType<sys_config>();
            return Ok(await Task.FromResult(_sysConfigService.UpdateConfig(sys_Config)));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysConfig"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] SysConfig sysConfig)
        {
            var sys_Config = sysConfig.BuildAdapter().AdaptToType<sys_config>();
            return Ok(await Task.FromResult(_sysConfigService.AddConfig(sys_Config)));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromForm] long configId)
        {
            return Ok(await Task.FromResult(_sysConfigService.DeleteConfig(configId)));

        }
    }
}
