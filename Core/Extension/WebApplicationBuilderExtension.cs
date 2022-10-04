using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace Core.Extension
{
    public static class WebApplicationBuilderExtension
    {
        public static void RunApp(this WebApplication app)
        {
            app.UseSwagger()
                .ConfigSwagger()
                .UseHttpsRedirection()
                .UseRequestLocalization()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .AddCustomMiddlewares();
            app.MapControllers();
            app.Run();
        }

        public static WebApplicationBuilder SetBuilder(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddConfiguration(GetConfigurationFile());
            SetLogManagerConfig();
            builder.Services.AddServices(builder.Configuration);
            builder.Host
                .UseNLog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder => builder.RegisterServices());
            return builder;
        }

        public static void SetLogManagerConfig()
        {
            var environment = Environment.GetEnvironmentVariable("Environment") ?? "Development";
            var path = Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory), "Configs", environment, "nlog.config");
            LogManager.LoadConfiguration(path);
        }

        public static IConfiguration GetConfigurationFile(bool isUnitTest = false)
        {
            string environment, configPath;

            if (isUnitTest)
            {
#if DEBUG
                environment = "Development";
#else
                environment = "Production";
#endif
                configPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Configs", environment);
            }
            else
            {
                environment = Environment.GetEnvironmentVariable("Environment") ?? "Development";
                configPath = Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory), "Configs", environment);
            }

            var config = new ConfigurationBuilder()
                 .SetBasePath(configPath)
                 .AddJsonFile("environment.json", false)
                 .Build();

            return config;
        }
    }
}
