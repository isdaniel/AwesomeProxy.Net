using NUnit.Framework;
using Moq;
using AOPLib.FilterAttribute;
using AOPLib.Core;
using System.Runtime.Remoting.Messaging;
using System.Collections.Generic;


namespace AOPLib.Test
{
    public class Test1Attribute : AopBaseAttribute
    {
        public override void MethodExcuted(ExcutedContext result)
        {

        }
        public override void MethodExcuting(ExcuteingContext input)
        {
            List<object> oList = new List<object>();
            foreach (string s in input.InArgs)
            {
                oList.Add(s.Replace("!", "").Replace("#", ""));
            }
            input.InArgs = oList.ToArray();
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
            _excutedContext = new ExcutedContext(MethodCallobject, ReturngOjbect);
            _excutingContext = new ExcuteingContext(MethodCallobject);
        }

        [Test]
        public void MethodExcuting_Test()
        {
            Test1Attribute t = new Test1Attribute();
            t.MethodExcuting(_excutingContext);

            var except = new object[] { "12345,,11dasd" };
            var result = _excutingContext.InArgs;

            Assert.AreEqual(except, result);
        }

        [Test]
        public void MethodExcuted_MethoName_True()
        {
            Test1Attribute t = new Test1Attribute();
            t.MethodExcuted(_excutedContext);

            var except = "Test_ExcutedContext";
            var result = _excutedContext.MethodName;

            Assert.AreEqual(except, result);
        }
    }
}
