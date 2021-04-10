namespace ApiServer.Model.Model.WX
{
    public class WXAuth
    {
        /// <summary>
        /// 加密数据
        /// </summary>
        public string EncryptData { get; set; }

        /// <summary>
        /// 加密向量
        /// </summary>
        public string IV { get; set; }

        public string SessionId { get; set; }
    }
}
