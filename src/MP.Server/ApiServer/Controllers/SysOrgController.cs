using ApiServer.BLL.IBLL;
using ApiServer.Model.Entity;
using ApiServer.Model.Model.ViewModel;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 部门管理
    /// </summary>
    [Route("api/[controller]")]
    public class SysOrgController : BaseController
    {
        private readonly ISysOrgService _sysOrgService;
        private readonly IBaseService<sys_user> _baseSysUserService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysOrgService"></param>
        /// <param name="baseSysUserService"></param>
        public SysOrgController(ISysOrgService sysOrgService, IBaseService<sys_user> baseSysUserService)
        {
            _sysOrgService = sysOrgService;
            _baseSysUserService = baseSysUserService;
        }

        /// <summary>
        /// 部门层级树
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("tree")]
        public async Task<IActionResult> Tree([FromForm] Dictionary<string, string> pairs)
        {
            string userName = (string)pairs["username"];
            string orgNameLike = (string)pairs["orgNameLike"];
            bool? orgStatus = null;
            if (!string.IsNullOrWhiteSpace(pairs["orgStatus"]))
            {
                orgStatus = Convert.ToBoolean(pairs["orgStatus"]);
            }
            sys_user sys_user = _baseSysUserService.GetModels(a => a.username == userName).SingleOrDefault();
            return Ok(await Task.FromResult(_sysOrgService.GetOrgTreeById(sys_user.org_id, orgNameLike, orgStatus)));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sysOrg"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] SysOrg sysOrg)
        {
            var sys_org = sysOrg.BuildAdapter().AdaptToType<sys_org>();
            return Ok(await Task.FromResult(_sysOrgService.UpdateOrg(sys_org)));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysOrg"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] SysOrg sysOrg)
        {
            var sys_org = sysOrg.BuildAdapter().AdaptToType<sys_org>();
            return Ok(await Task.FromResult(_sysOrgService.AddOrg(sys_org)));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysOrg"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] SysOrg sysOrg)
        {
            var sys_org = sysOrg.BuildAdapter().AdaptToType<sys_org>();
            return Ok(await Task.FromResult(_sysOrgService.DeleteOrg(sys_org)));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("status/change")]
        public async Task<IActionResult> Update([FromForm] long orgId, bool status)
        {
            return Ok(await Task.FromResult(_sysOrgService.UpdateStatus(orgId, status)));

        }
    }
}
