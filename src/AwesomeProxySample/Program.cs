using AwesomeProxy;
using System;
using AwesomeProxySample.Service;
using AwesomeProxySample.Model;

namespace RealProxySample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //1.使用 ProxyFactory.GetProxyInstance 取得代理物件
            var t = ProxyFactory.GetProxyInstance<ServiceBase>(typeof(IntService));
            //2.執行方法
            var result = t.add(1, 2);
            t.SetPerson(new Person() { Age = 10 });
            Console.WriteLine(result);

            LoginInfo RDuser = new LoginInfo()
            {
                Name = "daniel",
                Type = AwesomeProxySample.AuthType.RD
            };

            LoginInfo PMuser = new LoginInfo()
            {
                Name = "Amy",
                Type = AwesomeProxySample.AuthType.PM
            };

            AuthService authService = ProxyFactory.GetProxyInstance
                <AuthService>();

            Console.WriteLine(authService.GetMoney(RDuser));

            Console.WriteLine(authService.GetMoney(PMuser));

            Console.ReadKey();
        }
    }
}