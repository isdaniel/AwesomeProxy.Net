using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AwesomeProxy.Test
{
    public abstract class InheritMarshalByRefObject : MarshalByRefObject
    {
    }

    public class InheritSubClass : InheritMarshalByRefObject
    {
    }

    public class NoInheritMarshalByRefObject
    {
    }

    [TestFixture]
    public class ProxyFactoryTest
    {
        [Test]
        public void NoArgs_InheritMarshalByRefObject_True()
        {
            var DFactory = ProxyFactory.GetProxyInstance(new InheritSubClass());
        }

        [Test]
        public void ProxyFactory_InheritTest_True()
        {
            ProxyFactory.GetProxyInstance<InheritMarshalByRefObject>(typeof(InheritSubClass));
        }

        [Test]
        public void ProxyFactory_InheritTest_ThrowEx()
        {
            var Except = "傳入 subjectType 需繼承於InheritMarshalByRefObject";

            var Result = Assert.Throws<ArgumentException>(() =>
            {
                ProxyFactory.GetProxyInstance<InheritMarshalByRefObject>(typeof(NoInheritMarshalByRefObject));
            });

            Assert.AreEqual(Result.Message, Except);
        }

        [Test]
        public void ProxyFactory_GetCustomerError()
        {
            var Except = "TestError";

            var Result = ProxyFactory.GetProxyInstance<CError>().GetError();

            Assert.AreEqual(Result, Except);
        }
    }
}