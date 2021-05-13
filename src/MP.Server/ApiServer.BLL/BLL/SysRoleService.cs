using ApiServer.BLL.IBLL;
using ApiServer.Common;
using ApiServer.Common.Attributes;
using ApiServer.Model.Entity;
using ApiServer.Model.Enum;
using ApiServer.Model.Model.MsgModel;
using ApiServer.Model.Model.ViewModel;
using Mapster;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ApiServer.BLL.BLL
{
    public class SysRoleService : ISysRoleService
    {
        private readonly IBaseService<sys_role> _baseSysRoleService;
        private readonly IMySystemService _mySystemService;
        private readonly IBaseService<sys_user_role> _sysUserRoleService;
        private readonly IBaseService<sys_user> _sysUserService;

        public SysRoleService(IBaseService<sys_role> baseSysRoleService,
            IMySystemService mySystemService, IBaseService<sys_user_role> sysUserRoleService,
            IBaseService<sys_user> sysUserService)
        {
            _baseSysRoleService = baseSysRoleService;
            _mySystemService = mySystemService;
            _sysUserRoleService = sysUserRoleService;
            _sysUserService = sysUserService;
        }

        /// <summary>
        /// 根据用户名获取用户角色（目前只支持单用户单角色）
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string GetRoleByUserName(string userName)
        {
            string roleCode = string.Empty;
            try
            {
                sys_user sys_user = _sysUserService.GetModels(a => a.username == userName).SingleOrDefault();
                sys_user_role sys_user_role = _sysUserRoleService.GetModels(a => a.user_id == sys_user.id).SingleOrDefault();
                sys_role sys_role = _baseSysRoleService.GetModels(a => a.id == sys_user_role.role_id && a.status == false).SingleOrDefault();
                roleCode = sys_role.role_code;
            }
            catch (Exception ex)
            {
                throw new CustomException(500, "用户角色不存在或角色已被禁用");
            }

            return roleCode;
        }


        /// <summary>
        /// 根据参数查询角色记录
        /// </summary>
        /// <param name="roleLik">角色编码 或角色描述 或角色名称模糊查询</param>
        /// <returns>角色记录列表</returns>
        public MsgModel QueryRoles(string roleLik)
        {
            Expression<Func<sys_role, bool>> express = null;
            if (!string.IsNullOrWhiteSpace(roleLik))
            {
                express = a => a.role_code.Contains(roleLik) || a.role_desc.Contains(roleLik) || a.role_name.Contains(roleLik);
            }
            var sysRoleList = _baseSysRoleService.GetModels(express).ToList();
            var data = sysRoleList.BuildAdapter().AdaptToType<List<SysRole>>();
            return MsgModel.Success(data, "查询成功！");
        }

        public MsgModel UpdateRole(sys_role sys_role)
        {
            if (!_baseSysRoleService.Update(sys_role))
            {
                return MsgModel.Fail("角色更新失败！");
            }
            return MsgModel.Success("角色更新成功！");
        }

        public MsgModel AddRole(sys_role sys_role)
        {
            CustomException customException = new CustomException();
            sys_role.id = new Snowflake().GetId();
            sys_role.status = false;// 是否禁用:false
            if (_baseSysRoleService.GetModels(a => a.role_code == sys_role.role_code).Any())
            {
                customException.Code = (int)HttpStatusCode.Status500InternalServerError;

                return MsgModel.Fail(StatusCodes.Status500InternalServerError, "角色编码已存在，不能重复！");
            }
            if (!_baseSysRoleService.Insert(sys_role))
            {
                return MsgModel.Fail("新增角色失败！");
            }
            return MsgModel.Success("新增角色成功！");
        }

        public MsgModel DeleteRole(long id)
        {
            if (!_baseSysRoleService.DeleteRange(_baseSysRoleService.GetModels(a => a.id == id)))
            {
                return MsgModel.Fail("删除角色失败！");
            }
            return MsgModel.Success("删除角色成功！");
        }

        /// <summary>
        /// 获取角色记录及某用户勾选角色记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MsgModel GetRolesAndChecked(long userId)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                // 所有角色记录
                { "roleDatas", _baseSysRoleService.GetModels(a => a.status == false).ToList().BuildAdapter().AdaptToType<List<SysRole>>() },
                //某用户具有的角色id列表
                { "checkedRoleIds", _mySystemService.GetCheckedRoleIds(userId) }
            };
            return MsgModel.Success(dict, "查询成功！");
        }

        /// <summary>
        /// 保存某用户勾选的角色id数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="checkedIds"></param>
        [Transaction]
        public MsgModel SaveCheckedKeys(long userId, List<long> checkedIds)
        {
            _sysUserRoleService.DeleteRange(_sysUserRoleService.GetModels(a => a.user_id == userId).ToList());
            _mySystemService.InsertUserRoleIds(userId, checkedIds);
            return MsgModel.Success("用户角色保存成功！"); ;
        }

        /// <summary>
        /// 角色管理：更新角色的禁用状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public MsgModel UpdateStatus(long id, bool status)
        {
            sys_role sys_role = _baseSysRoleService.GetModels(a => a.id == id).SingleOrDefault();
            sys_role.status = status;
            bool result = _baseSysRoleService.Update(sys_role);

            return result ? MsgModel.Success("角色禁用状态更新成功！") : MsgModel.Fail("角色禁用状态更新失败！");
        }

    }
}
