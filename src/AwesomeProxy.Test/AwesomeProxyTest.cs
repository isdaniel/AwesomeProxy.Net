using NUnit.Framework;
using Moq;
using System.Runtime.Remoting.Messaging;
using System;

namespace AwesomeProxy.Test
{
    public class CError : MarshalByRefObject {
        [CustomerError]
        public string GetError() {
            throw new Exception("test");
        }
    }

    public class ExcptionClass : MarshalByRefObject
    {
        [Exception]
        public string GetException(string errorMsg)
        {
            throw new Exception(errorMsg);
        }

        [DefaultException(DefaultErrorMsg = "DefaultException")]
        public string ThrowDefaultExcption()
        {
            throw new Exception();
        }

        public void NormalExcption(string msg)
        {
            throw new Exception(msg);
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
        private ExecutedContext _executedContext;
        private ExecutingContext _executing;

        [SetUp]
        public void Init()
        {
            Mock<IMethodCallMessage> moqCallMessage = new Mock<IMethodCallMessage>();
            Mock<IMethodReturnMessage> moqReturnMessage = new Mock<IMethodReturnMessage>();

            moqCallMessage.Setup(o => o.Args).Returns(new object[] { "12345!!##,,11dasd" });
            moqCallMessage.Setup(o => o.MethodName).Returns("Test_ExecutedContext");
            moqReturnMessage.Setup(o => o.ReturnValue).Returns(It.IsAny<object>);

            _executedContext = new ExecutedContext(moqReturnMessage.Object);
            _executing = new ExecutingContext(moqCallMessage.Object);
        }

        [Test]
        public void MethodExecuting_Test()
        {
            Test1Attribute t = new Test1Attribute();
            t.OnExecuting(_executing);

            var except = new object[] { "12345,,11dasd" };
            var result = _executing.Args;

            Assert.AreEqual(except, result);
        }

        [Test]
        public void MethodExecuted_MethodNameNull_True()
        {
            Test1Attribute t = new Test1Attribute();
            t.OnExecuted(_executedContext);

            var result = _executedContext.MethodName;

            Assert.AreEqual(null, result);
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

        [Test]
        public void Method_DefaultException_ThrowDefaultExcption()
        {
            string except = "DefaultException";
            string result = ProxyFactory.GetProxyInstance(new ExcptionClass()).ThrowDefaultExcption();

            Assert.AreEqual(except, result);
        }

        [Test]
        public void Method_Exception_ThrowException()
        {
            string except = "DefaultException";

            Exception ex = Assert.Throws<Exception>(() =>
            {
                new ExcptionClass().NormalExcption(except);
            });

            Assert.That(ex.Message, Is.EqualTo(except));
        }

        [Test]
        public void ProxyMethod_Exception_ThrowException()
        {
            string except = "DefaultException";

            System.Reflection.TargetInvocationException ex = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
            {
                ProxyFactory.GetProxyInstance(new ExcptionClass())
                .NormalExcption(except);
            });

            Assert.That(ex.InnerException.Message, Is.EqualTo(except));
        }

        [Test]
        public void InputString_GetFirstArg_True()
        {
            var except = "12345!!##,,11dasd";
            var result = _executing.GetFirstArg<string>();

            Assert.AreEqual(except, result);
        }


        [Test]
        public void InputString_TryGetFirstArg_True()
        {
            var except = "12345!!##,,11dasd";
            string result;

            Assert.AreEqual(true, _executing.TryGetFirstArg(out result));
            Assert.AreEqual(except, result);
        }
    }
}