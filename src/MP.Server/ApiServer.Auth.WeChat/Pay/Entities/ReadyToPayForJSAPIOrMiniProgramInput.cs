using System;
using System.Collections.Generic;

namespace ApiServer.Auth.WeChat.Pay.Entities
{
    /// <summary>
    /// JSAPI/小程序下单API的输入模型
    /// <seealso cref="ServiceV3.ReadyToPayAsync(ReadyToPayForJSAPIOrMiniProgramInput)" href="https://pay.weixin.qq.com/wiki/doc/apiv3/wxpay/pay/transactions/chapter3_2.shtml#top"/>
    /// </summary>
    public class ReadyToPayForJSAPIOrMiniProgramInput
    {
        /// <summary>
        /// 小程序、公众号、app的appId
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 微信支付商户号
        /// </summary>
        internal string mchid { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 交易结束时间
        /// </summary>
        public DateTime time_expire { get; set; }
        /// <summary>
        /// 附加数据
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 通知地址
        /// </summary>
        internal string notify_url { get; set; }
        /// <summary>
        /// 订单优惠标记
        /// </summary>
        public string goods_tag { get; set; }
        #region 订单金额
        /// <summary>
        /// 订单金额
        /// </summary>
        public class Amount
        {
            /// <summary>
            /// 订单总金额，单位为分。
            /// </summary>
            public int total { get; set; }
            /// <summary>
            /// 货币类型
            /// CNY：人民币，境内商户号仅支持人民币。
            /// </summary>
            public string currency { get; set; }
        }
        /// <summary>
        /// 订单金额
        /// </summary>
        public Amount amount { get; set; }
        #endregion
        #region 支付者
        /// <summary>
        /// 支付者
        /// </summary>
        public class Payer
        {
            /// <summary>
            /// 用户标识
            /// </summary>
            public string openid { get; set; }
        }
        /// <summary>
        /// 支付者
        /// </summary>
        public Payer payer { get; set; }
        #endregion
        #region 优惠
        /// <summary>
        /// 优惠
        /// </summary>
        public class Detail
        {
            /// <summary>
            /// 订单原价
            /// </summary>
            public int cost_price { get; set; }
            /// <summary>
            /// 商品小票ID
            /// </summary>
            public string invoice_id { get; set; }
            #region 单品
            /// <summary>
            /// 单品列表
            /// </summary>
            public List<Goods_detail> goods_detail { get; set; }
            /// <summary>
            /// 单品
            /// </summary>
            public class Goods_detail
            {
                /// <summary>
                /// 商户侧商品编码
                /// </summary>
                public string merchant_goods_id { get; set; }
                /// <summary>
                /// 微信侧商品编码
                /// </summary>
                public string wechatpay_goods_id { get; set; }
                /// <summary>
                /// 商品名称
                /// </summary>
                public string goods_name { get; set; }
                /// <summary>
                /// 商品数量
                /// </summary>
                public int quantity { get; set; }
                /// <summary>
                /// 商品单价
                /// </summary>
                public int unit_price { get; set; }
            }
            #endregion
        }
        /// <summary>
        /// 优惠
        /// </summary>
        public Detail detail { get; set; }
        #endregion
        #region 场景信息
        /// <summary>
        /// 场景信息
        /// </summary>
        public class Scene_info
        {
            /// <summary>
            /// 用户终端IP
            /// </summary>
            public string payer_client_ip { get; set; }
            /// <summary>
            /// 商户端设备号
            /// </summary>
            public string device_id { get; set; }
            /// <summary>
            /// 门店信息
            /// </summary>
            public Store_info store_Info { get; set; }
            /// <summary>
            /// 门店信息
            /// </summary>
            public class Store_info
            {
                /// <summary>
                /// 门店编号
                /// </summary>
                public string id { get; set; }
                /// <summary>
                /// 门店名称
                /// </summary>
                public string name { get; set; }
                /// <summary>
                /// 地区编码
                /// </summary>
                public string area_code { get; set; }
                /// <summary>
                /// 详细地址
                /// </summary>
                public string address { get; set; }
            }
        }
        /// <summary>
        /// 场景信息
        /// </summary>
        public Scene_info scene_info { get; set; }
        #endregion
    }
}
