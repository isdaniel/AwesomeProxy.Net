using System;
using System.Collections.Generic;
using System.Reflection;

namespace AwesomeProxy
{
    /// <summary>
    /// AOP Proxy Class
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public class DynamicProxy<TObject> : DispatchProxy
    {
        private TObject _target;
        private MethodInfo _targetMethod;
        private object[] _args;

        /// <summary>
        /// 執行Exception過濾器
        /// </summary>
        /// <param name="exceptionFilter"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private ExceptionContext OnException(IList<IExceptionFilter> exceptionFilter, Exception ex)
        {
            ExceptionContext exceptionContext = new ExceptionContext(_targetMethod,_args)
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

        public static TObject CreateProxy(Func<TObject> creator)
        {
            if (creator == null)
            {
                throw new NullReferenceException("creator delegation function can't be null!");
            }

            object proxy = Create<TObject, DynamicProxy<TObject>>();
            ((DynamicProxy<TObject>) proxy)._target = creator();

            return (TObject)proxy;
        }


        private void Executed(IList<IExcuteFilter> filters, object returnValue)
        {
            ExecutedContext executeContext = new ExecutedContext(_targetMethod, _args, returnValue);

            foreach (var filter in filters)
            {
                filter.OnExecuted(executeContext);
                if (executeContext.Result != null)
                    break;
            }
        }

        private ExecutingContext Executing(IList<IExcuteFilter> filters)
        {
            //封裝執行前上下文
            ExecutingContext execute = new ExecutingContext(_targetMethod, _args);

            foreach (var filter in filters)
            {
                filter.OnExecuting(execute);
                if (execute.Result != null)
                    break;
            }

            return execute;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            FilterInfo filterInfo = new FilterInfo(_target, targetMethod);
            _targetMethod = targetMethod;
            _args = args;
            object result;
            try
            {
                ExecutingContext executing = Executing(filterInfo.ExecuteFilters);

                if (executing.Result != null)
                {
                    return executing.Result;
                }

                result = targetMethod.Invoke(_target, args);

                //Execute Executed Filters
                Executed(filterInfo.ExecuteFilters, result);
            }
            catch (Exception ex)
            {
                ExceptionContext exception = OnException(filterInfo.ExceptionFilters, ex);
                //Is There any Customer error Result
                if (exception.Result != null)
                {
                    return exception.Result;
                }

                throw ex;
            }
            return result;
        }
    }
}