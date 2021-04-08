using ApiServer.Model.Entity;
using ApiServer.Model.Model;
using ApiServer.Model.Model.AuthModel;
using ApiServer.Model.Model.MsgModel;
using System.Collections.Generic;

namespace ApiServer.BLL.IBLL
{
    public interface ISysApiService
    {
        List<PermissionItem> GetAllApiOfRole();
        MsgModel GetApiTreeById(string apiNameLike, bool apiStatus);

        MsgModel UpdateApi(sys_api sys_Api);

        MsgModel AddApi(sys_api sys_Api);

        MsgModel DeleteApi(sys_api sys_Api);

        List<string> GetCheckedKeys(long roleId);

        List<string> GetExpandedKeys();

        MsgModel SaveCheckedKeys(long roleId, List<long> checkedIds);

        MsgModel UpdateStatus(long id, bool status);

    }
}
