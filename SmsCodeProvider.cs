using Sugar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sugar.SMS
{
    public class SmsCodeProvider
    {
        /// <summary>
        /// 验证码数据
        /// </summary>
        private Dictionary<string, AuthCodeInfo> authCodeDic = new Dictionary<string, AuthCodeInfo>();
        /// <summary>
        /// 配置数据
        /// </summary>
        private SmsCodeOptions options;
        /// <summary>
        /// 短信服务
        /// </summary>
        private SmsProvider smsProvider;

        public SmsCodeProvider(SmsProvider smsProvider, SmsCodeOptions options)
        {
            this.smsProvider = smsProvider;
            this.options = options;
            //定时清理超时验证码
            Task.Run(() =>
            {
                while (true)
                {
                    if (authCodeDic.Count > 0)
                    {
                        var now = DateTime.Now;
                        var list = authCodeDic.Where(t => now.Subtract(t.Value.CreateTime) > options.liveMinutes).ToList();
                        list.ForEach(t => authCodeDic.Remove(t.Key));
                    }
                    Task.Delay(TimeSpan.FromHours(1));
                }
            });
        }
        /// <summary>
        /// 发送验证码(短信模板包含且仅包含两个参数，一个是验证码，一个是有效时间）
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="templateid">394325</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SendCode(string phone, int templateid, ref string msg)
        {
            if (!IsReSend(phone, ref msg)) return false;
            msg = CreateCode();
            var result = smsProvider.SendSMSMessage(new List<string> { phone }, templateid, new List<string>() { msg, options.liveMinutes.TotalMinutes.ToString("#0") });
            if (result.result == 0)
            {
                lock (authCodeDic)
                {
                    if (authCodeDic.ContainsKey(phone))
                    {
                        authCodeDic[phone] = new AuthCodeInfo() { Code = msg, CreateTime = DateTime.Now };
                    }
                    else
                    {
                        authCodeDic.Add(phone, new AuthCodeInfo() { Code = msg, CreateTime = DateTime.Now });
                    }
                }
                return true;
            }
            else
            {
                msg = $"{result.errMsg}({result.result})";
                return false;
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="reason">验证失败原因</param>
        /// <returns></returns>
        public bool Verify(string phone, string code, out string reason)
        {
            lock (authCodeDic)
            {
                if (!authCodeDic.ContainsKey(phone) || authCodeDic[phone] == null)
                {
                    reason = MessageType.当前数据不存在.ToString();
                    return false;
                }
                if (authCodeDic[phone].Code != code)
                {
                    reason = MessageType.验证码错误.ToString();
                    return false;
                }
                if (DateTime.Now.Subtract(authCodeDic[phone].CreateTime) > options.liveMinutes)
                {
                    reason = MessageType.验证码已超时.ToString();
                    authCodeDic.Remove(phone);
                    return false;
                }
                reason = MessageType.成功.ToString();
                authCodeDic.Remove(phone);
                return true;
            }
        }

        /// <summary>
        /// 是否可以重新发送
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        private bool IsReSend(string phone, ref string msg)
        {
            if (!options.ReSendCheck) return true;
            if (!authCodeDic.ContainsKey(phone)) return true;
            var timeDiff = DateTime.Now.Subtract(authCodeDic[phone].CreateTime);
            if (timeDiff > options.ReSendInteval) return true;
            msg = $"请勿频繁操作，等待{Math.Floor(options.ReSendInteval.TotalSeconds - timeDiff.TotalSeconds)}秒后可以重新发送";
            return false;
        }

        /// <summary>
        /// 生产验证码
        /// </summary>
        /// <returns></returns>
        private string CreateCode()
        {
            return (new Random(DateTimeOffset.Now.GetHashCode())).Next(0, MaxCodeValue()).ToString().PadLeft(options.CodeBit, '0');
        }

        /// <summary>
        /// 验证码最大数字
        /// </summary>
        /// <returns></returns>
        private int MaxCodeValue()
        {
            switch (options.CodeBit)
            {
                case 4:
                    return 9999;
                case 5:
                    return 99999;
                case 6:
                    return 999999;
                case 7:
                    return 9999999;
                case 8:
                    return 99999999;
                default:
                    return 9999;
            }
        }

        private class AuthCodeInfo
        {
            public string Code { get; set; }
            public DateTime CreateTime { get; set; }
            public bool Verified { get; set; }
        }
    }
}
