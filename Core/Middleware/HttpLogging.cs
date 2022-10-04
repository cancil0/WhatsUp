using Autofac;
using Core.Abstract;
using Core.Extension;
using Microsoft.AspNetCore.Http;

namespace Core.Middleware
{
    public class HttpLogging : IMiddleware
    {
        private readonly ILoggerService loggerService;
        private readonly ILocalizerService localizerService;
        public HttpLogging(ILoggerService loggerService, ILocalizerService localizerService)
        {
            this.loggerService = loggerService;
            this.localizerService = localizerService;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Extensions.LifetimeScope = (ILifetimeScope)context.RequestServices.GetService(typeof(ILifetimeScope));
            localizerService.SetLanguage(context);
            string correlationId = Guid.NewGuid().ToString();
            context.Items.Add("CorrelationId", correlationId);
            NLog.ScopeContext.PushProperty("CorrelationId", correlationId);

            context.Response.OnStarting(() =>
            {
                context.Response.ContentType = "application/json";
                context.Response.Headers["duration"] = context.Items["Duration"].ToString();

                var properties = new List<KeyValuePair<string, object>>()
                {
                    new("UserName", context.Items["UserName"] ?? "defaultuser"),
                    new("StatusCode", context.Response.StatusCode),
                    new("HttpMethod", context.Request.Method),
                    new("ServiceName", context.Request.Path.Value.Split("/")[3]),
                    new("RouteUrl", context.Request.Path),
                    new("Duration", context.Items["Duration"].ToString())
                };

                NLog.ScopeContext.PushProperties(properties);
                loggerService.LogDebug("Core.Logging.HttpServiceFinished", context.Items["Duration"].ToString());
                loggerService.LogDebug("ServiceName:{0} | StatusCode:{1} | HttpMethod:{2} | RouteUrl:{3}",
                    context.Request.Path.Value.Split("/")[3],
                    context.Response.StatusCode.ToString(),
                    context.Request.Method,
                    context.Request.Path);

                return Task.CompletedTask;
            });

            await next(context);
        }
    }
}