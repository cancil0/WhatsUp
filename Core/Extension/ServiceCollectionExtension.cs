using Core.Concrete;
using FluentValidation.AspNetCore;
using Infrastructure.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Reflection;
using System.Text;

namespace Core.Extension
{
    public static class ServiceCollectionExtension
    {
        private static readonly Assembly CoreAssembly = Assembly.Load("Core");
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters()
                .AddMemoryCache(x => x.ExpirationScanFrequency = TimeSpan.FromMinutes(30))
                .AddLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddNLog();
                })
                .AddEndpointsApiExplorer()
                .IntegrateSwagger()
                .AddDbContext(configuration)
                .InjectServices()
                .JwtSettings(configuration)
                .AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            return services;
        }

        public static IServiceCollection JwtSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audince"],
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        public static IServiceCollection IntegrateSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x.FullName);
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization Header",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Connection");
            LoggerFactory LoggerFactory = new();
            LoggerFactory.AddProvider(new NLogLoggerProvider());
            services.AddDbContext<Context>(optionsBuilder =>
            {
                optionsBuilder
                    .UseLoggerFactory(LoggerFactory)
                    .UseNpgsql(connectionString, postgreServerOption => 
                    {
                        postgreServerOption.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        postgreServerOption.CommandTimeout(configuration.GetValue<int>("Context:CommandTimeOut"));
                    })
                    .EnableDetailedErrors()
                    .UseLazyLoadingProxies(configuration.GetValue<bool>("Context:UseLazyLoading"))
                    .EnableSensitiveDataLogging(configuration.GetValue<bool>("Context:EnableSensitiveDataLogging"))
                    .EnableServiceProviderCaching(configuration.GetValue<bool>("Context:EnableServiceProviderCaching"));
            });

            return services;
        }

        public static IServiceCollection InjectNotGenerics(this IServiceCollection services)
        {
            services.AddScoped(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
            services.AddSingleton(typeof(IMemoryCache), typeof(MemoryCache));
            services.AddSingleton(typeof(IStringLocalizer), typeof(StringLocalizer));
            return services;
        }

        public static IServiceCollection InjectMiddlewares(this IServiceCollection services)
        {
            CoreAssembly.GetTypes()
                .Where(x => !string.IsNullOrEmpty(x.Namespace))
                .Where(x => x.IsClass)
                .Where(x => x.Namespace.StartsWith("Core.Middleware"))
                .ToList()
                .ForEach(assignedTypes =>
                {
                    services.AddScoped(assignedTypes);
                });

            return services;
        }

        public static IServiceCollection InjectCoreServices(this IServiceCollection services)
        {
            CoreAssembly.GetTypes()
                .Where(x => !string.IsNullOrEmpty(x.Namespace))
                .Where(x => x.IsClass)
                .Where(x => x.Namespace.StartsWith("Core.Concrete") && x.Name.Contains("Service") && !x.Name.Contains("Base"))
                .ToList()
                .ForEach(assignedTypes =>
                {
                    var serviceType = assignedTypes.GetInterfaces().First(i => i.Namespace.StartsWith("Core.Abstract"));
                    services.AddSingleton(serviceType, assignedTypes);
                });

            return services;
        }

        public static IServiceCollection InjectBusinessServices(this IServiceCollection services)
        {
            Assembly.Load("Business")
                .GetTypes()
                .Where(x => !string.IsNullOrEmpty(x.Namespace))
                .Where(x => x.IsClass)
                .Where(x => x.Namespace.StartsWith("Business.Concrete") && x.Name.Contains("Service"))
                .ToList()
                .ForEach(assignedTypes =>
                {
                    var serviceType = assignedTypes.GetInterfaces().First(i => i.Namespace.StartsWith("Business.Abstract"));
                    services.AddScoped(serviceType, assignedTypes);
                });

            return services;
        }
        public static IServiceCollection InjectDataAccessServices(this IServiceCollection services)
        {
            Assembly.Load("DataAccess")
                .GetTypes()
                .Where(x => !string.IsNullOrEmpty(x.Namespace))
                .Where(x => x.IsClass)
                .Where(x => x.Namespace.StartsWith("DataAccess.Concrete") && x.Name.EndsWith("Dal"))
                .ToList()
                .ForEach(assignedTypes =>
                {
                    var serviceType = assignedTypes.GetInterfaces().First(i => i.Namespace.StartsWith("DataAccess.Abstract"));
                    services.AddScoped(serviceType, assignedTypes);
                });
            return services;
        }

        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.InjectNotGenerics();
            services.InjectCoreServices();
            services.InjectMiddlewares();
            services.InjectDataAccessServices();
            //services.InjectBusinessServices();
            return services;
        }
    }
}
