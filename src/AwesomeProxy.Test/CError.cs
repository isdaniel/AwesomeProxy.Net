using System;

namespace AwesomeProxy.Test
{
    public class CError : ICError
    {
     
        public string GetError() {
            throw new Exception("test");
        }
    }
    public interface ICError
    {
        [CustomerError]
        string GetError();
    }
}