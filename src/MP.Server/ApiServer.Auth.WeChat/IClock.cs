using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiServer.Auth.WeChat
{
    /// <summary>
    /// 时钟
    /// 系统中很多地方都需要使用当前时间，但本机的时间不一定准确
    /// 根据情况可以考虑提供一个获取网络上的准确时间获取对象
    /// 默认还是本地时间，LocalClock
    /// </summary>
    public interface IClock
    {
        ValueTask<DateTime> GetNowAsync();
        //DateTime GetNow();
        ValueTask<DateTimeOffset> GetNowOffsetAsync();
        //DateTimeOffset GetNowOffset();
    }
    /// <summary>
    /// 本地时钟
    /// </summary>
    public class LocalClock : IClock
    {
        //public static readonly LocalClock Instance = new LocalClock();
        public ValueTask<DateTime> GetNowAsync()
        {
            return new ValueTask<DateTime>(DateTime.Now);
        }

        public ValueTask<DateTimeOffset> GetNowOffsetAsync()
        {
            return new ValueTask<DateTimeOffset>(DateTimeOffset.Now);
        }

    }
}
