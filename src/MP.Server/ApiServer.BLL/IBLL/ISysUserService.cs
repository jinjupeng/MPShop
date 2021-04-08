using ApiServer.Model.Entity;
using ApiServer.Model.Model.MsgModel;
using System;

namespace ApiServer.BLL.IBLL
{
    public interface ISysUserService
    {
        MsgModel GetUserByUserName(string userName);
        MsgModel UpdateUser(sys_user sys_user);
        MsgModel AddUser(sys_user sys_user);
        MsgModel DeleteUser(long userId);
        MsgModel PwdReset(long userId);
        MsgModel IsDefault(string userName);
        MsgModel ChangePwd(string userName, string oldPass, string newPass);
        MsgModel UpdateEnabled(long id, bool enabled);
        MsgModel QueryUser(long? orgId, string userName, string phone, string email, bool? enabled, DateTime? createStartTime, DateTime? createEndTime, int pageNum, int pageSize);
    }
}
