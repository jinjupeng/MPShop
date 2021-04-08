using ApiServer.Model.Model.MsgModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ApiServer.Extensions.Filters
{
    /// <summary>
    /// 自定义全局异常过滤器：当程序发生异常时，处理系统出现的未捕获的异常
    /// 自定义一个全局异常过滤器需要实现IExceptionFilter接口
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="env"></param>
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// IExceptionFilter接口会要求实现OnException方法，当系统发生未捕获异常时就会触发这个方法。
        /// OnException方法有一个ExceptionContext异常上下文，其中包含了具体的异常信息，HttpContext及mvc路由信息。
        /// 系统一旦出现未捕获异常后，比较常见的做法就是使用日志工具，将异常的详细信息记录下来，方便调试
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            MsgModel msgModel;
            var exception = context.Exception;
            if (exception is CustomException)
            {
                var customException = exception as CustomException;
                msgModel = MsgModel.Fail(customException.Code, customException.Msg);
            }
            else
            {
                msgModel = MsgModel.Fail(exception.Message);
            }

            //if (_env.IsDevelopment())
            //{
            //    json.message = context.Exception.StackTrace;// 堆栈信息
            //}

            context.Result = new InternalServerErrorObjectResult(msgModel);
            // 采用Serilog日志框架记录
            _logger.LogError(msgModel.message, WriteLog(msgModel.message, exception));
            context.ExceptionHandled = true;
        }

        /// <summary>
        /// 自定义返回格式
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public string WriteLog(string throwMsg, System.Exception ex)
        {
            return string.Format("【自定义错误】：{0} \r\n【异常类型】：{1} \r\n【异常信息】：{2} \r\n【堆栈调用】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
        }

        /// <summary>
        /// 
        /// </summary>
        public class InternalServerErrorObjectResult : ObjectResult
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            public InternalServerErrorObjectResult(object value) : base(value)
            {
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
