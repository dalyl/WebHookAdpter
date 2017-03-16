using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.Features;

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
