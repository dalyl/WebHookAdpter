using System;
using System.Collections.Generic;
using System.Text;

namespace WebHookAdpter.Core
{
    public interface IClient
    {
        StatusResult<T> Post<T>(string url, string data);
        StatusResult Post(string url, string data);
    }
}
