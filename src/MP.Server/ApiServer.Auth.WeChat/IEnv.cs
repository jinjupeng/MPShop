namespace ApiServer.Auth.WeChat
{
    /*
     * 有时候希望获取当前环境相关数据，但又不想直接引用依赖的包
     * 比如我们的文件上传功能是在独立的dll项目里做的，当这个dll用在asp.net core环境中时，希望上传到wwwroot目录中
     * 因为希望上传的图片将来可以被访问，但是因为我们是独立的dll项目，不希望它直接依赖asp.net相关包
     * 此时我们独立的项目可以依赖IEnv，而在asp.net中依赖注入具体实现
     */

    /// <summary>
    /// 用来获取当前程序宿主环境相关数据
    /// </summary>
    public interface IEnv
    {
        /// <summary>
        /// web根目录，在asp.net core中就是wwwroot目录
        /// </summary>
        string WebRoot { get; }
        /// <summary>
        /// 当前请求的根url地址 https://www.abc.com/
        /// </summary>
        string RootUrl { get; }
        /// <summary>
        /// 安全目录，此目录不能被普通用户请求。类似asp.net webform中的app_data app_code 等
        /// 在asp.net core中好像只有wwwroot默认能被外网普通用户请求到，应用程序根目录应该是外网用户不能随意访问的
        /// </summary>
        string SecureDirectory { get; }
    }
}
