using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwesomeProxy;
using AwesomeProxy.FilterAttribute;
using System.Runtime.Remoting.Messaging;

namespace AwesomeProxySample.Service
{
    public class CacheService : MarshalByRefObject
    {
        [Cache(CacheName = "GetCacheDate")]
        public string GetCacheDate()
        {
            return DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
        }
    }

    public class CacheAttribute : AopBaseAttribute
    {
        public string CacheName { get; set; }

        public override void OnExecuting(ExecutingContext context)
        {
            object cacheObj = CallContext.GetData(CacheName);
            if (cacheObj != null)
            {
                context.Result = cacheObj;
            }
        }

        public override void OnExecuted(ExecutedContext context)
        {
            CallContext.SetData(CacheName, context.Result);
        }
    }
}