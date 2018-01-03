using AOPLib.Core;
using AOPLib.FilterAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPLib.Test
{
    public class Test1Attribute : AopBaseAttribute
    {
        public override void OnExcuted(ExcutedContext result)
        {
           
        }
        public override void OnExcuting(ExcuteingContext input)
        {
            List<object> oList = new List<object>();
            foreach (string s in input.Args)
            {
                oList.Add(s.Replace("!", "").Replace("#", ""));
            }
            input.Args = oList.ToArray();
        }
    }

    public class ExceptionAttribute : AopBaseAttribute
    {
        public override void OnException(ExceptionContext exceptionContext)
        {
            exceptionContext.Result = exceptionContext.Exception.InnerException.Message;
        }
    }


    public class DefaultExceptionAttribute :AopBaseAttribute
    {
        public string DefaultErrorMsg { get; set; }
        public override void OnException(ExceptionContext exceptionContext)
        {
            exceptionContext.Result = DefaultErrorMsg;
        }
    }

    public class RefAttribute : AopBaseAttribute
    {

        public override void OnExcuting(ExcuteingContext args)
        {
            if (args.Args.FirstOrDefault() != null)
            {
                args.Args[0] = args.Args[0].ToString()+"Hello";
            }
        }
    }

    public class OutAttribute : AopBaseAttribute
    {

        public override void OnExcuting(ExcuteingContext args)
        {
            if (args.Args.FirstOrDefault() != null)
            {
                args.Args[0] = args.Args[0].ToString() + "Hello";
                Console.Write(args.Args[0]);
            }
        }

        public override void OnExcuted(ExcutedContext result)
        {
            if (result.Args.FirstOrDefault() != null)
            {
                result.Args[0] = result.Args[0].ToString() + "Hello";
                Console.Write(result.Args[0]);
            }
        }
    }
}
