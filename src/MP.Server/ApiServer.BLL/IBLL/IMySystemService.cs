using ApiServer.Model.Entity;
using ApiServer.Model.Model.MsgModel;
using System;
using System.Collections.Generic;

namespace ApiServer.BLL.IBLL
{
    public interface IMySystemService
    {
        List<sys_org> SelectOrgTree(long rootOrgId, string orgNameLike, bool? orgStatus);

        List<sys_menu> SelectMenuTree(long rootMenuId, string menuNameLike, bool? menuStatus);

        List<sys_api> SelectApiTree(long rootApiId, string apiNameLike, bool apiStatus);

        int InsertRoleMenuIds(long roleId, List<long> checkedIds);

        int InsertRoleApiIds(long roleId, List<long> checkedIds);

        List<string> SelectApiExpandedKeys();

        List<string> SelectMenuExpandedKeys();

        List<string> SelectApiCheckedKeys(long roleId);

        List<string> SelectMenuCheckedKeys(long roleId);

        List<string> GetCheckedRoleIds(long userId);

        long InsertUserRoleIds(long userId, List<long> checkedIds);

        List<sys_menu> SelectMenuByUserName(string userName);
        MsgModel SelectUser(int pageIndex, int pageSize, long? orgId, string userName, string phone, string email, bool? enabled, DateTime? createStartTime, DateTime? createEndTime);
    }
}
