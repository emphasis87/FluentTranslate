using FluentTranslate.Serialization.Fluent;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentTranslate.Services
{
    public static class FluentServices
    {
        public static IFluentAggregator Aggregator => Default.GetService<IFluentAggregator>();
        public static IFluentCloneFactory CloneFactory => Default.GetService<IFluentCloneFactory>();
        public static IFluentSerializer Serializer => Default.GetService<IFluentSerializer>();
        public static IFluentDeserializer Deserializer => Default.GetService<IFluentDeserializer>();

        private static readonly Lazy<IServiceProvider> _defaultLazy = new(() =>
        {
            var services = new ServiceCollection();
            services.AddFluent();
            services.AddLogging();
            var provider = services.BuildServiceProvider();
            return provider;
        });

        private static IServiceProvider _default;
        public static IServiceProvider Default
        {
            get => _default ?? _defaultLazy.Value;
            set => _default = value;
        }

        public static void AddFluent(this IServiceCollection services)
        {
            services.AddSingleton<IFluentAggregator, FluentAggregator>();
            services.AddSingleton<IFluentCloneFactory, FluentCloneFactory>();
            services.AddSingleton<IFluentSerializer, FluentSerializer>();
            services.AddSingleton<IFluentDeserializer, FluentDeserializer>();
        }
    }
}
