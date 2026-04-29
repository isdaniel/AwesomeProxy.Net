using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace AwesomeProxy
{
    internal static class MethodInvokerCache
    {
        private static readonly ConcurrentDictionary<MethodInfo, Func<object, object[], object>> _cache
            = new ConcurrentDictionary<MethodInfo, Func<object, object[], object>>();

        public static Func<object, object[], object> GetOrCreate(MethodInfo method)
        {
            return _cache.GetOrAdd(method, BuildInvoker);
        }

        private static Func<object, object[], object> BuildInvoker(MethodInfo method)
        {
            var parameters = method.GetParameters();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.IsByRef)
                    return (target, args) => method.Invoke(target, args);
            }

            var targetParam = Expression.Parameter(typeof(object), "target");
            var argsParam = Expression.Parameter(typeof(object[]), "args");

            var argExpressions = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                argExpressions[i] = Expression.Convert(
                    Expression.ArrayIndex(argsParam, Expression.Constant(i)),
                    parameters[i].ParameterType);
            }

            var instance = Expression.Convert(targetParam, method.DeclaringType);
            Expression call = Expression.Call(instance, method, argExpressions);

            if (method.ReturnType == typeof(void))
            {
                var block = Expression.Block(call, Expression.Constant(null, typeof(object)));
                return Expression.Lambda<Func<object, object[], object>>(block, targetParam, argsParam).Compile();
            }

            if (method.ReturnType.IsValueType)
            {
                call = Expression.Convert(call, typeof(object));
            }

            return Expression.Lambda<Func<object, object[], object>>(call, targetParam, argsParam).Compile();
        }

        internal static void Clear() => _cache.Clear();
    }
}
