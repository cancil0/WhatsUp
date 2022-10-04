using Core.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Text;

namespace Core.Concrete
{
    public class LocalizerService : ILocalizerService
    {
        private readonly IMemoryCache cache;
        private readonly IConfiguration configuration;

        public LocalizerService(IMemoryCache cache, IConfiguration configuration)
        {
            this.cache = cache;
            this.configuration = configuration;
        }

        public bool DoesCultureExist(string cultureName)
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Any(culture => string.Equals(culture.Name, cultureName, StringComparison.CurrentCultureIgnoreCase));
        }

        public IConfiguration GetResources()
        {
            string fileName = GetResourceFileName();

            if (!cache.TryGetValue(fileName + "a", out IConfiguration resources))
            {
                var path = Path.Combine(GetResourcePath(), fileName);
                using StreamReader reader = new(path);
                string json = reader.ReadToEnd();
                resources = new ConfigurationBuilder().AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(json))).Build();
                cache.Set(fileName + "a", resources);
            }

            return resources;
        }

        public void SetLanguage(HttpContext context)
        {
            string cultureKey;
            if (context.Request.Headers.TryGetValue("Accept-Language", out var acceptLang))
            {
                cultureKey = acceptLang.ToString().Split(",").First();
            }
            else
            {
                cultureKey = "en-US";
            }

            if (DoesCultureExist(cultureKey))
            {
                var culture = new CultureInfo(cultureKey);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
        }
        public string GetResourceFileName()
        {
            string cultureKey = CultureInfo.CurrentCulture.TwoLetterISOLanguageName ?? "en";
            return string.Format("{0}.json", cultureKey.ToString());
        }

        #region Private Methods

        private static string GetResourcePath()
        {
            return Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory), "Resources");
        }

        #endregion
    }
}
