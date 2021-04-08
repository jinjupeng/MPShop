using ApiServer.Model.Model.Files;
using ApiServer.Model.Model.MsgModel;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ApiServer.BLL.IBLL
{
    public interface IFileUploadService
    {
        Task<MsgModel> UploadAsync(FileUploadModel model, CancellationToken cancellationToken = default);

        Task SaveAsync(IFormFile formFile, string savePath, CancellationToken cancellationToken = default);

        Task<string> SaveWidthMd5Async(IFormFile formFile, string savePath, CancellationToken cancellationToken = default);
    }
}
