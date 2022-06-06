using FluentTranslate.Serialization.Fluent;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentTranslate.Common
{
    public static class FluentServices
    {
        private static readonly Lazy<IServiceProvider> _default = new(() =>
        {
            var services = new ServiceCollection();
            services.AddFluent();
            services.AddLogging();
            var provider = services.BuildServiceProvider();
            return provider;
        });

        public static IServiceProvider Default => _default.Value;

        public static void AddFluent(this IServiceCollection services)
        {
            services.AddSingleton<IFluentAggregator, FluentAggregator>();
            services.AddSingleton<IFluentCloneFactory, FluentCloneFactory>();
            services.AddSingleton<IFluentSerializer, FluentSerializer>();
            services.AddSingleton<IFluentDeserializer, FluentDeserializer>();
        }
    }
}
