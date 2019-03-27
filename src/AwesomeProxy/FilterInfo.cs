using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AwesomeProxy
{
    /// <summary>
    /// Filtering Current Registered Point
    /// 1.on Class
    /// 2.on Method
    /// </summary>
    public class FilterInfo
    {
        private readonly List<IExcuteFilter> _executeFilters = new List<IExcuteFilter>();
        private readonly List<IExceptionFilter> _exceptionFilters = new List<IExceptionFilter>();

        public FilterInfo(MarshalByRefObject target, MethodInfo method)
        {
            //search for class Attribute
            var classAttr = target.GetType().GetCustomAttributes(typeof(Attribute), true);
            //search for method Attribute
            var methodAttr = Attribute.GetCustomAttributes(method, typeof(Attribute), true);

            var unionAttr = classAttr.Union(methodAttr);

            _executeFilters.AddRange(unionAttr.OfType<IExcuteFilter>());
            _exceptionFilters.AddRange(unionAttr.OfType<IExceptionFilter>());
        }

        public IList<IExcuteFilter> ExecuteFilters
        {
            get
            {
                return _executeFilters;
            }
        }

        public IList<IExceptionFilter> ExceptionFilters
        {
            get
            {
                return _exceptionFilters;
            }
        }
    }
}