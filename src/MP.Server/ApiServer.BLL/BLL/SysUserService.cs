using ApiServer.BLL.IBLL;
using ApiServer.Common;
using ApiServer.Model.Entity;
using ApiServer.Model.Model.MsgModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace ApiServer.BLL.BLL
{
    public class SysUserService : ISysUserService
    {
        private readonly IBaseService<sys_user> _baseSysUserService;
        private readonly IMySystemService _mySystemService;
        private readonly ISysConfigService _sysConfigService;

        public SysUserService(IBaseService<sys_user> baseSysUserService,
            IMySystemService mySystemService, ISysConfigService sysConfigService)
        {
            _baseSysUserService = baseSysUserService;
            _mySystemService = mySystemService;
            _sysConfigService = sysConfigService;
        }

        /// <summary>
        /// 根据登录用户名查询用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public MsgModel GetUserByUserName(string userName)
        {
            sys_user sysUser = _baseSysUserService.GetModels(a => a.username == userName).SingleOrDefault();
            if (sysUser != null)
            {
                sysUser.password = "";
            }

            return MsgModel.Success(sysUser);
        }

        /// <summary>
        /// 用户管理：查询
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="userName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="enabled"></param>
        /// <param name="createStartTime"></param>
        /// <param name="createEndTime"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public MsgModel QueryUser(int? orgId, string userName, string phone, string email, bool? enabled, DateTime? createStartTime, DateTime? createEndTime, int pageNum, int pageSize)
        {
            return _mySystemService.SelectUser(pageNum, pageSize, orgId, userName, phone, email, enabled, createStartTime, createEndTime);
        }


        /// <summary>
        /// 用户管理：修改
        /// </summary>
        /// <param name="sys_user"></param>
        public MsgModel UpdateUser(sys_user sys_user)
        {
            if (!_baseSysUserService.Update(sys_user))
            {
                return MsgModel.Fail(StatusCodes.Status500InternalServerError, "更新用户失败！");
            }
            return MsgModel.Success("更新用户成功");
        }

        /// <summary>
        /// 用户管理：新增
        /// </summary>
        /// <param name="sys_user"></param>
        public MsgModel AddUser(sys_user sys_user)
        {
            sys_user.password = PasswordEncoder.Encode(_sysConfigService.GetConfigItem("user.init.password"));
            sys_user.create_time = DateTime.Now; //创建时间
            sys_user.enabled = true;//新增用户激活
            if (_baseSysUserService.GetModels(a => a.username == sys_user.username).Any())
            {
                return MsgModel.Fail(StatusCodes.Status500InternalServerError, "用户名已存在，不能重复");
            }
            if (!_baseSysUserService.Insert(sys_user))
            {
                return MsgModel.Fail("新增用户失败！");
            }
            return MsgModel.Success("新增用户成功！");
        }

        /// <summary>
        /// 用户管理：删除
        /// </summary>
        /// <param name="userId"></param>
        public MsgModel DeleteUser(int userId)
        {
            if (!_baseSysUserService.DeleteRange(_baseSysUserService.GetModels(a => a.id == userId)))
            {
                return MsgModel.Fail(StatusCodes.Status500InternalServerError, "删除用户失败！");
            }
            return MsgModel.Success("删除用户成功！");
        }

        /// <summary>
        /// 用户管理：重置密码
        /// </summary>
        /// <param name="userId"></param>
        public MsgModel PwdReset(int userId)
        {
            sys_user sys_user = _baseSysUserService.GetModels(a => a.id == userId).ToList().SingleOrDefault();
            sys_user.id = userId;
            sys_user.password = PasswordEncoder.Encode(_sysConfigService.GetConfigItem("user.init.password"));
            var length = sys_user.password.Length;
            bool result = _baseSysUserService.Update(sys_user);
            if (!result)
            {
                return MsgModel.Fail(StatusCodes.Status500InternalServerError, "密码重置失败！");
            }
            return MsgModel.Success("密码重置成功！");
        }

        /// <summary>
        /// 判断当前登录的用户密码是否是默认密码，如果是，会让他去修改
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public MsgModel IsDefault(string userName)
        {
            sys_user sys_user = _baseSysUserService.GetModels(a => a.username == userName).SingleOrDefault();
            //判断数据库密码是否是默认密码
            var result = PasswordEncoder.IsMatch(sys_user.password, _sysConfigService.GetConfigItem("user.init.password"));
            //判断数据库密码是否是默认密码
            return MsgModel.Success(result, "获取成功！");
        }

        /// <summary>
        /// 个人中心：修改密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPass"></param>
        /// <param name="newPass"></param>
        public MsgModel ChangePwd(string userName, string oldPass, string newPass)
        {
            sys_user sys_user = _baseSysUserService.GetModels(a => a.username == userName).SingleOrDefault();
            // 判断旧密码是否正确
            bool isMatch = PasswordEncoder.IsMatch(sys_user.password, oldPass);
            if (!isMatch)
            {
                return MsgModel.Fail("原密码输入错误，请确认后重新输入！");
            }
            sys_user.password = PasswordEncoder.Encode(newPass);
            var result = _baseSysUserService.Update(sys_user);
            return result ? MsgModel.Success("密码修改成功！") : MsgModel.Fail("密码修改失败！");
        }

        /// <summary>
        /// 用户管理：更新用户的激活状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enabled"></param>
        public MsgModel UpdateEnabled(int id, bool enabled)
        {
            sys_user sys_user = _baseSysUserService.GetModels(a => a.id == id).SingleOrDefault();
            sys_user.enabled = enabled;
            bool result = _baseSysUserService.Update(sys_user);

            return result ? MsgModel.Success("用户状态更新成功！") : MsgModel.Fail("用户状态更新失败！");

        }
    }
}
