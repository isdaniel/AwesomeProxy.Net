using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeProxy.FilterAttribute
{
    public abstract class AopBaseAttribute : Attribute, IExcuteFilter, IExceptionFilter
    {
        /// <summary>
        /// 執行後攔截方法
        /// </summary>
        /// <param name="result"></param>
        public virtual void OnExecuted(ExecutedContext context)
        {
        }

        /// <summary>
        /// 執行前攔截方法
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnExecuting(ExecutingContext context)
        {
        }

        /// <summary>
        /// 錯誤處理攔截方法
        /// </summary>
        /// <param name="exceptionContext">錯誤資訊上下文</param>
        public virtual void OnException(ExceptionContext exceptionContext)
        {
        }
    }
}