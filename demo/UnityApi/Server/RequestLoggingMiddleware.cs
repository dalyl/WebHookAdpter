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

        public async Task Invoke_bak (HttpContext context)
        {
            var startTime = DateTime.UtcNow;

            var watch = Stopwatch.StartNew();
            await _next.Invoke(context);
            watch.Stop();

            var logTemplate = @"Client IP: {clientIP}
Request path: {requestPath}
Request content type: {requestContentType}
Request content length: {requestContentLength}
Start time: {startTime}
Duration: {duration}";

            _logger.LogInformation(logTemplate,
                context.GetClientIPAddress(),
                context.Request.Path,
                context.Request.ContentType,
                context.Request.ContentLength,
                startTime,
                watch.ElapsedMilliseconds);
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
                    context.Response.Body = buffer;
                    await _next.Invoke(context);
                    buffer.Seek(0, SeekOrigin.Begin);

                    using (var reader = new StreamReader(buffer))
                    {
                        string responseBody = await reader.ReadToEndAsync();

                        try
                        {
                           // responseBody = "";

                            var watch = Stopwatch.StartNew();
                            await _next.Invoke(context);
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

    }

    public interface IMessageWriter
    {
        void Write(string message);
    }

    public class CoreLogMessageWriter : IMessageWriter
    {
        private readonly ILogger<CoreLogMessageWriter> _logger;

        public CoreLogMessageWriter(ILogger<CoreLogMessageWriter> logger)
        {
            _logger = logger;
        }

        public void Write(string message) {
            var Event = new EventId (1,"interface message");
            _logger.LogInformation(Event, message);
        }
    }

    public class MessageConduitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICryptography _CryptographyProvider;
        private readonly IMessageWriter _writer;

        public MessageConduitMiddleware(RequestDelegate next, ICryptography CryptographyProvider, IMessageWriter writer)
        {
            _next = next;
            _writer = writer;
            _CryptographyProvider = CryptographyProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            var startTime = DateTime.UtcNow;
            string requestBody = string.Empty;
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
                    context.Response.Body = buffer;
                    await _next.Invoke(context);
                    buffer.Seek(0, SeekOrigin.Begin);

                    var watch = Stopwatch.StartNew();
                    using (var reader = new StreamReader(buffer))
                    {
                        string responseBody = await reader.ReadToEndAsync();

                        try
                        {
                            responseBody = "";/*encrypt here*/
                            var messageTemplate = "";
                            var message = messageTemplate;
                            _writer.Write(message);
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
                    watch.Stop();

                }
            }
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
