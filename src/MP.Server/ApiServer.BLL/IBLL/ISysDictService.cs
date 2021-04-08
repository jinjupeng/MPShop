using ApiServer.Model.Entity;
using ApiServer.Model.Model.MsgModel;

namespace ApiServer.BLL.IBLL
{
    public interface ISysDictService
    {
        MsgModel All();
        MsgModel Query(string groupName, string groupCode);

        MsgModel Update(sys_dict sys_Dict);
        MsgModel Add(sys_dict sys_Dict);
        MsgModel Delete(long id);
    }
}
