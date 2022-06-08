using Microsoft.Extensions.Logging;

namespace FluentTranslate.Services
{
    public interface IFluentProvider
    {
        string Name { get; }
    }

    public abstract class FluentProvider<T> : FluentService<T>, IFluentProvider
        where T : class, IFluentProvider
    {
        public abstract string Name { get; }

        protected FluentProvider(ILogger<T> logger = null)
            : base(logger)
        {

        }
    }
}
