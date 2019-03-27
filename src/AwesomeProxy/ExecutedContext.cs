using System.Runtime.Remoting.Messaging;

namespace AwesomeProxy
{
    /// <summary>
    /// 執行後上下文
    /// </summary>
    public class ExecutedContext : ContextBase
    {
        public ExecutedContext(IMethodReturnMessage returnMethod)
        {
            Args = returnMethod.Args;
            MethodName = returnMethod.MethodName;
            Result = returnMethod.ReturnValue;
        }
    }
}