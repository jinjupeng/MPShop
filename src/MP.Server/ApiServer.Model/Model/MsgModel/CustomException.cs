using System;

namespace ApiServer.Model.Model.MsgModel
{
    public class CustomException : ApplicationException
    {
        /// <summary>
        /// 异常错误编码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string Msg { get; set; }

        public CustomException() { }

        public CustomException(int code, string message)
        {
            Code = code;
            Msg = message;
        }

        public CustomException(int code, Exception innerException)
        {
            Code = code;
            Msg = innerException.Message;
        }
    }
}
