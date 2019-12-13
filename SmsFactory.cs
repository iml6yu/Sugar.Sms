using System;
using System.Collections.Generic;
using System.Text;

namespace Sugar.SMS
{
    internal class SmsFactory
    {
        internal static SmsProvider Create(SmsOptions options)
        {
            switch (options.SmsServiceType)
            {
                case SmsServiceType.AliYunSms:
                    throw new NotImplementedException("暂不支持阿里云短信服务");
                case SmsServiceType.TencentSms:
                default:
                    return new SmsTencentProvider(options); 
            }
        }

        internal static SmsCodeProvider Create(SmsProvider smsProvider, SmsCodeOptions options)
        {
            return new SmsCodeProvider(smsProvider, options);
        }
    }
}
