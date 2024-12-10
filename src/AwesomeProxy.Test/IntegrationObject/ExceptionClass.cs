using System;

namespace AwesomeProxy.Test.IntegrationObject
{
    public class ExceptionClass : IExceptionClass
    {

        public string GetException(string errorMsg)
        {
            throw new Exception(errorMsg);
        }

        public string ThrowDefaultException()
        {
            throw new Exception();
        }

        public void NormalException(string msg)
        {
            throw new Exception(msg);
        }
    }

    public interface IExceptionClass
    {
        [Exception]
        public string GetException(string errorMsg);
        [DefaultException(DefaultErrorMsg = "DefaultException")]
        public string ThrowDefaultException();
        public void NormalException(string msg);
    }
}