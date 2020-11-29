using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentTranslate.Domain;
using static FluentTranslate.Infrastructure.EqualityHelper;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentCompositeResourceProvider : IFluentResourceProvider
	{
		void Add(IFluentResourceProvider provider);
		void Remove(IFluentResourceProvider provider);
	}

	public class FluentCompositeResourceProvider : FluentPollingResourceProvider, IFluentCompositeResourceProvider
	{
		private readonly object _lock = new object();

		protected IFluentMerger Merger =>
			Configuration?.Services.GetService<IFluentMerger>() ?? FluentMerger.Default;

		private List<IFluentResourceProvider> _providers = new List<IFluentResourceProvider>();

		public FluentCompositeResourceProvider(IFluentConfiguration configuration = null)
			: base(configuration)
		{
			
		}

		public void Add(IFluentResourceProvider provider)
		{
			lock (_lock)
			{
				_providers = _providers.Union(new[] { provider }).ToList();
			}
			foreach (var context in GetContexts())
			{
				context.LastPolled = null;
			}
		}

		public void Remove(IFluentResourceProvider provider)
		{
			lock (_lock)
			{
				_providers = _providers.Union(new[] { provider }).ToList();
			}
			foreach (var context in GetContexts())
			{
				context.LastPolled = null;
			}
		}

		protected override async Task<FluentResource> FindResourceAsync(Context context, CultureInfo culture)
		{
			var ctx = (CompositeContext) context;

			var tasks = _providers.Select(provider => 
				provider.GetResourceAsync(culture));

			var results = await Task.WhenAll(tasks);
			if (AreEqual(results, ctx.LastResults))
				return null;

			var result = Merger.Combine(results);
			return result;
		}

		protected override Context CreateContext(CultureInfo culture)
		{
			return new CompositeContext(culture);
		}

		protected class CompositeContext : Context
		{
			public List<IFluentResourceProvider> Providers;
			public FluentResource[] LastResults;

			public CompositeContext(CultureInfo culture) : base(culture)
			{
				Providers = new List<IFluentResourceProvider>();
				LastResults = new FluentResource[0];
			}
		}
	}
}