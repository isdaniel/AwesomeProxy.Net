using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace AwesomeProxy
{
    internal sealed class CachedFilterInfo
    {
        public IReadOnlyList<IExcuteFilter> ExecuteFilters { get; }
        public IReadOnlyList<IExceptionFilter> ExceptionFilters { get; }

        public CachedFilterInfo(Type targetType, MethodInfo method)
        {
            var classAttr = targetType.GetCustomAttributes(typeof(Attribute), true);
            var methodAttr = Attribute.GetCustomAttributes(method, typeof(Attribute), true);

            var execFilters = new List<IExcuteFilter>();
            var excFilters = new List<IExceptionFilter>();

            for (int i = 0; i < classAttr.Length; i++)
            {
                if (classAttr[i] is IExcuteFilter ef) execFilters.Add(ef);
                if (classAttr[i] is IExceptionFilter xf) excFilters.Add(xf);
            }

            for (int i = 0; i < methodAttr.Length; i++)
            {
                if (methodAttr[i] is IExcuteFilter ef) execFilters.Add(ef);
                if (methodAttr[i] is IExceptionFilter xf) excFilters.Add(xf);
            }

            ExecuteFilters = execFilters;
            ExceptionFilters = excFilters;
        }
    }

    internal static class FilterCache
    {
        private static readonly ConcurrentDictionary<(Type, MethodInfo), CachedFilterInfo> _cache
            = new ConcurrentDictionary<(Type, MethodInfo), CachedFilterInfo>();

        public static CachedFilterInfo GetOrAdd(Type targetType, MethodInfo method)
        {
            return _cache.GetOrAdd((targetType, method), key => new CachedFilterInfo(key.Item1, key.Item2));
        }

        internal static void Clear() => _cache.Clear();
    }
}
