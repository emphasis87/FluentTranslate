using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public abstract class FluentLocalizedFileProvider : FluentLocalizedProvider
	{
		public string RootPath { get; }

		protected Dictionary<CultureInfo, IFluentCompositeProvider> ProviderByCulture { get; }
		protected Dictionary<string, IFluentProvider> ProviderByPath { get; }

		protected FluentLocalizedFileProvider(string rootPath, IFluentConfiguration configuration) 
			: base(configuration)
		{
			RootPath = rootPath;
			ProviderByCulture = new Dictionary<CultureInfo, IFluentCompositeProvider>();
			ProviderByPath = new Dictionary<string, IFluentProvider>();
		}

		protected virtual IEnumerable<string> GetPaths(CultureInfo culture)
		{
			var extension = Path.GetExtension(RootPath);
			yield return Path.ChangeExtension(RootPath, $"{culture.Name}{extension}");
			yield return Path.ChangeExtension(RootPath, $"{culture.TwoLetterISOLanguageName}{extension}");
			yield return RootPath;
		}

		protected virtual IFluentCompositeProvider CreateProvider(CultureInfo culture)
		{
			return new FluentInvariantCompositeProvider(Configuration);
		}

		protected abstract IFluentProvider CreateProvider(string path);
		
		protected override async Task<Timestamped<FluentResource>> FindResourceAsync(DateTime now, Timestamped<FluentResource> lastResult, CultureInfo culture)
		{
			if (!ProviderByCulture.TryGetValue(culture, out var compositeProvider))
			{
				compositeProvider = CreateProvider(culture);
				var paths = GetPaths(culture);
				foreach (var path in paths)
				{
					if (!ProviderByPath.TryGetValue(path, out var provider))
					{
						provider = CreateProvider(path);
						ProviderByPath[path] = provider;
					}
					compositeProvider.Add(provider);
				}
				ProviderByCulture[culture] = compositeProvider;
			}

			var next = await compositeProvider.GetResourceAsync(culture);
			return next;
		}
	}
}