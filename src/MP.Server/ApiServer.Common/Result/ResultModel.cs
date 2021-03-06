using Newtonsoft.Json;

namespace ApiServer.Common.Result
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class ResultModel<T> : IResultModel<T>
    {
        public bool isok { get; set; } = true;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Msg { get; private set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = 200;

        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; private set; }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">说明</param>
        public ResultModel<T> Success(T data = default, string msg = "success")
        {
            Code = 200;
            Data = data;
            Msg = msg;
            isok = true;

            return this;
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg">说明</param>
        public ResultModel<T> Failed(string msg = "failed")
        {
            Code = 500;
            Msg = msg;
            isok = false;

            return this;
        }
    }

    /// <summary>
    /// 返回结果
    /// </summary>
    public static class ResultModel
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public static IResultModel Success<T>(T data = default, string msg = "ok")
        {
            return new ResultModel<T>().Success(data, msg);
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static IResultModel Success()
        {
            return Success<string>();
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        public static IResultModel Failed<T>(string error = null)
        {
            return new ResultModel<T>().Failed(error ?? "failed");
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <returns></returns>
        public static IResultModel Failed(string error = null)
        {
            return Failed<string>(error);
        }

        /// <summary>
        /// 根据布尔值返回结果
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static IResultModel Result<T>(bool success)
        {
            return success ? Success<T>() : Failed<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static IResultModel Result(bool success)
        {
            return success ? Success() : Failed();
        }

        /// <summary>
        /// 数据已存在
        /// </summary>
        /// <returns></returns>
        public static IResultModel HasExists => Failed("数据已存在");

        /// <summary>
        /// 数据不存在
        /// </summary>
        public static IResultModel NotExists => Failed("数据不存在");
    }
}
