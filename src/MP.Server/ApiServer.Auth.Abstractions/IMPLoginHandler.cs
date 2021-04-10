using ApiServer.Auth.Abstractions.LoginModels;
using System.Threading.Tasks;

namespace ApiServer.Auth.Abstractions
{
    public interface IMPLoginHandler
    {
        Task LoginAsync(LoginContext context);
    }
}
