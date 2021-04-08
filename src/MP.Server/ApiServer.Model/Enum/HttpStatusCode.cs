using System.ComponentModel;

namespace ApiServer.Model.Enum
{
    /// <summary>
    /// 状态码枚举
    /// </summary>
    public enum HttpStatusCode
    {
        [Description("您输入的数据格式错误或您没有权限访问资源！")]
        Status400Error = 400,

        /// <summary>
        /// 未登录（需要重新登录）
        /// </summary>
        [Description("未授权，请重新登录")]
        Status401Unauthorized = 401,

        /// <summary>
        /// 权限不足
        /// </summary>
        [Description("拒绝访问")]
        Status403Forbidden = 403,

        /// <summary>
        /// 资源不存在
        /// </summary>
        [Description("请求错误,未找到该资源")]
        Status404NotFound = 404,

        [Description("请求方法未允许")]
        Status405MethodNotAllowed = 405,

        [Description("服务器等待客户端发送的请求时间过长，超时")]
        Status408RequestTimeout = 408,

        /// <summary>
        /// 系统内部错误（非业务代码里显式抛出的异常，例如由于数据不正确导致空指针异常、数据库异常等等）
        /// </summary>
        [Description("系统出现异常，请您稍后再试或联系管理员！")]
        Status500InternalServerError = 500,

        [Description("服务器不支持请求的功能，无法完成请求")]
        Status501NotImplemented = 501,

        [Description("网络错误")]
        Status502BadGateway = 502,

        [Description("服务不可用")]
        Status503ServiceUnavailable = 503,

        [Description("网络超时")]
        Status504GatewayTimeout = 504,

        [Description("http版本不支持该请求")]
        Status505HttpVersionNotSupported = 505,

    }
}
