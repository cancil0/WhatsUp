using Autofac;
using Core.Abstract;
using Entity.Abstract;
using Entity.Enums;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Core.Extension
{
    public static class Extensions
    {
        public static ILifetimeScope LifetimeScope { get; set; }
        public static T Resolve<T>() => (T)LifetimeScope.Resolve<T>();

        public static async Task CustomException(HttpContext context)
        {
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                var loggerService = Resolve<ILoggerService>();
                switch (contextFeature.Error)
                {
                    case AppException exception:
                        {
                            loggerService.LogException(exception, contextFeature.Error.Message);
                            context.Response.StatusCode = exception.ExceptionType.ToInt();
                            break;
                        }
                    case DbUpdateException exception:
                        {
                            loggerService.LogException(exception, exception.InnerException?.Message ?? exception.Message);
                            context.Response.StatusCode = ExceptionTypes.InternalServerError.GetValue().ToInt();
                            break;
                        }
                    case Exception exception:
                        {
                            loggerService.LogException(exception, exception.InnerException?.Message ?? exception.Message);
                            context.Response.StatusCode = ExceptionTypes.InternalServerError.GetValue().ToInt();
                            break;
                        }
                    default:
                        {
                            loggerService.LogException(contextFeature.Error, contextFeature.Error.Message);
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                        }
                }

                context.Items["ExceptionOccured"] = true;

                var response = new ExceptionResponse()
                {
                    StatusCode = context.Response.StatusCode,
                    CorrelationId = context.Items["CorrelationId"].ToString(),
                    Message = contextFeature.Error.Message
                }.ToString();

                await context.Response.WriteAsync(response);
            }
        }

        public static ControllerActionDescriptor GetController(HttpContext httpContext)
        {
            var pathElements = httpContext.Request.Path.ToString().Split("/").Where(m => m != "");
            string controllerName = (pathElements.ElementAtOrDefault(0) == "" ? null : pathElements.ElementAtOrDefault(1));
            string actionName = (pathElements.ElementAtOrDefault(1) == "" ? null : pathElements.ElementAtOrDefault(2));

            var actionDescriptorsProvider = httpContext.RequestServices.GetRequiredService<IActionDescriptorCollectionProvider>();
            ControllerActionDescriptor controller = actionDescriptorsProvider.ActionDescriptors.Items
            .Where(s => s is ControllerActionDescriptor bb
                        && bb.ActionName == actionName
                        && bb.ControllerName == controllerName
                        && (bb.ActionConstraints == null
                            || (bb.ActionConstraints != null
                                && bb.ActionConstraints.Any(x => x is HttpMethodActionConstraint cc
                                && cc.HttpMethods.Any(m => m.ToLower() == httpContext.Request.Method.ToLower())))))
            .Select(s => s as ControllerActionDescriptor)
            .FirstOrDefault();
            return controller;
        }
    }
}
