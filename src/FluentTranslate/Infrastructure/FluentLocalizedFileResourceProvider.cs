using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public abstract class FluentLocalizedFileResourceProvider : FluentPollingResourceProvider
	{
		public string RootPath { get; }

		private readonly ConcurrentDictionary<string, IFluentResourceProvider> _providerByPath = 
			new ConcurrentDictionary<string, IFluentResourceProvider>();

		protected FluentLocalizedFileResourceProvider(string rootPath, IFluentConfiguration configuration = null) 
			: base(configuration)
		{
			RootPath = rootPath;

			IsLocalized = true;
		}

		protected virtual IEnumerable<string> GetPaths(CultureInfo culture)
		{
			var extension = Path.GetExtension(RootPath);
			if (!string.IsNullOrWhiteSpace(culture.Name))
				yield return Path.ChangeExtension(RootPath, $"{culture.Name}{extension}");
			yield return Path.ChangeExtension(RootPath, $"{culture.TwoLetterISOLanguageName}{extension}");
			yield return RootPath;
		}

		protected virtual IFluentCompositeResourceProvider CreateProvider(CultureInfo culture)
		{
			var compositeProvider = new FluentCompositeResourceProvider(Configuration);
			var paths = GetPaths(culture).Distinct().ToArray();
			foreach (var path in paths)
			{
				if (!_providerByPath.TryGetValue(path, out var provider))
				{
					provider = CreateProvider(path);
					provider = _providerByPath.GetOrAdd(path, provider);
				}
				compositeProvider.Add(provider);
			}

			return compositeProvider;
		}

		protected abstract IFluentResourceProvider CreateProvider(string path);
		
		protected override async Task<FluentResource> FindResourceAsync(Context context, CultureInfo culture)
		{
			var ctx = (LocalizedFileContext) context;
			var compositeProvider = ctx.Provider;
			var next = await compositeProvider.GetResourceAsync(culture);
			return next;
		}

		protected override Context CreateContext(CultureInfo culture)
		{
			var compositeProvider = CreateProvider(culture);
			return new LocalizedFileContext(compositeProvider, culture);
		}

		protected class LocalizedFileContext : Context
		{
			public IFluentCompositeResourceProvider Provider { get; }

			public LocalizedFileContext(IFluentCompositeResourceProvider provider, CultureInfo culture) : base(culture)
			{
				Provider = provider;
			}
		}
	}
}