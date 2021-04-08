using ApiServer.Model.Model.MsgModel;
using ApiServer.Model.Model.WX;

namespace ApiServer.BLL.IBLL
{
    public interface IWxService
    {
        MsgModel AuthLogin(WXAuth wxAuth);
        string GetSessionId(string code);
        string WxDecrypt(string encryptedData, string sessionId, string vi);
    }
}
