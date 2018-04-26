using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace AwesomeProxy
{
    public abstract class ContextBase
    {        
        /// <summary>
        /// 返回結果
        /// </summary>
        public object Result { get; set; }
        public object[] Args { get; set; }

        /// <summary>
        /// 取得第一個參數
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T GetFirstArg<T>()
            where T : class
        {
            T result = default(T);

            if (IsExistArgs)
                result = Args.OfType<T>().FirstOrDefault();

            return result;
        }

        public virtual bool TryGetFristArg<T>(out T arg)
             where T : class
        {
            arg = Args.OfType<T>().FirstOrDefault();

            return arg != default(T);
        }

        private bool IsExistArgs
        {
            get {
                return Args.Length > 0;
            }
        }
    }


    /// <summary>
    /// 執行前上下文
    /// </summary>
    public class ExcuteingContext : ContextBase
    {
        public ExcuteingContext(IMethodCallMessage callMessage)
        {
            Args = callMessage.Args;
            MethodName = callMessage.MethodName;
        }

        public string MethodName { get; set; }
    }

    /// <summary>
    /// 執行後上下文
    /// </summary>
    public class ExcutedContext : ContextBase
    {
        public ExcutedContext(IMethodReturnMessage returnMethod)
        {
            Args = returnMethod.Args;
            MethodName = returnMethod.MethodName;
            Result = returnMethod.ReturnValue;
        }
        public string MethodName { get; set; }
    }

    public class ExceptionContext : ContextBase
    {
        public ExceptionContext(IMethodCallMessage callMessage)
        {
            Args = callMessage.InArgs;
        }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public Exception Exception { get; set; }
    }
}