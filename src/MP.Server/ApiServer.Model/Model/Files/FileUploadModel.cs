using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace ApiServer.Model.Model.Files
{
    /// <summary>
    /// 单文件上传
    /// </summary>
    public class FileUploadModel
    {
        /// <summary>
        /// 当前请求
        /// </summary>
        public HttpRequest Request { get; set; }

        /// <summary>
        /// 上传的文件对象
        /// </summary>
        public IFormFile FormFile { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 存储根路径
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>
        /// 模块编码
        /// </summary>
        public string Module { get; set; } = string.Empty;

        /// <summary>
        /// 分组
        /// </summary>
        public string Group { get; set; } = string.Empty;

        /// <summary>
        /// 路径
        /// </summary>
        public string SubPath { get; set; } = string.Empty;

        /// <summary>
        /// 最大允许大小(单位：字节，为0表示不限制)
        /// </summary>
        public long MaxSize { get; set; }

        /// <summary>
        /// 限制后缀名
        /// </summary>
        public List<string> LimitExt { get; set; }

        /// <summary>
        /// 计算文件的MD5
        /// </summary>
        public bool CalcMd5 { get; set; } = true;

        /// <summary>
        /// 完整目录
        /// </summary>
        public string FullPath => Path.Combine(RootPath, RelativePath);

        /// <summary>
        /// 相对目录
        /// </summary>
        public string RelativePath => Path.Combine(Module, Group, SubPath ?? String.Empty);
    }

}
