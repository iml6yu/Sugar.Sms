using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sugar.SMS
{
    public class SmsCodeOptions
    {
        /// <summary>
        /// 重发检验 默认true
        /// </summary>
        public bool ReSendCheck { get; set; } = true;

        /// <summary>
        /// 重发时间间隔（默认两分钟）
        /// </summary>
        public TimeSpan ReSendInteval { get; set; } = TimeSpan.FromMinutes(2);

        /// <summary>
        /// 验证码有效时常 （默认10分钟）
        /// </summary>
        public TimeSpan liveMinutes { get; set; } = TimeSpan.FromMinutes(10);

        /// <summary>
        /// 验证码位数 （默认4位）
        /// </summary>
        public int CodeBit { get; set; } = 4;//默认4位

    }


}
