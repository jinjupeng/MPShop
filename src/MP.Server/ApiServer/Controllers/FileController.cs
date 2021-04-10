using ApiServer.BLL.IBLL;
using ApiServer.Model.Enum;
using ApiServer.Model.Files;
using ApiServer.Model.Model.Config;
using ApiServer.Model.Model.Files;
using ApiServer.Model.Model.MsgModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileInfo = ApiServer.Model.Model.Files.FileInfo;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 文件接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IFileUploadService _fileUploadService;
        private readonly FilePathConfig _filePathConfig;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileStorageService"></param>
        /// <param name="fileUploadService"></param>
        /// <param name="filePathConfig"></param>
        public FileController(IFileStorageService fileStorageService, IFileUploadService fileUploadService,
            IOptions<FilePathConfig> filePathConfig)
        {
            _fileStorageService = fileStorageService;
            _fileUploadService = fileUploadService;
            _filePathConfig = filePathConfig.Value;
        }

        /// <summary>
        /// 单文件上传
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormCollection form)
        {
            var formFile = form.Files.FirstOrDefault();
            var uploadModel = new FileUploadModel
            {
                Request = Request,
                FormFile = formFile,
                RootPath = _filePathConfig.UploadPath,
                Module = "Admin",
                Group = Path.Combine("OSS", "Open"),
                SubPath = Path.Combine("code", "file")
            };

            var fileUploadResult = await _fileUploadService.UploadAsync(uploadModel);
            var fileInfo = new FileInfo(formFile.FileName)
            {
                SaveName = formFile.FileName,
                Path = "resource/"
            };

            var fileInfoResult = fileUploadResult.data as FileInfo;
            var fileObj = new FileObject
            {
                PhysicalPath = Path.Combine(_filePathConfig.UploadPath, fileInfoResult.FullPath),
                AccessMode = FileAccessMode.Private,
                Group = Path.Combine("OSS", "Open"),
                ModuleCode = "Admin",
                FileInfo = fileInfo
            };
            var fileStorageResult = _fileStorageService.Upload(fileObj);

            return Ok(await Task.FromResult(MsgModel.Success(fileStorageResult)));
        }
    }
}
