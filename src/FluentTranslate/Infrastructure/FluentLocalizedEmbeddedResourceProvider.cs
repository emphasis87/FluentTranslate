using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public class FluentLocalizedEmbeddedResourceProvider : FluentPollingResourceProvider
	{
		public Assembly RootAssembly { get; }
		public string RootPath { get; }

		private readonly ConcurrentDictionary<(Assembly, string), IFluentResourceProvider> _providerByAssemblyAndPath =
			new ConcurrentDictionary<(Assembly, string), IFluentResourceProvider>();

		public FluentLocalizedEmbeddedResourceProvider(Assembly assembly, string rootPath, IFluentConfiguration configuration = null)
			: base(configuration)
		{
			RootAssembly = assembly;
			RootPath = rootPath;

			IsLocalized = true;
			IsPollingEnabled = false;
		}

		protected virtual IEnumerable<(Assembly, string)> GetPaths(CultureInfo culture)
		{
			var extension = Path.GetExtension(RootPath);

			if (!string.IsNullOrWhiteSpace(culture.Name))
			{
				var pathWithLongCulture = Path.ChangeExtension(RootPath, $"{culture.Name}{extension}");
				yield return (RootAssembly, pathWithLongCulture);
			}

			var pathWithShortCulture = Path.ChangeExtension(RootPath, $"{culture.TwoLetterISOLanguageName}{extension}");
			yield return (RootAssembly, pathWithShortCulture);

#if NETFRAMEWORK || NETSTANDARD2_0
			Assembly satellite = null;
			try
			{
				satellite = RootAssembly.GetSatelliteAssembly(culture);
			}
			catch { }
			
			if (satellite != null)
				yield return (satellite, RootPath);
#endif

			yield return (RootAssembly, RootPath);
		}

		protected virtual IFluentCompositeResourceProvider CreateProvider(CultureInfo culture)
		{
			var compositeProvider = new FluentCompositeResourceProvider(Configuration);
			var parameters = GetPaths(culture).Distinct().ToArray();
			foreach (var (assembly, path) in parameters)
			{
				if (!_providerByAssemblyAndPath.TryGetValue((assembly, path), out var provider))
				{
					provider = CreateProvider(assembly, path);
					provider = _providerByAssemblyAndPath.GetOrAdd((assembly, path), provider);
				}
				compositeProvider.Add(provider);
			}

			return compositeProvider;
		}

		protected virtual IFluentResourceProvider CreateProvider(Assembly assembly, string path)
		{
			return new FluentEmbeddedResourceProvider(assembly, path, Configuration);
		}

		protected override async Task<FluentResource> FindResourceAsync(Context context, CultureInfo culture)
		{
			var ctx = (LocalizedFileContext)context;
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
