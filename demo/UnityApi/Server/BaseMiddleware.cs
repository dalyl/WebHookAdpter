using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Owin;
using Microsoft.AspNetCore.Http;

namespace UnityApi.Server
{
    public abstract class OwinMiddleware<TOptions>
    {

        private readonly RequestDelegate _next;

        public TOptions Options { get; set; }

        public HttpContext Context { get; set; }

        public OwinMiddleware(RequestDelegate next) { _next = next; }

        public Task Invoke(HttpContext context) {
            Context = context;
            var fetchHandlers =  CreateHandlers();
            Task.WaitAll(fetchHandlers);
            var handers = fetchHandlers.Result;
            if (handers == null)
            {
                throw (new Exception(" error define"));
            }
            foreach (var hander in handers) {
                var Invoke = hander.InvokeAsync();
                 Task.WaitAny(Invoke);
                Options = Invoke.Result;
            }
            return this._next(context);
        }

        public RequestDelegate Next { get { return _next; } }


        protected abstract Task<IList<IMiddlewareHandler<TOptions>>> CreateHandlers();

    }

    public interface IMiddlewareHandler<TOptions>
    {
        Task Initialize(TOptions opts, HttpContext context);
        Task<TOptions> InvokeAsync();
    }

   

    public class EncryptionOptions : BaseOptions { }

    /// <summary>
    /// 加密传输
    /// </summary>
    public class EncryptionMiddleware : OwinMiddleware<EncryptionOptions>
    {
        public EncryptionMiddleware(RequestDelegate next) : base(next)
        {

        }

        protected override async Task<IList<IMiddlewareHandler<EncryptionOptions>>> CreateHandlers()
        {
            throw new NotImplementedException();
        }
    }

    public class DecryptionOptions : BaseOptions { }

    public class DecryptionMiddleware : OwinMiddleware<DecryptionOptions>
    {
        public DecryptionMiddleware(RequestDelegate next) : base(next)
        {

        }

        protected override async Task<IList<IMiddlewareHandler<DecryptionOptions>>> CreateHandlers()
        {
            var opts = new DecryptionOptions();
            var handler = new EncryptionHandler();
            await handler.Initialize(opts, base.Context);
            return new List<IMiddlewareHandler<DecryptionOptions>> { handler };
        }

        public class EncryptionHandler :IMiddlewareHandler<DecryptionOptions>
        {
            public DecryptionOptions Options { get; set; }

            public HttpContext Context { get; set; }

            public async Task Initialize(DecryptionOptions opts, HttpContext context)
            {
                Options = opts;
                Context = context;
            }

            public async Task<DecryptionOptions> InvokeAsync() {


                return Options;
            }
        }

    }
}
