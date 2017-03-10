using System;
using System.Net.Http;
using System.Text;
using WebHookAdpter.Core;

namespace WebHookAdpter.Client
{
    public class Deserialize : IDeserialize
    {
        public T ReadResult<T>(string input)
        {
            return default(T);
        }
    }

    public class Client : IClient
    {
        public IDeserialize Deserializer { get; } = new Deserialize();

        RespondResult GetRespondResult<T>(HttpResponseMessage respond)
        {
            var result = respond.Content.ReadAsStringAsync().Result;
            if (respond.StatusCode == System.Net.HttpStatusCode.OK) {
                var content = Deserializer.ReadResult<T>(result);
                return new RespondResult<T> {  StatusCode= System.Net.HttpStatusCode.OK.ToString(), Content= content };
            }
            return new RespondError(respond.StatusCode.ToString(), respond.Content.ToString());
        }

        public RespondResult Post<T>(string url, string data)
        {
            HttpClient Client = new HttpClient();
            HttpContent Content = new StringContent(data, Encoding.UTF8, "application/json");
            var result = Client.PostAsync(new Uri(url), Content).Result;
            return GetRespondResult<T>(result);
        }

        public RespondResult Post(string url, string data)
        {
            return Post<string>(url,data);
        }

    }
}
