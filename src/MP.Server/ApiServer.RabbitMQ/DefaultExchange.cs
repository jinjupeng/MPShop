

namespace ApiServer.RabbitMQ
{
    /// <summary>
    /// 默认交换器
    /// </summary>
    public class DefaultExchange
    {
        public const string Direct = "apiserver.direct";

        public const string Fanout = "apiserver.fanout";

        public const string Topic = "apiserver.topic";

        public const string Headers = "apiserver.headers";
    }
}
