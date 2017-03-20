using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using System.IO;
using System.Text;
using System.Threading;

namespace UnityApi.Server
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var startTime = DateTime.UtcNow;

            string requestBody =string.Empty;
            using (var stream = context.Request.Body)
            {
                using (var reader = new StreamReader(stream))
                {
                    try
                    {
                        requestBody = await reader.ReadToEndAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            }

            using (var stream = context.Response.Body)
            {
                using (var buffer = new MemoryStream())
                {
                    var watch = Stopwatch.StartNew();
                    context.Response.Body = buffer;
                    await _next.Invoke(context);
                    buffer.Seek(0, SeekOrigin.Begin);

                    using (var reader = new StreamReader(buffer))
                    {
                        string responseBody = await reader.ReadToEndAsync();

                        try
                        {

                            watch.Stop();
                            var logTemplate = @"Client IP: {clientIP}
Request path: {requestPath}
Request content type: {requestContentType}
Request content length: {requestContentLength}
Request content: {requestContent}
Response content: {responseContent}
Start time: {startTime}
Duration: {duration}";

                            _logger.LogInformation(logTemplate,
                                context.GetClientIPAddress(),
                                context.Request.Path,
                                context.Request.ContentType,
                                context.Request.ContentLength,
                                requestBody,
                                responseBody, 
                                startTime,
                                watch.ElapsedMilliseconds);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                            responseBody = string.Empty;
                            context.Response.StatusCode = 500;
                        }

                        var bytes = Encoding.UTF8.GetBytes(responseBody);
                        buffer.SetLength(0);
                        buffer.Write(bytes, 0, bytes.Length);
                        buffer.Seek(0, SeekOrigin.Begin);
                        context.Response.ContentLength = buffer.Length;
                        buffer.CopyTo(stream);
                    }
                }
            }
        }
    }

    public interface ICryptography
    {
        string Encrypt(string input);
    }

    public interface IMessageWriter
    {
        void WriterResponse(string requestBody, string responseBody);
        void WriterInterfaceLog(HttpContext context, string requestBody, string responseBody);
    }

    public class CoreLogMessageWriter : IMessageWriter
    {
        private readonly ILogger<CoreLogMessageWriter> _logger;
        private readonly EventId _Event = new EventId(100, "interface message");

        public CoreLogMessageWriter(ILogger<CoreLogMessageWriter> logger)
        {
            _logger = logger;
        }

        public void WriterResponse(string requestBody, string responseBody)
        {
            try
            {
                var messageTemplate = "";
                var message = messageTemplate;
                Write(message);
                Write("\n");
                Write(requestBody);
                Write("\n");
                Write(responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(_Event, ex, ex.Message);
            }
        }

        public void WriterInterfaceLog(HttpContext context, string requestBody, string responseBody)
        {
            try
            {
                var messageTemplate = "";
                var message = messageTemplate;
                Write(message);
                Write("\n");
                Write(requestBody);
                Write("\n");
                Write(responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(_Event, ex, ex.Message);
            }
        }

        private void Write(string message) {
            _logger.LogInformation(_Event, message);
        }
    }

    public class Cryptography : ICryptography
    {
        public string Encrypt(string input)
        {
            return input;
        }
    }

    public interface IMessageFactory
    {
        Task<bool> FilterRequest(HttpContext context);
        Task<string> Response(string context);
    }

    public class MessageFactory: IMessageFactory
    {
        private readonly IMessageWriter _writer;

        public MessageFactory(IMessageWriter writer)
        {
            RequestSwitchTable.Add("/user/", 300);
            RequestSwitchTable.Add("/system/", 400);
        }

        private Dictionary<string, int> RequestSwitchTable { get; set; } = new Dictionary<string, int>();

        private int GetAreaCode(HttpContext context) 
        {
            foreach (var item in RequestSwitchTable) {
                if (context.Request.Method.Contains(item.Key)) {
                    return item.Value;
                }
            }
            return 200;
        }

        private async Task<bool> Response300(HttpContext context)
        {
            await context.Response.WriteAsync("Hello 300 World!");
            return true;
        }

        private async Task<bool> Response400(HttpContext context)
        {
            await context.Response.WriteAsync("Hello 400 World!");
            return true;
        }

        private async Task<bool> Response500(HttpContext context)
        {
            await context.Response.WriteAsync("Hello 500 World!");
            return true;
        }

        public async Task<bool> FilterRequest(HttpContext context)
        {
            var code = GetAreaCode(context);
            switch (code)
            {
                case 300: return await Response300(context);
                case 400: return await Response400(context);
                case 200: return false;
                default: return await Response500(context);
            }
        }

        public async Task<string> Response(string context)
        {
            var time = DateTime.Now;
            var mac = ComputerMac(context,time.ToString("yyyyMMddhhmmss"));
            return $"{{\"Content\":\"{context}\",\"mac\":\"{mac}\",\"Time\":\"{time.ToString("yyyy-MM-dd hh:mm:ss")}\" }}";
        }

        private string ComputerMac(string context,string time)
        {
            return time;
        }

    }

    public class MessageConduitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MessageConduitMiddleware> _logger;
        private readonly IMessageFactory _handler;
        private readonly IMessageWriter _writer;
        private readonly EventId _Event = new EventId(1, "MessageConduitMiddleware");

        public MessageConduitMiddleware(RequestDelegate next, ILogger<MessageConduitMiddleware> logger, IMessageFactory handler, IMessageWriter writer)
        {
            _next = next;
            _logger = logger;
            _writer = writer;
            _handler = handler;
        }

        public async Task Invoke(HttpContext context)
        {
            var StartTime = DateTime.UtcNow;
            var isFiltered = await _handler.FilterRequest(context);
            if (isFiltered) return;
            var watch = Stopwatch.StartNew();
            var requestBody = await ReadRequest(context);
            var responseBody = await GetResponse(context);
            watch.Stop();
            _writer.WriterInterfaceLog(context, requestBody, responseBody);
        }

    

        public async Task<string> ReadRequest(HttpContext context)
        {
            using (var stream = context.Request.Body)
            {
                using (var reader = new StreamReader(stream))
                {
                    try
                    {
                        return await reader.ReadToEndAsync();
                    }
                    catch (Exception ex)
                    {
                       _logger.LogError(_Event, ex, ex.Message);
                    }
                }
            }
            return string.Empty;
        }

        public async Task<string> GetResponse(HttpContext context)
        {
            using (var stream = context.Response.Body)
            {
                using (var buffer = new MemoryStream())
                {
                    var responseBody = string.Empty;
                    context.Response.Body = buffer;
                    await _next.Invoke(context);
                    buffer.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(buffer))
                    {
                        var content = await reader.ReadToEndAsync();
                        responseBody = await _handler.Response(content);
                        _writer.WriterResponse(content, responseBody);
                        try
                        {
                            var bytes = Encoding.UTF8.GetBytes(responseBody);
                            buffer.SetLength(0);
                            buffer.Write(bytes, 0, bytes.Length);
                            buffer.Seek(0, SeekOrigin.Begin);
                            context.Response.ContentLength = buffer.Length;
                            buffer.CopyTo(stream);
                            return responseBody;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(_Event, ex, ex.Message);
                        }
                    }
                }
            }
            return string.Empty;
        }
    }

    public static class HttpContextExtensions
    {
        public static string GetClientIPAddress(this HttpContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var connection = context.Features.Get<IHttpConnectionFeature>();
            return connection?.RemoteIpAddress?.ToString();
        }
    }
}
