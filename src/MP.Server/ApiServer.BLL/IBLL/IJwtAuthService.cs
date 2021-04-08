using ApiServer.Model.Model.MsgModel;
using ApiServer.Model.Model.ViewModel;

namespace ApiServer.BLL.IBLL
{
    public interface IJwtAuthService
    {
        MsgModel Login(string username, string password);

        MsgModel SignUp(SysUser user);
    }
}
