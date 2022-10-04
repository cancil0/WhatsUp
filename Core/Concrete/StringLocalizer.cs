using Core.Abstract;
using Core.Extension;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System.Net;

namespace Core.Concrete
{
    public class StringLocalizer : IStringLocalizer
    {
        private readonly ILocalizerService localizerService;
        private readonly IMemoryCache cache;
        private readonly IConfiguration configuration;
        public StringLocalizer(ILocalizerService localizerService, IMemoryCache cache, IConfiguration configuration)
        {
            this.localizerService = localizerService;
            this.cache = cache;
            this.configuration = configuration;
        }
        public LocalizedString this[string name] => GetResource(name);

        public LocalizedString this[string name, params object[] arguments] => GetResource(name, arguments);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            string fileName = localizerService.GetResourceFileName();

            if (!cache.TryGetValue(fileName, out List<LocalizedString> localizedStrings))
            {
                var resources = localizerService.GetResources();
                localizedStrings = new List<LocalizedString>();
                foreach (var item in resources.AsEnumerable())
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        localizedStrings.Add(new LocalizedString(item.Key.Replace(":", "."), item.Value));
                    }
                }
                cache.Set(fileName, localizedStrings);
            }

            return localizedStrings;
        }

        private LocalizedString GetResource(string name, params object[] arguments)
        {
            var isThrowException = configuration.GetValue<bool>("Exceptions:ThrowException:NotFoundResourceKey");
            if (string.IsNullOrEmpty(name))
            {
                if (isThrowException)
                {
                    throw new AppException("Core.Localization.KeyCanNotBeNull");
                }

                name = "Localization.KeyIsNull";
            }
            var resources = GetAllStrings(true);
            var resource = resources.FirstOrDefault(x => x.Name == name);

            if (string.IsNullOrEmpty(resource))
            {
                var exceptionKey = "Core.Localization.KeyNotFound";

                if (isThrowException)
                {
                    throw new AppException(exceptionKey, HttpStatusCode.NotFound.ToString(), name);
                }

                resource = resources.FirstOrDefault(x => x.Name == exceptionKey);
                arguments = new object[] { name };
            }

            return new LocalizedString(name, string.Format(resource, arguments));
        }
    }
}
