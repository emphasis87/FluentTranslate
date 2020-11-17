using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentTranslate.Domain;

namespace FluentTranslate
{
	public interface IFluentProvider
	{
		FluentResource Resource { get; }
		IObservable<FluentResource> ResourceObservable { get; }
	}

	public class FluentFileProvider : 
	{

	}

	public class CompositeFluentProvider : IObservable<FluentResource>
	{
		private readonly FluentProviderCombinator _combinator =
			new FluentProviderCombinator();

		private readonly HashSet<IFluentProvider> _providers =
			new HashSet<IFluentProvider>();

		public CompositeFluentProvider()
		{
			
		}

		public void Add(IFluentProvider provider)
		{
			lock (_providers)
			{
				if (_providers.Add(provider))
					return;

				_combinator.OnNext(_providers.ToArray());
			}
		}

		public IDisposable Subscribe(IObserver<FluentResource> observer)
		{
			observer.OnNext();
		}
	}

	public class FluentProviderCombinator : IObserver<IFluentProvider[]>
	{
		public void OnCompleted()
		{
			throw new NotImplementedException();
		}

		public void OnError(Exception error)
		{
			throw new NotImplementedException();
		}

		public void OnNext(IFluentProvider[] value)
		{
			throw new NotImplementedException();
		}
	}
}
