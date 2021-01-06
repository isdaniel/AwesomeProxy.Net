using System;
using NUnit.Framework;

namespace AwesomeProxy.Test
{
    public interface IA
    {

    }

    public class A : IA
    {
    }

    [TestFixture]
    public class ProxyFactoryTest
    {

        [Test]
        public void ProxyFactory_InheritTest_True()
        {
           var obj = ProxyFactory.GetProxyInstance<IA>(typeof(A));
           Assert.IsNotNull(obj);
        }

        [Test]
        public void ProxyFactory_InheritTest_NoType()
        {
            var obj = ProxyFactory.GetProxyInstance<IA,A>();
            Assert.IsNotNull(obj);
        }

        [Test]
        public void ProxyFactory_GetCustomerError()
        {
            var Except = "TestError";

            var Result = ProxyFactory.GetProxyInstance<ICError>(()=>new CError()).GetError();

            Assert.AreEqual(Result, Except);
        }
    }
}