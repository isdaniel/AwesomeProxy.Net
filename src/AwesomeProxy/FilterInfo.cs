using System.Collections.Generic;
using System.Reflection;

namespace AwesomeProxy
{
    public class FilterInfo
    {
        private readonly CachedFilterInfo _cached;

        public FilterInfo(object target, MethodInfo method)
        {
            _cached = FilterCache.GetOrAdd(target.GetType(), method);
        }

        public IList<IExcuteFilter> ExecuteFilters
        {
            get
            {
                return new List<IExcuteFilter>(_cached.ExecuteFilters);
            }
        }

        public IList<IExceptionFilter> ExceptionFilters
        {
            get
            {
                return new List<IExceptionFilter>(_cached.ExceptionFilters);
            }
        }
    }
}
