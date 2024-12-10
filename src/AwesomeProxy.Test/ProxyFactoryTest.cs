using System;
using AwesomeProxy.Test.IntegrationObject;
using NUnit.Framework;

namespace AwesomeProxy.Test
{
    public interface IA
    {

    }

    public class A : IA
    {
    }

    public class UnInheritClass { }

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
        public void ProxyFactory_UnInherit_ThrowArgumentException()
        {
            var expectedErrorMsg = $"傳入 subjectType 需繼承於IA";
            Assert.Throws<ArgumentException>(() => { ProxyFactory.GetProxyInstance<IA>(typeof(UnInheritClass)); }, expectedErrorMsg);
        }

        [Test]
        public void ProxyFactory_InheritTest_NoType()
        {
            var obj = ProxyFactory.GetProxyInstance<IA,A>();
            Assert.IsNotNull(obj);
        }

        [Test]
        public void ProxyFactory_NotHaveMatchingConstructor_Generic_ThrowArgumentException()
        {
            var expectedErrorMsg = $"Type 'A' does not have a matching constructor.";
            Assert.Throws<MissingMethodException>(() => { ProxyFactory.GetProxyInstance<IA, A>(new object[] { new object() }); }, expectedErrorMsg);
        }

        [Test]
        public void ProxyFactory_NotHaveMatchingConstructor_Parameter_ThrowArgumentException()
        {
            var expectedErrorMsg = $"Type 'A' does not have a matching constructor.";
            Assert.Throws<MissingMethodException>(() => { ProxyFactory.GetProxyInstance<IA>(typeof(A),new object[] { new object() }); }, expectedErrorMsg);
        }

        [Test]
        public void GetProxyInstanceWithParams_NotHaveMatchingConstructor_Parameter_ThrowArgumentException()
        {
            var expectedErrorMsg = $"Type 'A' does not have a matching constructor.";
            Assert.Throws<MissingMethodException>(() => { ProxyFactory.GetProxyInstanceWithParams<IA>(typeof(A), new object(), new object()); }, expectedErrorMsg);
        }

        [Test]
        public void GetProxyInstanceWithParams_NotHaveMatchingConstructor_Generic_ThrowArgumentException()
        {
            var expectedErrorMsg = $"Type 'A' does not have a matching constructor.";
            Assert.Throws<MissingMethodException>(() => { ProxyFactory.GetProxyInstanceWithParams<IA, A>(typeof(A), new object(), new object()); }, expectedErrorMsg);
        }

        [Test]
        public void ProxyFactory_GetCustomerError()
        {
            var Except = "TestError";

            var Result = ProxyFactory.GetProxyInstance<ICError>(()=>new CError()).GetError();

            Assert.AreEqual(Result, Except);
        }

        [Test]
        public void ProxyFactory_Null()
        {
            var ExpectErrorMesg = "realSubject delegation function can't be null!";

            Assert.Throws<NullReferenceException>(() => ProxyFactory.GetProxyInstance<ICError>(null) , ExpectErrorMesg);
        }
    }
}