using NUnit.Framework;
using Moq;
using System;
using System.Reflection;
using AwesomeProxy.Test.IntegrationObject;

namespace AwesomeProxy.Test
{

    [TestFixture]
    public class AOPTest
    {
        private ExecutedContext _executedContext;
        private ExecutingContext _executing;

        [SetUp]
        public void Init()
        {
            Mock<MethodInfo> moqmethodInfo = new Mock<MethodInfo>();
            var args = new object[] {"12345!!##,,11dasd"};
            moqmethodInfo.Setup(o => o.Name).Returns("Test_ExecutedContext");

            _executedContext = new ExecutedContext(moqmethodInfo.Object, args, It.IsAny<object>());
            _executing = new ExecutingContext(moqmethodInfo.Object, args);
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

            Assert.AreEqual("Test_ExecutedContext", result);
        }

        [Test]
        public void Method_Exception_ErrorString()
        {
            string result = ProxyFactory.GetProxyInstance<IExceptionClass>(()=> new ExceptionClass()).GetException("錯誤!!");
           
            Assert.AreEqual(result, "錯誤!!");
        }

        [Test]
        public void Method_RefArg_and_OutArg_String()
        {
            string arg = "daniel";
            string exceptRef = "Hello danielHello";
            string exceptOut = "Hello Hello";

            string outString;

            ProxyFactory.GetProxyInstance<IRefArgClass>(()=>new RefArgClass()).ExecuteRef(ref arg);
            Assert.AreEqual(exceptRef, arg);

            ProxyFactory.GetProxyInstance<IRefArgClass>(() => new RefArgClass()).ExecuteOut(out outString);
            Assert.AreEqual(exceptOut, outString);
        }

        [Test]
        public void Method_DefaultException_ThrowDefaultException()
        {
            string except = "DefaultException";
            string result = ProxyFactory.GetProxyInstance<IExceptionClass>(() => new ExceptionClass()).ThrowDefaultException();

            Assert.AreEqual(except, result);
        }

        [Test]
        public void Method_Exception_ThrowException()
        {
            string except = "DefaultException";

            Exception ex = Assert.Throws<Exception>(() =>
            {
                new ExceptionClass().NormalException(except);
            });

            Assert.That(ex.Message, Is.EqualTo(except));
        }

        [Test]
        public void ProxyMethod_Exception_ThrowException()
        {
            string except = "DefaultException";

            TargetInvocationException ex = Assert.Throws<TargetInvocationException>(() =>
            {
                ProxyFactory.GetProxyInstance<IExceptionClass>(() => new ExceptionClass())
                .NormalException(except);
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

        [Test]
        public void WithParameterClass_Type_PasswordMask()
        {
            //WithParameterClass withParameter = new WithParameterClass("hello world!", "AwesomeProxy is so good!");

            var proxyInstance = ProxyFactory.GetProxyInstance<IWithParameterClass>(typeof(WithParameterClass),new object[] { "hello world!", "AwesomeProxy is so good!" });

            var act = proxyInstance.RecordInfo("Enter the password: abc1234");

            var expect = "hello world!\r\nAwesomeProxy is so good!\r\nEnter the password: *******\r\n";

            Assert.AreEqual(expect, act);
        }

        [Test]
        public void WithParameterClass_Generic_PasswordMask()
        {
            //WithParameterClass withParameter = new WithParameterClass("hello world!", "AwesomeProxy is so good!");

            var proxyInstance = ProxyFactory.GetProxyInstance<IWithParameterClass, WithParameterClass>(new object[] { "hello world!", "AwesomeProxy is so good!" });

            var act = proxyInstance.RecordInfo("Enter the password: abc1234");

            var expect = "hello world!\r\nAwesomeProxy is so good!\r\nEnter the password: *******\r\n";

            Assert.AreEqual(expect, act);
        }

        [Test]
        public void WithParameterClass_GetProxyInstanceWithParams_Type_PasswordMask()
        {
            //WithParameterClass withParameter = new WithParameterClass("hello world!", "AwesomeProxy is so good!");

            var proxyInstance = ProxyFactory.GetProxyInstanceWithParams<IWithParameterClass>(typeof(WithParameterClass), "hello world!", "AwesomeProxy is so good!");

            var act = proxyInstance.RecordInfo("Enter the password: abc1234");

            var expect = "hello world!\r\nAwesomeProxy is so good!\r\nEnter the password: *******\r\n";

            Assert.AreEqual(expect, act);
        }

        [Test]
        public void WithParameterClass_GetProxyInstanceWithParams_Generic_PasswordMask()
        {
            //WithParameterClass withParameter = new WithParameterClass("hello world!", "AwesomeProxy is so good!");

            var proxyInstance = ProxyFactory.GetProxyInstanceWithParams<IWithParameterClass, WithParameterClass>("hello world!", "AwesomeProxy is so good!");

            var act = proxyInstance.RecordInfo("Enter the password: abc1234");

            var expect = "hello world!\r\nAwesomeProxy is so good!\r\nEnter the password: *******\r\n";

            Assert.AreEqual(expect, act);
        }
    }
}