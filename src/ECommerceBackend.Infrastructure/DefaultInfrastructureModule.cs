using System.Collections.Generic;
using System.Reflection;
using Autofac;
using ECommerceBackend.Core;
using ECommerceBackend.Core.Entities.OrderEntities;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Core.Services;
using ECommerceBackend.Infrastructure.Data;
using ECommerceBackend.SharedKernel.Interfaces;
using MediatR;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace ECommerceBackend.Infrastructure
{
    public class DefaultInfrastructureModule : Module
    {
        private bool _isDevelopment = false;
        private List<Assembly> _assemblies = new List<Assembly>();

        public DefaultInfrastructureModule(bool isDevelopment, Assembly callingAssembly = null)
        {
            _isDevelopment = isDevelopment;
            var coreAssembly = Assembly.GetAssembly(typeof(DatabasePopulator));
            var infrastructureAssembly = Assembly.GetAssembly(typeof(EfRepository));
            _assemblies.Add(coreAssembly);
            _assemblies.Add(infrastructureAssembly);
            if (callingAssembly != null)
            {
                _assemblies.Add(callingAssembly);
            }
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_isDevelopment)
            {
                RegisterDevelopmentOnlyDependencies(builder);
            }
            else
            {
                RegisterProductionOnlyDependencies(builder);
            }
            RegisterCommonDependencies(builder);
        }

        private void RegisterCommonDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<EfRepository>().As<IRepository>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                .RegisterAssemblyTypes(_assemblies.ToArray())
                .AsClosedTypesOf(mediatrOpenType)
                .AsImplementedInterfaces();
            }

            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<MailGunEmailService>().As<IEmailService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ImageKitService>().As<IImageService>()
               .InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<ICrudService<Product>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ProductStockUpdateService>().As<IProductStockUpdateService>()
               .InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<ICrudService<Order>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<IOrderService>()
               .InstancePerLifetimeScope();
        }

        private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
        {
            // TODO: Add development only services
        }

        private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
        {
            // TODO: Add production only services
        }

    }
}
