using System.Threading.Tasks;

namespace ApiServer.BLL.IBLL
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string sendMsg);
    }
}
