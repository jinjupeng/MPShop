using System;
using System.Collections.Generic;

namespace ApiServer.Auth.WeChat.Pay.Entities
{
    /// <summary>
    /// 微信支付结果通知信息
    /// <seealso cref="" href="https://pay.weixin.qq.com/wiki/doc/apiv3/wxpay/pay/transactions/chapter3_11.shtml#top" />
    /// </summary>
    public class PayNotifyResult
    {
        /// <summary>
        /// 通知ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 通知创建时间
        /// </summary>
        public DateTime create_time { get; set; }
        /// <summary>
        /// 通知的类型，支付成功通知的类型为TRANSACTION.SUCCESS
        /// <para>示例值：TRANSACTION.SUCCESS</para>
        /// </summary>
        public string event_type { get; set; }
        /// <summary>
        /// 通知的资源数据类型，支付成功通知为encrypt-resource 
        /// <para>示例值：encrypt-resource</para>
        /// </summary>
        public string resource_type { get; set; }
        #region 通知数据
        /// <summary>
        /// 通知数据
        /// </summary>
        public class Resource
        {
            /// <summary>
            /// 加密算法类型
            /// </summary>
            public string algorithm { get; set; }
            /// <summary>
            /// 密文
            /// </summary>
            public string ciphertext { get; set; }
            /// <summary>
            /// 附件数据
            /// </summary>
            public string associated_data { get; set; }
            /// <summary>
            /// 随机串
            /// </summary>
            public string nonce { get; set; }
        }
        /// <summary>
        /// 通知数据
        /// </summary>
        public Resource resource { get; set; }
        #endregion
        /// <summary>
        /// 回调摘要
        /// <para>示例值：支付成功</para>
        /// </summary>
        public string summary { get; set; }
    }
    /// <summary>
    /// 支付成功通知参数
    /// <seealso cref="" href="https://pay.weixin.qq.com/wiki/doc/apiv3/wxpay/pay/transactions/chapter3_11.shtml#top" />
    /// </summary>
    public class PayNotifySuccessResult
    {
        /// <summary>
        /// 直连商户申请的公众号或移动应用appid。
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 直连商户的商户号，由微信支付生成并下发。
        /// </summary>
        public string mchid { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 交易状态
        /// </summary>
        public string trade_state { get; set; }
        /// <summary>
        /// 交易状态描述
        /// </summary>
        public string trade_state_desc { get; set; }
        /// <summary>
        /// 付款银行
        /// </summary>
        public string bank_type { get; set; }
        /// <summary>
        /// 附加数据
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 支付完成时间
        /// </summary>
        public string success_time { get; set; }
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
            /// 用户支付金额
            /// </summary>
            public int payer_total { get; set; }
            /// <summary>
            /// 货币类型
            /// CNY：人民币，境内商户号仅支持人民币。
            /// </summary>
            public string currency { get; set; }
            /// <summary>
            /// 用户支付币种
            /// CNY：人民币，境内商户号仅支持人民币。
            /// </summary>
            public string payer_currency { get; set; }
        }
        /// <summary>
        /// 订单金额
        /// </summary>
        public Amount amount { get; set; }
        #endregion

        #region 场景信息
        /// <summary>
        /// 场景信息
        /// </summary>
        public class Scene_info
        {
            /// <summary>
            /// 商户端设备号
            /// </summary>
            public string device_id { get; set; }
        }
        /// <summary>
        /// 场景信息
        /// </summary>
        public Scene_info scene_info { get; set; }
        #endregion
        #region 优惠
        /// <summary>
        /// 优惠
        /// </summary>
        public class Promotion_detail
        {
            /// <summary>
            /// 券ID
            /// </summary>
            public string coupon_id { get; set; }
            /// <summary>
            /// 优惠名称
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 优惠范围
            /// </summary>
            public string scope { get; set; }
            /// <summary>
            /// 优惠类型
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 优惠券面额
            /// </summary>
            public int amount { get; set; }
            /// <summary>
            /// 活动id
            /// </summary>
            public string stock_id { get; set; }
            /// <summary>
            /// 微信出资，单位分
            /// </summary>
            public int wechatpay_contribute { get; set; }
            /// <summary>
            /// 优惠币种 CNY：人民币，境内商户号仅支持人民币。
            /// </summary>
            public string currency { get; set; }
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
                public string goods_id { get; set; }
                /// <summary>
                /// 商品数量
                /// </summary>
                public int quantity { get; set; }
                /// <summary>
                /// 商品单价
                /// </summary>
                public int unit_price { get; set; }
                /// <summary>
                /// 商品优惠金额
                /// </summary>
                public int discount_amount { get; set; }
                /// <summary>
                /// 商品备注
                /// </summary>
                public string goods_remark { get; set; }
            }
            #endregion
        }
        /// <summary>
        /// 优惠
        /// </summary>
        public Promotion_detail promotion_detail { get; set; }
        #endregion
    }
}
