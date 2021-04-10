using ApiServer.Auth.WeChat.Pay.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiServer.Auth.WeChat.Pay
{
    /// <summary>
    /// 微信支付结果通知处理器
    /// <para>
    /// 微信支付结果通知中间件拦截微信服务器发生过来的请求,
    /// 解析请求的数据、解密、校验后得到最终的通知结果
    /// 回调IPayNotifyHandler
    /// </para>
    /// <seealso cref="" href="https://pay.weixin.qq.com/wiki/doc/apiv3/wxpay/pay/transactions/chapter3_11.shtml#top" />
    /// </summary>
    public interface IPayNotifyHandler
    {
        /// <summary>
        /// 处理微信支付结果通知
        /// 成功时不做任何处理，失败时直接抛出异常
        /// <seealso cref="" href="https://pay.weixin.qq.com/wiki/doc/apiv3/wxpay/pay/transactions/chapter3_11.shtml#top" />
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task PrecessAsync(PayNotifySuccessResult input, CancellationToken cancellationToken = default);
    }

    ///// <summary>
    ///// 支付结果通知处理器工厂
    ///// </summary>
    //public interface IPayNotifyHandlerFactory
    //{
    //    IPayNotifyHandler Create(WXPayOption option);
    //}
}
