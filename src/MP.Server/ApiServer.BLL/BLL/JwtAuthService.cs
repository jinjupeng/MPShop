using ApiServer.BLL.IBLL;
using ApiServer.Common;
using ApiServer.Common.Attributes;
using ApiServer.Common.Auth;
using ApiServer.Model.Entity;
using ApiServer.Model.Model.MsgModel;
using ApiServer.Model.Model.ViewModel;
using Mapster;
using System.Collections.Generic;
using System.Linq;

namespace ApiServer.BLL.BLL
{
    public class JwtAuthService : IJwtAuthService
    {
        private readonly IBaseService<sys_user> _baseService;

        public JwtAuthService(IBaseService<sys_user> baseService)
        {
            _baseService = baseService;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public MsgModel Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return MsgModel.Fail("用户名或密码为空！");
            }
            // 加密登陆密码
            string encodePassword = PasswordEncoder.Encode(password);

            sys_user sys_user = _baseService.GetModels(a => a.username == username && a.password == encodePassword).SingleOrDefault();
            if (sys_user == null)
            {
                return MsgModel.Fail("用户名或密码不正确！");
            }
            else if (sys_user.enabled == false)
            {
                return MsgModel.Fail("账户已被禁用！");
            }

            // 将一些个人数据写入token中
            var dict = new Dictionary<string, object>
                {
                    { ClaimAttributes.UserId, sys_user.id },
                    { ClaimAttributes.UserName, username }
                };

            var data = JwtHelper.IssueJwt(dict);
            return MsgModel.Success((object)data);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public MsgModel SignUp(SysUser user)
        {
            var dict = new Dictionary<string, object>();
            var stringRandom = CommonUtils.GetStringRandom(10);
            user.username = stringRandom;
            //user.nickname = stringRandom;
            if (user.phone != null)
            {
                var queryUser = _baseService.GetModels(a => a.phone == user.phone).SingleOrDefault();
                if (queryUser == null)
                {
                    var sysUser = new sys_user();
                    sysUser = user.BuildAdapter().AdaptToType<sys_user>();
                    _baseService.AddRange(sysUser);
                    var playLoad = new Dictionary<string, object>
                    {
                        { ClaimAttributes.UserId, queryUser.id },
                        { ClaimAttributes.UserName, queryUser.username }
                    };

                    var token = JwtHelper.IssueJwt(playLoad);
                    dict.Add("token", token);
                    return MsgModel.Success(dict);
                }
                else
                {
                    var userDto = new SysUser();
                    userDto = queryUser.BuildAdapter().AdaptToType<SysUser>();
                    // 用户存在直接登录
                    return Login(userDto.username, userDto.password);
                }
            }
            else
            {
                return MsgModel.Fail("参数格式错误！");
            }
        }
    }
}
