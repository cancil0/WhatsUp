using Core.Abstract;
using Entity.Abstract;
using Infrastructure.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Core.Middleware
{
    public class Response : IMiddleware
    {
        private readonly ILoggerService loggerService;
        private readonly IConfiguration configuration;
        private readonly Context dbContext;
        public Response(ILoggerService loggerService, IConfiguration configuration, Context dbContext)
        {
            this.loggerService = loggerService;
            this.configuration = configuration;
            this.dbContext = dbContext;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var request = context.Request;
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer);
            var requestData = Encoding.UTF8.GetString(buffer);
            if (string.IsNullOrEmpty(requestData) && (context.Request.Method == "GET" || context.Request.Method == "DELETE"))
            {
                if (context.Request.QueryString.HasValue)
                    requestData = context.Request.QueryString.Value;
                else
                    requestData = context.Request.Path.Value.Split('/').Last();
            }
            request.Body.Position = 0;

            Stream responseBody = context.Response.Body;
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await next(context);

            if (dbContext.Database.CurrentTransaction != null)
            {
                await dbContext.Database.CommitTransactionAsync();
                await dbContext.Database.CurrentTransaction.DisposeAsync();
            }

            memoryStream.Position = 0;
            string responseString = new StreamReader(memoryStream).ReadToEnd();
            string wrappedResponse = Wrap(responseString, context);
            byte[] responseBytes = Encoding.UTF8.GetBytes(wrappedResponse);
            context.Response.Body = responseBody;
            context.Response.ContentLength = responseBytes.Length;
            stopwatch.Stop();
            context.Items["Duration"] = stopwatch.ElapsedMilliseconds.ToString();

            if (configuration.GetValue<bool>("Logging:LogToApiLogTable"))
            {
                loggerService.LogToApiCallLog(context, requestData, wrappedResponse);
            }

            await context.Response.Body.WriteAsync(responseBytes);
        }

        private static string Wrap(string originalBody, HttpContext context)
        {
            /*
             * Check an exception is thrown. 
             * If it is true, we get response from Exception Middleware
             */
            var exceptionOccured = context.Items["ExceptionOccured"];
            if (exceptionOccured != null)
                return originalBody;

            object response;

            if ((context.Request.Method == "POST" || context.Request.Method == "PUT")
                && originalBody.Contains("validation errors"))
            {
                var returnObject = JObject.Parse(originalBody);
                response = returnObject.Property("errors").ToString(Formatting.None);
                response = JsonConvert.DeserializeObject("{" + response + "}");
            }
            else
            {
                //Json DeserializeObject
                if (originalBody.StartsWith("{") || originalBody.StartsWith("["))
                {
                    response = JsonConvert.DeserializeObject(originalBody, new JsonSerializerSettings()
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace,
                        Formatting = Formatting.Indented,
                        Culture = CultureInfo.CurrentCulture,
                        StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                    });
                }
                else
                {
                    response = string.IsNullOrEmpty(originalBody) ? null : originalBody;
                }
            }

            var wrapper = new SuccessResponse()
            {
                IsSuccessful = true,
                StatusCode = context.Response.StatusCode,
                CorrelationId = context.Items["CorrelationId"].ToString(),
                Message = "success",
                Data = response
            };

            return wrapper.ToString();
        }
    }
}