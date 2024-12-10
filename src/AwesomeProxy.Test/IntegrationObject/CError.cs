using System;

namespace AwesomeProxy.Test.IntegrationObject
{
    public class CError : ICError
    {

        public string GetError()
        {
            throw new Exception("test");
        }
    }
    public interface ICError
    {
        [CustomerError]
        string GetError();
    }
}