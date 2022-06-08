using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using FluentTranslate.Domain;
using FluentTranslate.Infrastructure;
using FluentTranslate.Services;

namespace FluentTranslate
{
    public interface IFluentConfiguration
	{
		IFluentCompositeResourceProvider Providers { get; }
		IFluentOptionsContainer Options { get; }
		IFluentServiceContainer Services { get; }
	}

	public class FluentConfiguration : IFluentConfiguration
	{
		public IFluentServiceContainer Services { get; }
		public IFluentOptionsContainer Options { get; }
		public IFluentCompositeResourceProvider Providers { get; }

		public FluentConfiguration()
		{
			Services = new FluentServiceContainer();
			Options = new FluentOptionsContainer();
			Providers = new FluentCompositeResourceProvider(this) {IsLocalized = true};
		}

		public static FluentConfiguration Default
		{
			get
			{
				var configuration = new FluentConfiguration();

				configuration.Services.AddService<IFluentConfiguration>(configuration);
				configuration.Services.AddService<IFluentResourceProvider>(configuration.Providers);
				configuration.Services.AddService<IFluentCloneFactory>(FluentCloneFactory.Default);
				configuration.Services.AddService<IFluentMerger>(new FluentMerger(configuration));
				configuration.Services.AddService<IFluentDeserializerContainer>(FluentDeserializerContainer.Default);
				configuration.Services.AddService<ICurrentCultureProvider>(new CurrentCultureProvider());

				configuration.Services.AddService(new HttpClient());

				configuration.Options.Add(
					new FluentResourceProviderOptions()
					{
						PollingInterval = TimeSpan.FromSeconds(1),
						FilePollingInterval = TimeSpan.FromSeconds(5),
						HttpPollingInterval = TimeSpan.FromMinutes(1),
					});

				return configuration;
			}
		}

		public virtual FluentConfiguration AddLocalFile(string path)
		{
			Providers.Add(new FluentLocalizedLocalFileResourceProvider(path, this));
			return this;
		}

		public virtual FluentConfiguration AddRemoteFile(string path, HttpClient client = null)
		{
			Providers.Add(new FluentLocalizedHttpFileResourceProvider(path, client, this));
			return this;
		}

		public virtual FluentConfiguration AddEmbeddedResource(Assembly assembly, string path)
		{
			Providers.Add(new FluentLocalizedEmbeddedResourceProvider(assembly, path, this));
			return this;
		}

		public FluentConfiguration AddFiles(IEnumerable<string> paths)
		{
			foreach (var path in paths)
			{
				AddFile(path);
			}

			return this;
		}

		public virtual FluentConfiguration AddFile(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				throw new ArgumentOutOfRangeException(nameof(path), "Unable to add null or whitespace path.");

			try
			{
				var uri = new Uri(path, UriKind.RelativeOrAbsolute);
				var scheme = uri.Scheme;
				if (scheme.StartsWith("http"))
					AddRemoteFile(path);

				return this;
			}
			catch (InvalidOperationException)
			{

			}

			try
			{
				var _ = new FileInfo(path);
				return this;
			}
			catch (ArgumentException)
			{
				throw new ArgumentException($"Unable to add file: '{path}'.", nameof(path));
			}
			catch (Exception)
			{

			}

			AddLocalFile(path);
			return this;
		}
	}

	public static class FluentConfigurationExtensions
	{
		public static FluentResource Deserialize(this IFluentConfiguration configuration, string content)
		{
			var resource = configuration.Deserialize(content, "ftl");
			return resource;
		}

		public static FluentResource Deserialize(this IFluentConfiguration configuration, string content, string extension)
		{
			if (content is null) 
				return null;

			var deserializers = configuration?.Services.GetService<IFluentDeserializerContainer>()
				?? FluentDeserializerContainer.Default;
			var deserializer = deserializers.Get(extension);
			if (deserializer is null)
				throw new InvalidOperationException($"Extension '{extension}' is not supported for deserialization.");

			var resource = deserializer.Deserialize(content);
			return resource;
		}
	}
}
