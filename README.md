# 使用样例

StartUp.cs 中注入服务

```csharp
   services.AddSmsProvider(Configuration.GetSection("SmsOption").Get<SmsOptions>()).AddSmsCodeProvider();
```
## 发送
```csharp
 /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="phoneCode"></param>
        /// <returns></returns>
        [HttpGet("{phoneCode}")]
        public string VerifyCode(string phoneCode)
        {
            string code = string.Empty;
            if (smsCodeProvider.SendCode(phoneCode, 304353, ref code))
                return MessageResult.CreateSuccessState().ToJson();
            return MessageResult.CreateErrorState(MessageType.Api调用异常, code).ToString();
        }
```

## 验证
```csharp
if (!smsCodeProvider.Verify(sign.Account, sign.VerfityCode)) 
```