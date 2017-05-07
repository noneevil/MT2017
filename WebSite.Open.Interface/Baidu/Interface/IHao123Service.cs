using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Interface.Baidu
{
    /// <summary>
    /// 团购类接口。
    /// </summary>
    /// <remarks></remarks>
    public interface IHao123Service
    {
        /// <summary>
        /// 获取或设置服务实例返回结果的格式，格式参见<see cref="RestFormat"/>。
        /// </summary>
        /// <remarks></remarks>
        RestFormat Format { get; set; }

        /// <summary>
        /// 获取返回json格式结果的IHao123Service接口实例。
        /// </summary>
        /// <remarks></remarks>
        IHao123Service JsonFormatServer { get; }


        /// <summary>
        /// 获取返回xml格式结果的IHao123Service接口实例。
        /// </summary>
        /// <remarks></remarks>
        IHao123Service XMLFormatServer { get; }

        /// <summary>
        /// 团购订单信息提交（在付款成功后调用）。
        /// </summary>
        /// <param name="order_id">订单号，在提交方系统中唯一。</param>
        /// <param name="title">团购商品短标题小于255 bytes。</param>
        /// <param name="logo">团购商品图片（海报）url小于255bytes。</param>
        /// <param name="url">团购商品url（需要和提交给百度导航的xml api中的商品地址完全一致）小于255bytes。</param>
        /// <param name="price">商品单价 单位：分 如2100表示rmb21.00。</param>
        /// <param name="goods_num">购买数量。</param>
        /// <param name="sum_price">总价 单位：分 例如：300000。</param>
        /// <param name="summary">商品描述，例如： 价值186元的简单爱蛋糕（南瓜无糖），小于2048bytes</param>
        /// <param name="expire">消费券过期时间，自Jan 1 1970 00:00:00 GMT的秒数; 0为不限制。</param>
        /// <param name="addr">商家地址，例如：朝阳区建国路178号汇通时代广场; 小于1024bytes。</param>
        /// <param name="uid">百度uid，如无tn参数，则此参数必填，0表示不填。</param>
        /// <param name="mobile">用户手机号，可以为空。</param>
        /// <param name="tn">百度推广渠道唯一ID，如为空，则表示非百度推广渠道的订单。</param>
        /// <param name="baiduid">百度推广渠道带过去的baiduid参数，直接回传。</param>
        /// <param name="bonus">百度分成金额（单位：分），值为订单总价*分成比例。</param>
        /// <returns>xml或json格式字符串，包含error_code：错误代码，如无，则表示成功；error_msg：错误信息，成功则返回success；id：成功分配的id号，建议记录在日志，便于核对。</returns>
        /// <remarks></remarks>
        string SaveOrder(string order_id, string title, string logo, string url, int price, int goods_num, int sum_price,
            string summary, int expire, string addr, int uid, string mobile, string tn, string baiduid, int bonus);

        /// <summary>
        /// 更新消费券有效期（在更改消费券有效期后调用）。
        /// </summary>
        /// <param name="order_ids">订单号，在提交方系统中唯一，和创建提交的订单号一致；支持批量修改，多个使用英文逗号分隔。</param>
        /// <param name="expire">消费券过期时间，自Jan 1 1970 00:00:00 GMT的秒数; 0为不限制。</param>
        /// <returns></returns>
        /// <remarks></remarks>
        string UpdateExpire(string order_ids, int expire);

        /// <summary>
        /// 标记消费券消为已用（在用户消费后调用）。
        /// </summary>
        /// <param name="order_id">订单号，在提交方系统中唯一，和创建提交的订单号一致。</param>
        /// <param name="used_time">消费券使用时间，自Jan 1 1970 00:00:00 GMT的秒数。</param>
        /// <returns></returns>
        /// <remarks></remarks>
        string UseOrder(string order_id, int used_time);
    }
}
