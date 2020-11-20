using System;
using System.Net.Http;
using FluentTranslate.Infrastructure;

namespace FluentTranslate
{
	public interface IFluentConfiguration
	{
		IFluentCompositeProvider Providers { get; }
		IFluentOptionsContainer Options { get; }
		IFluentServiceContainer Services { get; }
	}

	public class FluentConfiguration : IFluentConfiguration
	{
		public IFluentServiceContainer Services { get; }
		public IFluentOptionsContainer Options { get; }
		public IFluentCompositeProvider Providers { get; }

		public FluentConfiguration()
		{
			Services = new FluentServiceContainer();
			Options = new FluentOptionsContainer();
			Providers = new FluentInvariantCompositeProvider(this);
		}

		public static FluentConfiguration Default
		{
			get
			{
				var configuration = new FluentConfiguration();

				configuration.Services.AddService<IFluentConfiguration>(configuration);

				configuration.Services.AddService<IFluentCloneFactory>(FluentCloneFactory.Default);
				configuration.Services.AddService<IFluentMerger>(FluentMerger.Default);
				configuration.Services.AddService<IFluentDeserializerContainer>(FluentDeserializerContainer.Default);

				configuration.Options.Add(
					new FluentProviderOptions()
					{
						PollingInterval = TimeSpan.FromSeconds(5)
					});

				return configuration;
			}
		}

		public FluentConfiguration AddLocalFile(string path)
		{
			Providers.Add(new FluentLocalizedLocalFileProvider(path, this));
			return this;
		}

		public FluentConfiguration AddRemoteFile(string path, HttpClient client = null)
		{
			client ??= Services.GetOrAddService(() => new HttpClient());
			Providers.Add(new FluentLocalizedHttpFileProvider(path, client, this));
			return this;
		}
	}
}
