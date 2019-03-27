using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace AwesomeProxy
{
    /// <summary>
    /// AOP Proxy Class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DynamicProxy<T> : RealProxy
        where T : MarshalByRefObject
    {
        private readonly T _target;

        private IMethodCallMessage _callMethod = null;

        private IMethodReturnMessage _returnMethod = null;

        public DynamicProxy(T target) : base(typeof(T))
        {
            _target = target;
        }

        /// <summary>
        /// Execute RealSubject Method
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override IMessage Invoke(IMessage msg)
        {
            _callMethod = msg as IMethodCallMessage;
            MethodInfo targetMethod = _callMethod.MethodBase as MethodInfo;

            FilterInfo filterInfo = new FilterInfo(_target, targetMethod);

            try
            {
                ExecutingContext executing = Executing(filterInfo.ExecuteFilters);
                if (executing.Result != null)
                {
                    _returnMethod = GetReturnMessage(executing.Result, executing.Args);
                }
                else
                {
                    InvokeMethod(targetMethod, executing);

                    //Execute Executed Filters
                    Executed(filterInfo.ExecuteFilters);
                }
            }
            catch (Exception ex)
            {
                ExceptionContext exception = OnException(filterInfo.ExceptionFilters, ex);
                //Is There any Customer error Result
                if (exception.Result != null)
                {
                    _returnMethod = GetReturnMessage(exception.Result, exception.Args);
                }
                else
                {
                    _returnMethod = new ReturnMessage(ex, _callMethod);
                }
            }
            return _returnMethod;
        }

        private void InvokeMethod(MethodInfo targetMethod, ExecutingContext excuting)
        {
            object result = targetMethod.Invoke(_target, excuting.Args);
            _returnMethod = GetReturnMessage(result, excuting.Args);
        }

        /// <summary>
        /// 執行Exception過濾器
        /// </summary>
        /// <param name="exceptionFilter"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private ExceptionContext OnException(IList<IExceptionFilter> exceptionFilter, Exception ex)
        {
            ExceptionContext exceptionContext = new ExceptionContext(_callMethod)
            {
                Exception = ex
            };

            foreach (var filter in exceptionFilter)
            {
                filter.OnException(exceptionContext);
                if (exceptionContext.Result != null)
                    break;
            }

            return exceptionContext;
        }

        private ExecutedContext Executed(IList<IExcuteFilter> filters)
        {
            ExecutedContext executeContext = new ExecutedContext(_returnMethod);

            foreach (var filter in filters)
            {
                filter.OnExecuted(executeContext);
                if (executeContext.Result != null)
                    break;
            }

            return executeContext;
        }

        private ExecutingContext Executing(IList<IExcuteFilter> filters)
        {
            //封裝執行前上下文
            ExecutingContext execute = new ExecutingContext(_callMethod);

            foreach (var filter in filters)
            {
                filter.OnExecuting(execute);
                if (execute.Result != null)
                    break;
            }

            return execute;
        }

        private ReturnMessage GetReturnMessage(object result, object[] args)
        {
            return new ReturnMessage(result,
                                     args,
                                     args.Length,
                                     _callMethod.LogicalCallContext,
                                     _callMethod);
        }
    }
}