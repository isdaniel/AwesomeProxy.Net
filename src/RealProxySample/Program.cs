using AOPLib.Core;
using Newtonsoft.Json;
using RealProxySample.Service;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;

namespace RealProxySample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var t = ProxyFactory.GetProxyInstance<ServiceBase>(typeof(IntService));
            var result = t.add(1, 2);
            t.SetPerson(new Model.Person() { Age = 10 });
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}