using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Autofac.Features.Scanning;
using Core.Abstract;
using Core.Interceptor;
using Entity.Enums;
using Entity.Mapping;
using Infrastructure.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Extension
{
    public static class AutofacExtension
    {
        /*
         * Use to resolve nested services
         */
        public static T ResolveService<T>(this ILifetimeScope scope)
        {
            return scope.Resolve<T>();
        }

        public static ContainerBuilder RegisterServices(this ContainerBuilder builder)
        {
            builder.RegisterInstance(AutoMapperConfig.Initialize())
                .SingleInstance();

            builder.Register(x => new UnitOfWorkInterceptor(x.Resolve<Context>(), x.Resolve<IHttpContextAccessor>()))
            .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.Load("Business"))
            .AsClosedInterfacesOf(typeof(IBaseService<,>))
            .AsImplementedInterfaces()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(UnitOfWorkInterceptor))
            .InstancePerLifetimeScope();

            return builder;
        }

        public static IContainer RegisterServicesToTest(this ContainerBuilder builder)
        {
            IConfiguration configuration = WebApplicationBuilderExtension.GetConfigurationFile(true);
            var serviceCollection = new ServiceCollection();
            //serviceCollection.AddServices(configuration);
            builder.RegisterServices();
            builder.Populate(serviceCollection);
            var container = builder.Build();
            return container;
        }

        /// <summary>
        /// Specifies that a type from a scanned assembly is registered if it implements an interface
        /// that closes the provided open generic interface type.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TScanningActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to set service mapping on.</param>
        /// <param name="openGenericServiceType">The open generic interface or base class type for which implementations will be found.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> AsClosedInterfacesOf<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Type openGenericServiceType) where TScanningActivatorData : ScanningActivatorData
        {
            if (openGenericServiceType is null)
                throw new AppException("Core.Autofac.AutofacGenericSystemNotFound", ExceptionTypes.InternalServerError.GetValue(), nameof(openGenericServiceType));
            if (!openGenericServiceType.IsInterface)
                throw new AppException("Core.Autofac.GenericTypeMustBeInterface", ExceptionTypes.InternalServerError.GetValue(), nameof(openGenericServiceType));

            return registration
                .Where(candidateType => candidateType.IsClosedTypeOf(openGenericServiceType))
                .As(candidateType =>
                    candidateType.GetInterfaces()
                        .Where(i => i.IsClosedTypeOf(openGenericServiceType))
                        .Select(t => (Service)new TypedService(t)));
        }
    }
}
