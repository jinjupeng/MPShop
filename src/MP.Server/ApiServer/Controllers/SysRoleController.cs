using ApiServer.BLL.IBLL;
using ApiServer.Model.Entity;
using ApiServer.Model.Model.Nodes;
using ApiServer.Model.Model.ViewModel;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [Route("api/[controller]")]
    public class SysRoleController : BaseController
    {
        private readonly ISysRoleService _sysRoleService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysRoleService"></param>
        public SysRoleController(ISysRoleService sysRoleService)
        {
            _sysRoleService = sysRoleService;
        }

        /// <summary>
        /// 角色管理:查询
        /// </summary>
        /// <param name="roleLike"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("query")]
        public async Task<IActionResult> Query([FromForm] string roleLike)
        {
            return Ok(await Task.FromResult(_sysRoleService.QueryRoles(roleLike)));
        }

        /// <summary>
        /// 角色管理：修改
        /// </summary>
        /// <param name="sysRole"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] SysRole sysRole)
        {
            var sys_role = sysRole.BuildAdapter().AdaptToType<sys_role>();
            return Ok(await Task.FromResult(_sysRoleService.UpdateRole(sys_role)));

        }

        /// <summary>
        /// 角色管理：新增
        /// </summary>
        /// <param name="sysRole"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] SysRole sysRole)
        {
            var sys_role = sysRole.BuildAdapter().AdaptToType<sys_role>();
            return Ok(await Task.FromResult(_sysRoleService.AddRole(sys_role)));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromForm] int roleId)
        {
            return Ok(await Task.FromResult(_sysRoleService.DeleteRole(roleId)));

        }

        /// <summary>
        /// 用户管理：为用户分配角色，展示角色列表及勾选角色列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("checkedroles")]
        public async Task<IActionResult> CheckedRoles([FromForm] int userId)
        {
            return Ok(await Task.FromResult(_sysRoleService.GetRolesAndChecked(userId)));
        }

        /// <summary>
        /// 用户管理：保存用户角色
        /// </summary>
        /// <param name="userRoleCheckedIds"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("savekeys")]
        public async Task<IActionResult> Savekeys([FromBody] UserRoleCheckedIds userRoleCheckedIds)
        {
            return Ok(await Task.FromResult(_sysRoleService.SaveCheckedKeys(userRoleCheckedIds.UserId, userRoleCheckedIds.CheckedIds)));
        }

        /// <summary>
        /// 角色管理：更新角色禁用状态
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("status/change")]
        public async Task<IActionResult> Update([FromForm] int roleId, bool status)
        {
            return Ok(await Task.FromResult(_sysRoleService.UpdateStatus(roleId, status)));
        }
    }
}
