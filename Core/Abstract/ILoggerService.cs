using Microsoft.AspNetCore.Http;

namespace Core.Abstract
{
    public interface ILoggerService
    {
        void LogInfo(string key = null, params string[] args);
        void LogDebug(string key, params string[] args);
        void LogWarn(string key, params string[] args);
        void LogError(string key, params string[] args);
        void LogException(Exception exception, string message);
        void LogToApiCallLog(HttpContext context, string request, string response);
    }
}