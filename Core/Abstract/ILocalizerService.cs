using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Core.Abstract
{
    public interface ILocalizerService
    {
        bool DoesCultureExist(string cultureName);
        IConfiguration GetResources();
        void SetLanguage(HttpContext context);
        string GetResourceFileName();
    }
}
