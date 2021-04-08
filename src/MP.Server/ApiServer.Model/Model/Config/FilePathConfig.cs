﻿using ApiServer.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ApiServer.Model.Model.Config
{
    /// <summary>
    /// 路径配置
    /// </summary>
    public class FilePathConfig
    {
        private string _uploadPath;

        /// <summary>
        /// 文件上传存储根路径
        /// </summary>
        public string UploadPath
        {
            get
            {
                if (_uploadPath.IsNull())
                    _uploadPath = Path.Combine(AppContext.BaseDirectory, "Upload");

                if (!Path.IsPathRooted(_uploadPath))
                {
                    _uploadPath = Path.Combine(AppContext.BaseDirectory, _uploadPath);
                }

                return _uploadPath;
            }
            set => _uploadPath = value;
        }

        private string _tempPath;

        /// <summary>
        /// 临时文件存储根路径
        /// </summary>
        public string TempPath
        {
            get
            {
                if (_tempPath.IsNull())
                    _tempPath = Path.Combine(AppContext.BaseDirectory, "Temp");

                if (!Path.IsPathRooted(_tempPath))
                {
                    _tempPath = Path.Combine(AppContext.BaseDirectory, _tempPath);
                }

                return _tempPath;
            }
            set => _tempPath = value;
        }
    }
}
