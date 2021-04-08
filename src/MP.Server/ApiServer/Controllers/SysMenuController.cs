using ApiServer.BLL.IBLL;
using ApiServer.Model.Entity;
using ApiServer.Model.Model;
using ApiServer.Model.Model.MsgModel;
using ApiServer.Model.Model.Nodes;
using ApiServer.Model.Model.ViewModel;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    [Route("api/[controller]")]
    public class SysMenuController : BaseController
    {
        private readonly ISysMenuService _sysMenuService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysMenuService"></param>
        public SysMenuController(ISysMenuService sysMenuService)
        {
            _sysMenuService = sysMenuService;
        }

        /// <summary>
        /// 菜单管理：查询
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("tree")]
        public async Task<IActionResult> Tree([FromForm] Dictionary<string, string> pairs)
        {
            string menuNameLike = (string)pairs["menuNameLike"];
            bool? menuStatus = null;
            if (!string.IsNullOrWhiteSpace(pairs["menuStatus"]))
            {
                menuStatus = Convert.ToBoolean(pairs["menuStatus"]);
            }
            return Ok(await Task.FromResult(_sysMenuService.GetMenuTree(menuNameLike, menuStatus)));
        }

        /// <summary>
        /// 菜单管理：修改
        /// </summary>
        /// <param name="sysMenu"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] SysMenu sysMenu)
        {
            var sys_menu = sysMenu.BuildAdapter().AdaptToType<sys_menu>();
            return Ok(await Task.FromResult(_sysMenuService.UpdateMenu(sys_menu)));

        }

        /// <summary>
        /// 菜单管理：新增
        /// </summary>
        /// <param name="sysMenu"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] SysMenu sysMenu)
        {
            var sys_menu = sysMenu.BuildAdapter().AdaptToType<sys_menu>();
            return Ok(await Task.FromResult(_sysMenuService.AddMenu(sys_menu)));

        }

        /// <summary>
        /// 菜单管理：删除
        /// </summary>
        /// <param name="sysMenu"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] SysMenu sysMenu)
        {
            var sys_menu = sysMenu.BuildAdapter().AdaptToType<sys_menu>();
            return Ok(await Task.FromResult(_sysMenuService.DeleteMenu(sys_menu)));

        }

        /// <summary>
        /// 角色管理:菜单树展示（勾选项、展开项）
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("checkedtree")]
        public async Task<IActionResult> CheckedTree([FromForm] long roleId)
        {
            MsgModel msg = new MsgModel
            {
                isok = true,
                message = "获取成功！"
            };
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "tree", _sysMenuService.GetMenuTree("", default).data },
                { "expandedKeys", _sysMenuService.GetExpandedKeys() },
                { "checkedKeys", _sysMenuService.GetCheckedKeys(roleId) }
            };
            msg.data = dict;
            return Ok(await Task.FromResult(msg));

        }

        /// <summary>
        /// 角色管理：保存菜单勾选结果
        /// </summary>
        /// <param name="roleCheckedIds"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("savekeys")]
        public async Task<IActionResult> SaveKeys([FromBody] RoleCheckedIds roleCheckedIds)
        {
            return Ok(await Task.FromResult(_sysMenuService.SaveCheckedKeys(roleCheckedIds.RoleId, roleCheckedIds.CheckedIds)));

        }

        /// <summary>
        /// 系统左侧菜单栏加载，根据登录用户名加载它可以访问的菜单项
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("tree/user")]
        public async Task<IActionResult> UserTree([FromForm] string userName)
        {
            return Ok(await Task.FromResult(_sysMenuService.GetMenuTreeByUsername(userName))); ;

        }

        /// <summary>
        /// 菜单管理：更新菜单禁用状态
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("status/change")]
        public async Task<IActionResult> Update([FromForm] long menuId, bool status)
        {
            return Ok(await Task.FromResult(_sysMenuService.UpdateStatus(menuId, status)));

        }
    }
}
