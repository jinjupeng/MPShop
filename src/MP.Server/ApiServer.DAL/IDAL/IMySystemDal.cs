using ApiServer.Model.Entity;
using ApiServer.Model.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiServer.DAL.IDAL
{
    public interface IMySystemDal
    {
        IQueryable<sys_org> SelectOrgTree(long rootOrgId, string orgNameLike, bool? orgStatus);

        IQueryable<sys_menu> SelectMenuTree(long rootMenuId, string menuNameLike, bool? menuStatus);

        IQueryable<sys_api> SelectApiTree(long rootApiId, string apiNameLike, bool apiStatus);

        int InsertRoleMenuIds(int roleId, List<int> checkedIds);

        int InsertRoleApiIds(int roleId, List<int> checkedIds);

        IQueryable<string> SelectApiExpandedKeys();

        IQueryable<string> SelectMenuExpandedKeys();

        IQueryable<string> SelectApiCheckedKeys(long roleId);

        IQueryable<string> SelectMenuCheckedKeys(long roleId);

        IQueryable<string> GetCheckedRoleIds(long userId);

        int InsertUserRoleIds(int userId, List<int> checkedIds);

        IQueryable<sys_menu> SelectMenuByUserName(string userName);

        IQueryable<SysUserOrg> SelectUser(long? orgId,
                                      string userName,
                                      string phone,
                                      string email,
                                      bool? enabled,
                                      DateTime? createStartTime,
                                      DateTime? createEndTime);

    }
}
