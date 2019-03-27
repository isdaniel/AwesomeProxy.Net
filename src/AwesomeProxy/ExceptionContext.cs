using System;
using System.Runtime.Remoting.Messaging;

namespace AwesomeProxy
{
    public class ExceptionContext : ContextBase
    {
        public ExceptionContext(IMethodCallMessage callMessage)
        {
            Args = callMessage.InArgs;
            MethodName = callMessage.MethodName;
        }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public Exception Exception { get; set; }
    }
}