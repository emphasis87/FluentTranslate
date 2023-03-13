using FluentTranslate.Serialization.Fluent;
using FluentTranslate.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FluentTranslate
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFluentTranslate(this IServiceCollection services)
        {
            return services.AddFluentTranslate(default(Configuration));
        }

        public static IServiceCollection AddFluentTranslate(this IServiceCollection services, Action<Configuration> configure)
        {
            var configuration = new Configuration();
            configuration.AddDefaultConfiguration();

            return services.AddFluentTranslate(configuration);
        }

        public static IServiceCollection AddFluentTranslate(this IServiceCollection services, Configuration? configuration = null)
        {
            services.AddSingleton<IFluentCloneFactory, CloneFactory>();
            services.AddSingleton<IFluentElementSerializer, Serialization.Fluent.FluentSerializer>();
            services.AddSingleton<IFluentDeserializer, FluentDeserializer>();

            if (configuration is null)
            {
                configuration = new Configuration();
                configuration.AddDefaultConfiguration();
                services.AddSingleton(configuration);
            }

            return services;
        }
    }
}
