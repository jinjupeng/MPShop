using ApiServer.Model.Model.MsgModel;
using ApiServer.Model.Model.WX;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiServer.BLL.IBLL
{
    public interface IWxService
    {
        MsgModel AuthLogin(WXAuth wxAuth);
        Task<string> GetSessionIdAsync(string code);
        string WxDecrypt(string encryptedData, string sessionId, string vi);
        string CreateShaString(string ticket, string timestamp, string nonce, string url);
        Task<Dictionary<string, string>> GetSignatureAsync(string url);
        Task<Dictionary<string, string>> GetOpenIdAsync(string code);
        Task<Dictionary<string, string>> GetUserInfoAsync(string access_token, string openid);
        void StartGetUserInfo(HttpRequest ctx, bool getUserInfo = false);
    }
}
