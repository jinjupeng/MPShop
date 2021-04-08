using ApiServer.BLL.IBLL;
using ApiServer.Common.Encrypt;
using ApiServer.Common.Extensions;
using ApiServer.Model.Model.Files;
using ApiServer.Model.Model.MsgModel;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileInfo = ApiServer.Model.Model.Files.FileInfo;

namespace ApiServer.BLL.BLL
{
    public class FileUploadService : IFileUploadService
    {
        /// <summary>
        /// 单文件文件上传
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MsgModel> UploadAsync(FileUploadModel model, CancellationToken cancellationToken = default)
        {
            var result = new MsgModel();
            if (model.FormFile == null || model.FormFile.Length < 1)
            {
                if (model.Request.Form.Files != null && model.Request.Form.Files.Any())
                {
                    model.FormFile = model.Request.Form.Files[0];
                }
            }

            if (model.FormFile == null || model.FormFile.Length < 1)
                return MsgModel.Fail("请选择文件!");

            var name = model.FileName.IsNull() ? model.FormFile.FileName : model.FileName;
            var size = model.FormFile.Length;
            var fileInfo = new FileInfo(name, size);

            if (model.MaxSize > 0 && model.MaxSize < size)
                return MsgModel.Fail($"文件大小不能超过{new FileSize(model.MaxSize)}");

            if (model.LimitExt != null && !model.LimitExt.Any(m => m.EqualsIgnoreCase(fileInfo.Ext)))
                return MsgModel.Fail($"文件格式无效，请上传{model.LimitExt.Aggregate((x, y) => x + "," + y)}格式的文件");

            var date = DateTime.Now;
            fileInfo.Path = Path.Combine(model.RelativePath, date.ToString("yyyy"), date.ToString("MM"), date.ToString("dd"));
            var resultModel = await UploadSave(model.FormFile, fileInfo, model.RootPath, model.CalcMd5, cancellationToken);
            return MsgModel.Success(resultModel);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="formFile">文件</param>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="rootPath">根目录</param>
        /// <param name="calcMd5"></param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns></returns>
        private async Task<FileInfo> UploadSave(IFormFile formFile, FileInfo fileInfo, string rootPath, bool calcMd5, CancellationToken cancellationToken = default)
        {
            fileInfo.SaveName = $"{Guid.NewGuid().ToString().Replace("-", "")}.{fileInfo.Ext}";

            var fullDir = Path.Combine(rootPath, fileInfo.Path);
            if (!Directory.Exists(fullDir))
            {
                Directory.CreateDirectory(fullDir);
            }

            //写入
            var fullPath = Path.Combine(fullDir, fileInfo.SaveName);

            if (calcMd5)
                fileInfo.Md5 = await SaveWidthMd5Async(formFile, fullPath, cancellationToken);
            else
                await SaveAsync(formFile, fullPath, cancellationToken);

            return fileInfo;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="savePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SaveAsync(IFormFile formFile, string savePath, CancellationToken cancellationToken = default)
        {
            //写入
            using var stream = new FileStream(savePath, FileMode.Create);
            return formFile.CopyToAsync(stream, cancellationToken);
        }

        /// <summary>
        /// 保存文件，返回文件的MD5值
        /// </summary>
        /// <param name="formFile">文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<string> SaveWidthMd5Async(IFormFile formFile, string savePath, CancellationToken cancellationToken = default)
        {
            //写入
            await using var stream = new FileStream(savePath, FileMode.Create);
            var md5 = MD5Encrypt.Encrypt(stream);
            await formFile.CopyToAsync(stream, cancellationToken);

            return md5;
        }
    }
}
