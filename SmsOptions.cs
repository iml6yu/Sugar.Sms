using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sugar.SMS
{
    public class SmsOptions
    {
        /// <summary>
        /// 短信服务类型（默认腾讯云）
        /// </summary>
        public SmsServiceType SmsServiceType { get; set; } = SmsServiceType.TencentSms;
        /// <summary>
        /// appid
        /// </summary>
        public int SmsAppid { get; set; }
        /// <summary>
        /// appkey
        /// </summary>
        public string SmsAppKey { get; set; }

        /// <summary>
        /// 签名公司
        /// </summary>
        public string SignCompany { get; set; }

        /// <summary>
        /// 数据是否加密
        /// </summary>
        public bool IsEncrypt { get; set; }
    }

    public enum SmsServiceType
    {
        [Description("腾讯云短信服务")]
        TencentSms = 0,
        [Description("阿里云短信服务")]
        AliYunSms = 1
    }
}
