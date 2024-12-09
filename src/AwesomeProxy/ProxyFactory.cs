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
        public static TInterface GetProxyInstance<TInterface,TObject>(object[] para = null)
            where TObject : class,new()
            where TInterface:class
        {
            Type realObjectType = HasConstrcutorMetod(typeof(TObject),para);
            var target = Activator.CreateInstance(realObjectType, para) as TInterface;
            return GetProxyInstance(() => target);
        }

        /// <summary>
        /// 取得代理實體
        /// </summary>
        /// <typeparam name="TInterface">代理類型別</typeparam>
        /// <param name="subjectType">被代理類型別</param>
        /// <param name="para">被代理類建構子參數</param>
        /// <returns></returns>
        public static TInterface GetProxyInstance<TInterface>(Type subjectType, object[] para = null)
            where TInterface : class
        {
            HasConstrcutorMetod(subjectType, para);
            TInterface obj = Activator.CreateInstance(subjectType, para) as TInterface;

            if (obj == null)
            {
                throw new ArgumentException($"傳入 subjectType 需繼承於{typeof(TInterface).Name}");
            }

            return GetProxyInstance(()=> obj);
        }

        /// <summary>
        /// 取得代理實體
        /// </summary>
        /// <typeparam name="TObject">代理類型別</typeparam>
        /// <param name="realSubject">被代理類別實體</param>
        /// <returns></returns>
        public static TObject GetProxyInstance<TObject>(Func<TObject> realSubject)
             where TObject : class
        {
            if (realSubject == null)
            {
                throw new NullReferenceException("realSubject delegation function can't be null!");
            }

            return DynamicProxy<TObject>.CreateProxy(realSubject);
        }

        private static Type HasConstrcutorMetod(Type realObjectType, object[] para)
        {
            var parameterTypes = para?.Select(p => p?.GetType()).ToArray() ?? Type.EmptyTypes;
            var constructor = realObjectType.GetConstructor(parameterTypes);

            if (constructor == null)
            {
                throw new MissingMethodException($"Type '{realObjectType.Name}' does not have a matching constructor.");
            }

            return realObjectType;
        }

    }
}