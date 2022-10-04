using Core.Abstract;
using Microsoft.Extensions.Localization;

namespace Core.Extension
{
    public class AppException : Exception
    {
        public string ExceptionType { get; set; }
        public AppException(string message, string exceptionType = "404", params string[] args)
            : base(Extensions.Resolve<IStringLocalizer>().GetString(message, args))
        {
            ExceptionType = exceptionType;
        }
    }
}
