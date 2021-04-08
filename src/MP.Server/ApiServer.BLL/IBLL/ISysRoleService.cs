using ApiServer.Model.Entity;
using ApiServer.Model.Model.MsgModel;
using System.Collections.Generic;

namespace ApiServer.BLL.IBLL
{
    public interface ISysRoleService
    {
        string GetRoleByUserName(string userName);
        MsgModel QueryRoles(string roleLik);

        MsgModel UpdateRole(sys_role sys_role);
        MsgModel AddRole(sys_role sys_role);
        MsgModel DeleteRole(long id);
        MsgModel GetRolesAndChecked(long userId);
        MsgModel SaveCheckedKeys(long userId, List<long> checkedIds);
        MsgModel UpdateStatus(long id, bool status);
    }
}
