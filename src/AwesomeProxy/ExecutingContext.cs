using System.Runtime.Remoting.Messaging;

namespace AwesomeProxy
{
    /// <summary>
    /// 執行前上下文
    /// </summary>
    public class ExecutingContext : ContextBase
    {
        public ExecutingContext(IMethodCallMessage callMessage)
        {
            Args = callMessage.Args;
            MethodName = callMessage.MethodName;
        }
    }
}