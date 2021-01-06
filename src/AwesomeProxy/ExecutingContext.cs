
using System.Reflection;

namespace AwesomeProxy
{
    /// <summary>
    /// 執行前上下文
    /// </summary>
    public class ExecutingContext : ContextBase
    {
        public ExecutingContext(MethodInfo methodInfo,object[] args)
        {
            Args = args;
            MethodName = methodInfo.Name;
        }
    }
}