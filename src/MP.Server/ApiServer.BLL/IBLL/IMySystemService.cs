using ApiServer.Model.Entity;
using ApiServer.Model.Model.MsgModel;
using System;
using System.Collections.Generic;

namespace ApiServer.BLL.IBLL
{
    public interface IMySystemService
    {
        List<sys_org> SelectOrgTree(int rootOrgId, string orgNameLike, bool? orgStatus);

        List<sys_menu> SelectMenuTree(int rootMenuId, string menuNameLike, bool? menuStatus);

        List<sys_api> SelectApiTree(int rootApiId, string apiNameLike, bool apiStatus);

        int InsertRoleMenuIds(int roleId, List<int> checkedIds);

        int InsertRoleApiIds(int roleId, List<int> checkedIds);

        List<string> SelectApiExpandedKeys();

        List<string> SelectMenuExpandedKeys();

        List<string> SelectApiCheckedKeys(int roleId);

        List<string> SelectMenuCheckedKeys(int roleId);

        List<string> GetCheckedRoleIds(int userId);

        int InsertUserRoleIds(int userId, List<int> checkedIds);

        List<sys_menu> SelectMenuByUserName(string userName);
        MsgModel SelectUser(int pageIndex, int pageSize, int? orgId, string userName, string phone, string email, bool? enabled, DateTime? createStartTime, DateTime? createEndTime);
    }
}
