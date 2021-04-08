using Aliyun.OSS;
using Aliyun.OSS.Common;
using ApiServer.BLL.IBLL;
using ApiServer.Common.Extensions;
using ApiServer.Model.Enum;
using ApiServer.Model.Files;
using ApiServer.Model.Model.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ApiServer.BLL.BLL
{
    public class AliyunFileStorageService : IFileStorageService
    {
        private readonly ILogger<AliyunFileStorageService> _logger;
        private readonly AliyunConfig _config;
        public AliyunFileStorageService(IOptions<OSSConfig> config, ILogger<AliyunFileStorageService> logger)
        {
            _config = config.Value.Aliyun;
            _logger = logger;
        }

        public ValueTask<bool> Upload(FileObject fileObject)
        {
            // 创建OssClient实例。
            var client = new OssClient(_config.Endpoint, _config.AccessKeyId, _config.AccessKeySecret);
            try
            {
                // 上传文件。
                client.PutObject(_config.BucketName, fileObject.FileInfo.FullPath, fileObject.PhysicalPath);

                client.SetObjectAcl(_config.BucketName, fileObject.FileInfo.FullPath, fileObject.AccessMode == FileAccessMode.Open ? CannedAccessControlList.PublicRead : CannedAccessControlList.Private);

                fileObject.FileInfo.Url = GetUrl(fileObject.FileInfo.FullPath, fileObject.AccessMode);
                return new ValueTask<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("阿里云OSS文件上传异常：{@ex}", ex);
                return new ValueTask<bool>(false);
            }

        }

        public ValueTask<bool> Delete(FileObject fileObject)
        {
            bool result = true;
            var key = fileObject.FileInfo.FullPath;
            if (key.IsNull())
                result = false;

            // 创建OssClient实例。
            var client = new OssClient(_config.Endpoint, _config.AccessKeyId, _config.AccessKeySecret);
            try
            {
                // 删除文件
                client.DeleteObject(_config.BucketName, key);

                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("阿里云OSS文件删除异常：{@ex}", ex);
            }
            return new ValueTask<bool>(result);
        }

        public string GetUrl(string fullPath, FileAccessMode accessMode = FileAccessMode.Open)
        {
            if (fullPath.IsNull())
                return string.Empty;
            if (fullPath.StartsWith("http:", StringComparison.OrdinalIgnoreCase) || fullPath.StartsWith("https:", StringComparison.OrdinalIgnoreCase))
                return fullPath;

            //公开
            if (accessMode == FileAccessMode.Open)
            {
                return $"{_config.Domain}{HttpUtility.UrlEncode(fullPath, Encoding.UTF8)}";
            }

            //私有
            try
            {
                // 创建OSSClient实例。
                var client = new OssClient(_config.Endpoint, _config.AccessKeyId, _config.AccessKeySecret);
                // 生成签名URL
                var req = new GeneratePresignedUriRequest(_config.BucketName, fullPath, SignHttpMethod.Get)
                {
                    Expiration = DateTime.Now.AddHours(4)
                };
                var uri = client.GeneratePresignedUri(req);

                return $"{_config.Domain}{uri.PathAndQuery}";
            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}", ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }

            return string.Empty;
        }
    }
}
