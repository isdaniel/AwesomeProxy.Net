using AOPLib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOPLib.Core;

namespace AOPLib.FilterAttribute
{
    public abstract class AopBaseAttribute : Attribute, IExcuteFilter,IExceptionFilter
    {
        public virtual void OnExcuted(ExcutedContext result)
        {
        }

        public virtual void OnExcuting(ExcuteingContext args)
        {
        }

        public virtual void OnException(ExceptionContext exceptionContext)
        {
        }
    }
}