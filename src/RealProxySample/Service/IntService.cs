using AOPLib.FilterAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOPLib.Core;
using Newtonsoft.Json;
using RealProxySample.Model;

namespace RealProxySample.Service
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

    public class ConsoleLogAttribute : AopBaseAttribute
    {
        public override void OnExcuted(ExcutedContext result)
        {
            Console.WriteLine(JsonConvert.SerializeObject(result.Args));
        }

        public override void OnExcuting(ExcuteingContext args)
        {
            Console.WriteLine($"傳入參數:{JsonConvert.SerializeObject(args.Args)}");
        }
    }
}