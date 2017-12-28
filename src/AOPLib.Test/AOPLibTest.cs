using NUnit.Framework;
using Moq;
using AOPLib.FilterAttribute;
using AOPLib.Core;
using System.Runtime.Remoting.Messaging;
using System.Collections.Generic;
using System;

namespace AOPLib.Test
{
    

    public class ExcptionClass : MarshalByRefObject
    {
        [Exception]
        public string GetException(string errorMsg)
        {
            throw new Exception(errorMsg);
        }
    }

    public class RefArgClass : MarshalByRefObject
    {
        [Ref]
        public void ExcuteRef(ref string name)
        {
            name = $"Hello {name}";
        }

        [Out]
        public void ExcuteOut(out string name)
        {
            name = $"Hello ";
        }
    }

    [TestFixture]
    public class AOPTest
    {
        private Mock<AopBaseAttribute> _moqAttribute = new Mock<AopBaseAttribute>();
        private ExcutedContext _excutedContext;
        private ExcuteingContext _excutingContext;

        [SetUp]
        public void Init()
        {
            Mock<IMethodCallMessage> moqCallMessage = new Mock<IMethodCallMessage>();
            Mock<IMethodReturnMessage> moqReturnMessage = new Mock<IMethodReturnMessage>();

            moqCallMessage.Setup(o => o.Args).Returns(new object[] { "12345!!##,,11dasd" });
            moqCallMessage.Setup(o => o.MethodName).Returns("Test_ExcutedContext");
            moqReturnMessage.Setup(o => o.ReturnValue).Returns(It.IsAny<object>);

            IMethodCallMessage MethodCallobject = moqCallMessage.Object;
            IMethodReturnMessage ReturngOjbect = moqReturnMessage.Object;
            _excutedContext = new ExcutedContext(ReturngOjbect);
            _excutingContext = new ExcuteingContext(MethodCallobject);
        }

        [Test]
        public void MethodExcuting_Test()
        {
            Test1Attribute t = new Test1Attribute();
            t.OnExcuting(_excutingContext);

            var except = new object[] { "12345,,11dasd" };
            var result = _excutingContext.Args;

            Assert.AreEqual(except, result);
        }

        [Test]
        public void MethodExcuted_MethoName_True()
        {
            Test1Attribute t = new Test1Attribute();
            t.OnExcuted(_excutedContext);

            var except = "Test_ExcutedContext";
            var result = _excutedContext.MethodName;

            Assert.AreEqual(except, result);
        }

        [Test]
        public void Method_Exception_ErrorString()
        {
            string result = ProxyFactory.GetProxyInstance(new ExcptionClass()).GetException("錯誤!!");

            Assert.AreEqual(result, "錯誤!!");
        }

        [Test]
        public void Method_RefArg_and_OutArg_String()
        {
            string arg = "daniel";
            string exceptRef = "Hello danielHello";
            string exceptOut = "Hello Hello";

            string outString;

            ProxyFactory.GetProxyInstance(new RefArgClass()).ExcuteRef(ref arg);
            Assert.AreEqual(exceptRef, arg);

            ProxyFactory.GetProxyInstance(new RefArgClass()).ExcuteOut(out outString);
            Assert.AreEqual(exceptOut, outString);
        }
    }
}
