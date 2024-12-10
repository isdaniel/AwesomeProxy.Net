using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwesomeProxy;
using AwesomeProxy.FilterAttribute;

namespace AwesomeProxySample_net60.Service
{
    /// <summary>
    /// 使用AwesomeProxy記得要繼承 MarshalByRefObject
    /// </summary>
    public class AuthService : IAuthService
    {
        public string GetMoney(LoginInfo userInfo)
        {
            return $"Welcome {userInfo.Name} Get 1000$";
        }
    }

    [WriteAble]
    public interface IAuthService {
        string GetMoney(LoginInfo userInfo);
    }

    /// <summary>
    /// 示範使用 權限驗證攔截
    /// </summary>
    public class WriteAbleAttribute : AopBaseAttribute
    {
        public override void OnExecuting(ExecutingContext context)
        {
            LoginInfo loginInfo = context.GetFirstArg<LoginInfo>();

            if (loginInfo != null)
            {
                if (loginInfo.Type != AuthType.RD)
                {
                    context.Result = $"非法入侵 {loginInfo.Name} 職位:{loginInfo.Type.ToString()}";
                }
            }
        }
    }

    public class LoginInfo
    {
        public string Name { get; set; }

        public AuthType Type { get; set; }
    }
}