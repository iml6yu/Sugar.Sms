using qcloudsms_csharp;
using System;
using System.Collections.Generic;

namespace Sugar.SMS
{
    public abstract class SmsProvider
    {
        protected SmsOptions Options;
        public SmsProvider(SmsOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// 
        /// </summary> 
        /// <param name="sign"></param>
        /// <param name="phoneNumbers"></param>
        /// <param name="templateId"></param>
        /// <param name="contents"></param>
        /// <param name="nationCode"></param>
        /// <param name="extend"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public abstract SmsMultiSenderResult SendSMSMessage(List<string> phoneNumbers, int templateId, List<string> contents, string nationCode = "86", string extend = "", string ext = "");
    }

    /// <summary>
    /// 腾讯
    /// </summary>
    public class SmsTencentProvider : SmsProvider
    {
        public SmsTencentProvider(SmsOptions options) : base(options)
        {
        } 
        /// <summary>
        /// 
        /// </summary> 
        /// <param name="sign"></param>
        /// <param name="phoneNumbers"></param>
        /// <param name="templateId"></param>
        /// <param name="contents"></param>
        /// <param name="nationCode"></param>
        /// <param name="extend"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public override SmsMultiSenderResult SendSMSMessage( List<string> phoneNumbers, int templateId, List<string> contents, string nationCode = "86", string extend = "", string ext = "")
        {
            SmsMultiSender sender = new SmsMultiSender(Options.SmsAppid, Options.SmsAppKey);
            var result = sender.sendWithParam(nationCode, phoneNumbers,
                templateId, contents, Options.SignCompany, extend, ext);  // 签名参数未提供或者为空时，会使用默认签名发送短信
            return result;
        }
    }
    /// <summary>
    /// 阿里
    /// </summary>
    public class SmsALiYunProvider : SmsProvider
    {
        public SmsALiYunProvider(SmsOptions options) : base(options)
        {
        } 
      
        public override SmsMultiSenderResult SendSMSMessage( List<string> phoneNumbers, int templateId, List<string> contents, string nationCode = "86", string extend = "", string ext = "")
        {
            throw new NotImplementedException();
        }
    }
}
