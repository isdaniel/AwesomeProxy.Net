using System.Reflection;

namespace AwesomeProxy
{
    /// <summary>
    /// 執行後上下文
    /// </summary>
    public class ExecutedContext : ContextBase
    {
        public ExecutedContext(MethodInfo methodInfo,object[] args,object returnValue)
        {
            Args = args;
            MethodName = methodInfo.Name;
            Result = returnValue;
        }
    }
}