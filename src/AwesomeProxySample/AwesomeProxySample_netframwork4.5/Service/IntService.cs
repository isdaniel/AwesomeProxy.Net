using System;
using AwesomeProxySample.Model;
using AwesomeProxy.FilterAttribute;
using AwesomeProxy;
using Newtonsoft.Json;

namespace AwesomeProxySample.Service
{
    public class IntService : ServiceBase
    {
    }

    [ConsoleLog]
    public abstract class ServiceBase : MarshalByRefObject, IService<int>
    {
        public int add(int t1, int t2)
        {
            return t1 + t2;
        }

        public Person SetPerson(Person p)
        {
            p.Age = 100;
            p.Name = "test";
            return p;
        }
    }

    /// <summary>
    /// 紀錄Log標籤
    /// </summary>
    public class ConsoleLogAttribute : AopBaseAttribute
    {
        public override void OnExecuted(ExecutedContext result)
        {
            Console.WriteLine(JsonConvert.SerializeObject(result.Args));
        }

        public override void OnExecuting(ExecutingContext args)
        {
            Console.WriteLine($"傳入參數:{JsonConvert.SerializeObject(args.Args)}");
        }
    }
}