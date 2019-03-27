using AwesomeProxy.FilterAttribute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AwesomeProxy.Test
{
    public class Test1Attribute : AopBaseAttribute
    {
        public override void OnExecuted(ExecutedContext result)
        {
        }

        public override void OnExecuting(ExecutingContext input)
        {
            List<object> oList = new List<object>();
            foreach (string s in input.Args)
            {
                oList.Add(s.Replace("!", "").Replace("#", ""));
            }
            input.Args = oList.ToArray();
        }
    }

    public class CustomerErrorAttribute : AopBaseAttribute{
        public override void OnException(ExceptionContext exceptionContext)
        {
            exceptionContext.Result = "TestError";
        }
    }

    public class ExceptionAttribute : AopBaseAttribute
    {
        public override void OnException(ExceptionContext exceptionContext)
        {
            exceptionContext.Result = exceptionContext.Exception.InnerException.Message;
        }
    }

    public class DefaultExceptionAttribute : AopBaseAttribute
    {
        public string DefaultErrorMsg { get; set; }

        public override void OnException(ExceptionContext exceptionContext)
        {
            exceptionContext.Result = DefaultErrorMsg;
        }
    }

    public class RefAttribute : AopBaseAttribute
    {
        public override void OnExecuting(ExecutingContext args)
        {
            if (args.Args.FirstOrDefault() != null)
            {
                args.Args[0] = args.Args[0].ToString() + "Hello";
            }
        }
    }

    public class OutAttribute : AopBaseAttribute
    {
        public override void OnExecuting(ExecutingContext args)
        {
            if (args.Args.FirstOrDefault() != null)
            {
                args.Args[0] = args.Args[0].ToString() + "Hello";
                Console.Write(args.Args[0]);
            }
        }

        public override void OnExecuted(ExecutedContext result)
        {
            if (result.Args.FirstOrDefault() != null)
            {
                result.Args[0] = result.Args[0].ToString() + "Hello";
                Console.Write(result.Args[0]);
            }
        }
    }
}