using System;
using System.Net.Http;
using WebHookAdpter.Core;

namespace WebHookAdpter.Client
{
    public class Client : IClient
    {

        public StatusResult<T> Post<T>(string url, string data)
        {
            HttpClient Client = new HttpClient();
            var Content =new StringContent(data);
            var result = Client.PostAsync(new Uri(url), Content).Result;
            if (result.StatusCode ==  System.Net.HttpStatusCode.OK) return new StatusResult<T>();
            return new StatusResult<T>( $"{{'StatusCode':'{ result.StatusCode }','Content':'{ result.Content }'");
        }


        public StatusResult Post(string url, string data)
        {
            HttpClient Client = new HttpClient();
            var Content = new StringContent(data);
            var result = Client.PostAsync(new Uri(url), Content).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK) return new StatusResult();
            return new StatusResult( $"{{'StatusCode':'{ result.StatusCode }','Content':'{ result.Content }'");
        }


    }
}
