namespace ApiServer.Auth.WeChat.Pay.Entities
{
    /// <summary>
    /// 通用的一个模型，里面包含code、message字段
    /// </summary>
    public class StateDto
    {
        public static readonly StateDto Success = new StateDto("SUCCESS", "");
        public static readonly StateDto Error = new StateDto("Error", "系统错误！");

        public static readonly string SuccessJsonString;

        static StateDto()
        {
            SuccessJsonString = System.Text.Json.JsonSerializer.Serialize(Success);
        }

        public StateDto()
        {
        }

        public StateDto(string code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public string code { get; set; }
        public string message { get; set; }
    }
}
