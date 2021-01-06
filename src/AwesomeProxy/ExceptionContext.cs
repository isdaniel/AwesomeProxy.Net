using System;
using System.Reflection;


namespace AwesomeProxy
{
    public class ExceptionContext : ContextBase
    {
        public ExceptionContext(MethodInfo methodInfo,object[] args)
        {
            Args = args;
            MethodName = methodInfo.Name;
        }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public Exception Exception { get; set; }
    }
}