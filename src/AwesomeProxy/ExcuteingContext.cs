﻿using System;
using System.Runtime.Remoting.Messaging;

namespace AwesomeProxy
{
    /// <summary>
    /// 執行前上下文
    /// </summary>
    public class ExcuteingContext : IResult
    {
        public ExcuteingContext(IMethodCallMessage callMessage)
        {
            Args = callMessage.Args;
            MethodName = callMessage.MethodName;
        }

        public object[] Args { get; set; }

        public string MethodName { get; set; }

        /// <summary>
        /// 返回結果
        /// </summary>
        public object Result { get; set; }
    }

    /// <summary>
    /// 執行後上下文
    /// </summary>
    public class ExcutedContext : IResult
    {
        public ExcutedContext(IMethodReturnMessage returnMethod)
        {
            Args = returnMethod.Args;
            MethodName = returnMethod.MethodName;
            Result = returnMethod.ReturnValue;
        }

        public object[] Args { get; set; }

        public string MethodName { get; set; }

        /// <summary>
        /// 返回結果(如果非Null)
        /// </summary>
        public object Result { get; set; }
    }

    public class ExceptionContext : IResult
    {
        public ExceptionContext(IMethodCallMessage callMessage)
        {
            Args = callMessage.InArgs;
        }

        /// <summary>
        /// 傳入參數
        /// </summary>
        public object[] Args { get; set; }

        /// <summary>
        /// 返回錯誤回傳值
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public Exception Exception { get; set; }
    }

    public interface IResult
    {
        object Result { get; set; }
        object[] Args { get; set; }
    }

    public class ResultContext : IResult
    {
        public object Result { get; set; }
        public object[] Args { get; set; }
    }
}