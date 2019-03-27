using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeProxy
{
    public class ProxyFactory
    {
        /// <summary>
        /// 取得代理實體
        /// </summary>
        /// <param name="para">建構子參數</param>
        public static TObject GetProxyInstance<TObject>(object[] para = null)
            where TObject : MarshalByRefObject
        {
            TObject obj = Activator.CreateInstance(typeof(TObject), para) as TObject;
            return GetProxyInstance(obj);
        }

        /// <summary>
        /// 取得代理實體
        /// </summary>
        /// <typeparam name="TObject">代理類型別</typeparam>
        /// <param name="subjectType">被代理類型別</param>
        /// <param name="para">被代理類建構子參數</param>
        /// <returns></returns>
        public static TObject GetProxyInstance<TObject>(Type subjectType, object[] para = null)
            where TObject : MarshalByRefObject
        {
            TObject obj = Activator.CreateInstance(subjectType, para) as TObject;

            if (obj == null)
            {
                throw new ArgumentException($"傳入 subjectType 需繼承於{typeof(TObject).Name}");
            }

            return GetProxyInstance(obj);
        }

        /// <summary>
        /// 取得代理實體
        /// </summary>
        /// <typeparam name="TObject">代理類型別</typeparam>
        /// <param name="realSubject">被代理類別實體</param>
        /// <returns></returns>
        public static TObject GetProxyInstance<TObject>(TObject realSubject)
             where TObject : MarshalByRefObject
        {
            var proxy = new DynamicProxy<TObject>(realSubject);
            return proxy.GetTransparentProxy() as TObject;
        }
    }
}