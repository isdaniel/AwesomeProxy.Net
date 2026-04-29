using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace AwesomeProxy
{
    public class DynamicProxy<TObject> : DispatchProxy
    {
        private TObject _target;

        private ExceptionContext OnException(IReadOnlyList<IExceptionFilter> exceptionFilter, MethodInfo targetMethod, object[] args, Exception ex)
        {
            ExceptionContext exceptionContext = new ExceptionContext(targetMethod, args)
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
            ((DynamicProxy<TObject>)proxy)._target = creator();

            return (TObject)proxy;
        }

        private void Executed(IReadOnlyList<IExcuteFilter> filters, MethodInfo targetMethod, object[] args, object returnValue)
        {
            ExecutedContext executeContext = new ExecutedContext(targetMethod, args, returnValue);

            foreach (var filter in filters)
            {
                filter.OnExecuted(executeContext);
                if (executeContext.Result != null)
                    break;
            }
        }

        private ExecutingContext Executing(IReadOnlyList<IExcuteFilter> filters, MethodInfo targetMethod, object[] args)
        {
            ExecutingContext execute = new ExecutingContext(targetMethod, args);

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
            var cached = FilterCache.GetOrAdd(_target.GetType(), targetMethod);
            object result;
            try
            {
                ExecutingContext executing = Executing(cached.ExecuteFilters, targetMethod, args);

                if (executing.Result != null)
                {
                    return executing.Result;
                }

                var invoker = MethodInvokerCache.GetOrCreate(targetMethod);
                result = invoker(_target, args);

                Executed(cached.ExecuteFilters, targetMethod, args, result);
            }
            catch (TargetInvocationException tie)
            {
                var realEx = tie.InnerException ?? tie;
                ExceptionContext exception = OnException(cached.ExceptionFilters, targetMethod, args, realEx);
                if (exception.Result != null)
                {
                    return exception.Result;
                }

                ExceptionDispatchInfo.Capture(realEx).Throw();
                return null;
            }
            catch (Exception ex)
            {
                ExceptionContext exception = OnException(cached.ExceptionFilters, targetMethod, args, ex);
                if (exception.Result != null)
                {
                    return exception.Result;
                }

                ExceptionDispatchInfo.Capture(ex).Throw();
                return null;
            }
            return result;
        }
    }
}
