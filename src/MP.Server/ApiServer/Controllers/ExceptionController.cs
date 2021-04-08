using ApiServer.BLL.IBLL;
using ApiServer.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 异常
    /// </summary>
    public class ExceptionController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IBaseService<sys_user> _baseService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public ExceptionController(ILogger<ExceptionController> logger, IBaseService<sys_user> baseService)
        {
            _logger = logger;
            _baseService = baseService;
        }

        [HttpGet]
        [Route("api/exception")]
        public IEnumerable<string> Get()
        {
            var sysUser = new sys_user
            {
                id = 1297873308628307970
            };
            _baseService.Insert(sysUser);
            _logger.LogError("全局异常过滤测试");
            // throw new System.Exception("自定义全局异常过滤抛出测试");
            return new List<string>();
        }
    }
}
