namespace FluentTranslate.Infrastructure
{
	public interface IFluentServiceContainer : IServiceProvider
	{
		void AddService<T>(T service);
		void AddService(Type serviceType, object service);
		void RemoveService(Type serviceType);
	}

	public class FluentServiceContainer : IFluentServiceContainer
	{
		private readonly Dictionary<Type, object> _services =
			new Dictionary<Type, object>();

		public object GetService(Type serviceType)
		{
			return _services.TryGetValue(serviceType, out var service) ? service : default;
		}

		public void AddService<T>(T service)
		{
			AddService(typeof(T), service);
		}

		public void AddService(Type serviceType, object service)
		{
			_services[serviceType] = service;
		}

		public void RemoveService(Type serviceType)
		{
			_services.Remove(serviceType);
		}
	}

	public static class ServiceProviderExtensions
	{
		public static T GetService<T>(this IServiceProvider serviceProvider)
		{
			var service = serviceProvider?.GetService(typeof(T));
			return service is T result ? result : default;
		}

		public static T GetOrAddService<T>(this IFluentServiceContainer serviceContainer, Func<T> factory)  
			where T : class
		{
			if (serviceContainer is null)
				throw new ArgumentNullException(nameof(serviceContainer));

			var service = serviceContainer?.GetService(typeof(T));
			if (service is T result)
				return result;

			var nextResult = factory();
			serviceContainer.AddService(nextResult);
			return nextResult;
		}
	}
}
