using System;
using System.Collections.Generic;
using System.Text;

namespace WebHookAdpter.Core
{
    public class RespondResult
    {
        public string StatusCode { get; set; }
    }

    public class RespondResult<T> : RespondResult
    {
        public T Content { get; set; }
    }

    public class RespondError : RespondResult<string> { public RespondError(string code, string content) { StatusCode = code; Content = content; } }

    public interface IDeserialize {
        T ReadResult<T>(string input);
    }

    public interface IClient
    {
        IDeserialize Deserializer { get; }

        RespondResult Post<T>(string url, string data);

        RespondResult Post(string url, string data);
    }
}
