using ApiServer.Model.Entity;
using ApiServer.Model.Model.MsgModel;

namespace ApiServer.BLL.IBLL
{
    public interface ISysOrgService
    {
        MsgModel GetOrgTreeById(int rootOrgId, string orgNameLike, bool? orgStatus);

        MsgModel UpdateOrg(sys_org sys_org);

        MsgModel AddOrg(sys_org sys_org);

        MsgModel DeleteOrg(sys_org sys_org);

        MsgModel UpdateStatus(int id, bool status);
    }

}
