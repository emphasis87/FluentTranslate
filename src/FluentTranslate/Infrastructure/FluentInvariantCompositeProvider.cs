using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentCompositeProvider : IFluentProvider
	{
		void Add(IFluentProvider provider);
		void Remove(IFluentProvider provider);
	}

	public class FluentInvariantCompositeProvider : FluentInvariantProvider, IFluentCompositeProvider
	{
		protected IFluentMerger Merger =>
			Configuration.Services.GetService<IFluentMerger>() ?? FluentMerger.Default;

		protected List<IFluentProvider> Providers { get; } =
			new List<IFluentProvider>();

		protected DateTime LastModified { get; private set; }

		public FluentInvariantCompositeProvider(IFluentConfiguration configuration) 
			: base(configuration)
		{
			LastModified = DateTime.Now;
		}

		protected override async Task<Timestamped<FluentResource>> FindResourceAsync(DateTime now, Timestamped<FluentResource> lastResult, CultureInfo culture)
		{
			if (Providers.Count == 0)
				return new Timestamped<FluentResource>(LastModified, null);

			var queries = Providers.Select(x => x.GetResourceAsync(culture)).ToArray();
			var resources = await Task.WhenAll(queries);

			var nextResource = Merger.Combine(resources.Select(x => x.Value));
			var nextTimestamp = resources.Max(x => x.Timestamp);

			// Next timestamp must be at lest as old as last modification co providers collection
			// because their ordering may have changed
			if (nextTimestamp < LastModified)
				nextTimestamp = LastModified;

			return new Timestamped<FluentResource>(nextTimestamp, nextResource);
		}

		public void Add(IFluentProvider provider)
		{
			Semaphore.Wait();
			try
			{
				if (!Providers.Contains(provider))
				{
					Providers.Add(provider);
					LastModified = DateTime.Now;
					SetLastPolled(default);
				}
			}
			finally
			{
				Semaphore.Release();
			}
		}

		public void Remove(IFluentProvider provider)
		{
			Semaphore.Wait();
			try
			{
				if (Providers.Remove(provider))
				{
					LastModified = DateTime.Now;
					SetLastPolled(default);
				}
			}
			finally
			{
				Semaphore.Release();
			}
		}
	}
}