using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugar.SMS
{
    public static class SmsServiceExtensions
    {
        /// <summary>
        /// 注册短信服务单例
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddSmsProvider(this IServiceCollection services, SmsOptions options)
        {
            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }

            //添加单例
            services.AddSingleton(SmsFactory.Create(options));

            return services;
        }

        /// <summary>
        /// 注册验证码单例
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddSmsCodeProvider(this IServiceCollection services, SmsCodeOptions options)
        {
            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }
            var smsProvider = services
                .FirstOrDefault(d => d.ServiceType == typeof(SmsProvider))
                ?.ImplementationInstance as SmsProvider;

            if (smsProvider == null)
            {
                throw new InvalidOperationException("\"SmsCodeProvider\" must be after \"SmsProvider\".");
            }

            //添加单例
            services.AddSingleton(SmsFactory.Create(smsProvider, options));

            return services;
        }

        public static IServiceCollection AddSmsCodeProvider(this IServiceCollection services)
        {
            return services.AddSmsCodeProvider(new SmsCodeOptions());
        }
    }
}
