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
        MsgModel DeleteRole(int id);
        MsgModel GetRolesAndChecked(int userId);
        MsgModel SaveCheckedKeys(int userId, List<int> checkedIds);
        MsgModel UpdateStatus(int id, bool status);
    }
}
