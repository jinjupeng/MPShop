using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Auth.WeChat.Pay.Entities
{
    /// <summary>
    /// jsapi/小程序下单接口的返回值<br />
    /// <seealso cref="ServiceV3.ReadyToPayAsync(ReadyToPayForJSAPIOrMiniProgramInput)" href="https://pay.weixin.qq.com/wiki/doc/apiv3/wxpay/pay/transactions/chapter3_2.shtml" />
    /// </summary>
    public class ReadyToPayForJSAPIOrMiniProgramResult
    {
        public ReadyToPayForJSAPIOrMiniProgramResult()
        {

        }

        public ReadyToPayForJSAPIOrMiniProgramResult(string prepay_id)
        {
            this.prepay_id = prepay_id;
        }

        /// <summary>
        /// 预支付交易会话标识
        /// 预支付交易会话标识。用于后续接口调用中使用，该值有效期为2小时
        /// 示例值：wx201410272009395522657a690389285100
        /// </summary>
        public string prepay_id { get; set; }
    }
}
