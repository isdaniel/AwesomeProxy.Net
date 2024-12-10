using System;
using AwesomeProxy.FilterAttribute;
using AwesomeProxySample_net60.Model;

namespace AwesomeProxySample_net60.Service
{
    public class IntService : ServiceBase
    {
    }

    [ConsoleLog]
    public abstract class ServiceBase : IService<int>
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

}