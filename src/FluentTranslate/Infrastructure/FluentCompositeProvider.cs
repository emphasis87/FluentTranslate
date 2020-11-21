using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentTranslate.Domain;
using static FluentTranslate.Infrastructure.EqualityHelper;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentCompositeProvider : IFluentProvider
	{
		void Add(IFluentProvider provider);
		void Remove(IFluentProvider provider);
	}

	public class FluentCompositeProvider : FluentProvider, IFluentCompositeProvider
	{
		private readonly object _lock = new object();

		protected IFluentMerger Merger =>
			Configuration.Services.GetService<IFluentMerger>() ?? FluentMerger.Default;

		private List<IFluentProvider> _providers = new List<IFluentProvider>();

		public FluentCompositeProvider(IFluentConfiguration configuration)
			: base(configuration)
		{
			
		}

		public void Add(IFluentProvider provider)
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

		public void Remove(IFluentProvider provider)
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
			public List<IFluentProvider> Providers;
			public FluentResource[] LastResults;

			public CompositeContext(CultureInfo culture) : base(culture)
			{
				Providers = new List<IFluentProvider>();
				LastResults = new FluentResource[0];
			}
		}
	}
}