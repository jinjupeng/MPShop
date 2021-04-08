using ApiServer.Model.Entity;
using ApiServer.Model.Model.MsgModel;
using System.Collections.Generic;

namespace ApiServer.BLL.IBLL
{
    public interface ISysMenuService
    {
        MsgModel GetMenuTree(string menuNameLike, bool? menuStatus);
        MsgModel UpdateMenu(sys_menu sys_menu);
        MsgModel AddMenu(sys_menu sys_menu);
        MsgModel DeleteMenu(sys_menu sys_menu);
        List<string> GetCheckedKeys(long roleId);
        List<string> GetExpandedKeys();
        MsgModel SaveCheckedKeys(long roleId, List<long> checkedIds);
        MsgModel GetMenuTreeByUsername(string username);
        MsgModel UpdateStatus(long id, bool status);
    }
}
