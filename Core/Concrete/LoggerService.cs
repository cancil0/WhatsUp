using Core.Abstract;
using Core.Attributes;
using Entity.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System.Reflection;

namespace Core.Concrete
{
    public class LoggerService : ILoggerService
    {
        /**
         * You can check loglevel's rules in nlog.config
         * If you want to watch logs in files and console but not in database, use LogDebug method
         * If you want to watch logs in database and console, use LogInfo method
         */

        private readonly IStringLocalizer stringLocalizer;
        private readonly ILogger logger;

        public LoggerService(IStringLocalizer stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            logger = LogManager.GetCurrentClassLogger();
        }

        public void LogInfo(string key, params string[] args) => logger.Info(stringLocalizer.GetString(key, args));
        public void LogDebug(string key, params string[] args) => logger.Debug(stringLocalizer.GetString(key, args));
        public void LogWarn(string key, params string[] args) => logger.Warn(stringLocalizer.GetString(key, args));
        public void LogError(string key, params string[] args) => logger.Error(stringLocalizer.GetString(key, args));
        public void LogException(Exception exception, string message) => logger.Error(exception, message);
        public void LogToApiCallLog(HttpContext context, string request, string response)
        {
            var controller = Extension.Extensions.GetController(context);
            var loggerController = controller.MethodInfo.GetCustomAttribute<ApiLoggerAttribute>();
            var controllerReturnType = controller.MethodInfo.ReturnType;
            if (loggerController != null)
            {
                var isRequestLoggable = loggerController.Request;
                var isResponseLoggable = loggerController.Response;
                var correlationId = context.Items["CorrelationId"].ToString();
                var apiId = new Guid(correlationId);
                /*dbContext.Set<ApiLog>().Add(new ApiLog()
                {
                    ApiLogId = apiId,
                    Request = isRequestLoggable ? request : null,
                    Response = isResponseLoggable ? HidePropertyInLog(response, controllerReturnType) : null,
                    StatusCode = context.Response.StatusCode,
                    ServiceName = context.Request.Path.Value.Split("/")[3],
                    RouteUrl = context.Request.Path,
                    Method = context.Request.Method,
                    Duration = context.Items["Duration"].ToString().ToLong(),
                    UserName = context.Items["UserName"]?.ToString() ?? Environment.MachineName,
                    CreatedDate = DateTime.Now.DateToInt(),
                    CreatedTime = DateTime.Now.TimeToInt(),
                });
                dbContext.SaveChanges(true);*/
            }
        }

        private static string HidePropertyInLog(string response, Type responseType)
        {
            var properties = responseType.GetProperties().LastOrDefault()?.PropertyType.GetProperties();
            if (properties == null || !properties.Any())
            {
                return response;
            }
            var jsonResponse = JObject.Parse(response);
            JToken jToken = jsonResponse.GetValue("Data");

            if (jToken == null)
            {
                return response;
            }

            return Hide(properties, jToken);
        }

        private static string Hide(PropertyInfo[] properties, JToken jToken)
        {
            var typeList = new List<Type>()
            {
                typeof(string),
                typeof(int),
                typeof(long),
                typeof(short),
                typeof(decimal),
                typeof(bool),
                typeof(DateTime),
                typeof(Guid)
            };
            var res = new JObject();
            bool isProperyClass;
            MaskedAttribute masked = null;
            foreach (var property in properties)
            {
                isProperyClass = typeList.Any(x => x.Name == property.PropertyType.Name);
                masked = property.GetCustomAttribute<MaskedAttribute>();
                if (masked != null && isProperyClass)
                {
                    res.Add(property.Name, "********");
                }
                else if (!isProperyClass)
                {
                    var items = property.PropertyType.GetProperties();
                    res.Add(Hide(items, jToken[property.Name]), jToken[property.Name]);
                }
                else
                {
                    res.Add(property.Name, jToken[property.Name]);
                }
            }
            return res.ToString(Formatting.None);
        }
    }
}